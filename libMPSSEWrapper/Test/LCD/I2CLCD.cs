using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using libMPSSEWrapper.I2C;
using libMPSSEWrapper.Types;

namespace Test.LCD
{
    public class I2CLCD : LCD
    {
        private const int LCD_NOBACKLIGHT = 0x00;
        private const int LCD_BACKLIGHT = 0xff;

        private const int EN = 6;
        private const int RW = 5;
        private const int RS = 4;
        private const int D4 = 0;
        private const int D5 = 1;
        private const int D6 = 2;
        private const int D7 = 3;

        private int _address;
        private int _backlightPinMask;
        private int _backlightStsMask;

        private I2CDevice _i2cio;

        private int _en;
        private int _rw;
        private int _rs;
        int[] _dataPins = new int[4];

        private const int ConnectionSpeed = 40000; // Hz
        private const int LatencyTimer = 255; // Hz

        public I2CLCD(int address)
        {
            _config(address, EN, RW, RS, D4, D5, D6, D7);
        }

        public I2CLCD(int address, int backlightPin, BacklightPolarity pol)
        {
            _config(address, EN, RW, RS, D4, D5, D6, D7);
            SetBacklightPin(backlightPin, pol);
        }

        public I2CLCD(int address, int en, int rw, int rs)
        {
            _config(address, en, rw, rs, D4, D5, D6, D7);
        }

        public I2CLCD(int address, int en, int rw, int rs, int backlightPin, BacklightPolarity pol)
        {
            _config(address, en, rw, rs, D4, D5, D6, D7);
            SetBacklightPin(backlightPin, pol);
        }

        public I2CLCD(int address, int en, int rw, int rs, int d4, int d5, int d6, int d7)
        {
            _config(address, en, rw, rs, d4, d5, d6, d7);
        }

        public I2CLCD(int address, int en, int rw, int rs, int d4, int d5, int d6, int d7, int backlightPin, BacklightPolarity pol)
        {
            _config(address, en, rw, rs, d4, d5, d6, d7);
            SetBacklightPin(backlightPin, pol);
        }

        private int _init()
        {
            var adci2cConfig = new FtChannelConfig
            {
                ClockRate = ConnectionSpeed,
                LatencyTimer = LatencyTimer
            };

            _i2cio  = new I2CDevice(adci2cConfig, _address);

            _i2cio.Write(0);  // Set the entire port to LOW

            return 1;
        }

        private void _config(int address, int en, int rw, int rs, int d4, int d5, int d6, int d7)
        {
            _address = address;

            _backlightPinMask = 0;
            _backlightStsMask = LCD_NOBACKLIGHT;
            _polarity = BacklightPolarity.Positive;

            _en = (1 << en);
            _rw = (1 << rw);
            _rs = (1 << rs);

            // Initialise pin mapping
            _dataPins[0] = (1 << d4);
            _dataPins[1] = (1 << d5);
            _dataPins[2] = (1 << d6);
            _dataPins[3] = (1 << d7);

        }

        private void write4bits(int value, int mode)
        {
            int pinMapValue = 0;

            // Map the value to LCD pin mapping
            // --------------------------------
            for (int i = 0; i < 4; i++)
            {
                if ((value & 0x1) == 1)
                {
                    pinMapValue |= _dataPins[i];
                }
                value = (value >> 1);
            }

            // Is it a command or data
            // -----------------------
            if (mode == Constants.DATA)
            {
                mode = _rs;
            }

            pinMapValue |= mode | _backlightStsMask;
            _pulseEnable(pinMapValue);
        }

        private void _pulseEnable(int data)
        {
            _i2cio.Write(data | _en);   // En HIGH
            _i2cio.Write(data & ~_en);  // En LOW
        }

        public override void Begin(int cols, int rows, int charSize = Constants.LCD_5x8DOTS)
        {
            _init();
            base.Begin(cols, rows, charSize);
        }


        protected override void Send(int value, int mode)
        {
            if (mode == Constants.FOUR_BITS)
            {
                write4bits((value & 0x0F), Constants.COMMAND);
            }
            else
            {
                write4bits((value >> 4), mode);
                write4bits((value & 0x0F), mode);
            }
        }

        public override void SetBacklight(int value)
        {
            if (_backlightPinMask != 0x0)
            {
                // Check for polarity to configure mask accordingly
                // ----------------------------------------------------------
                if (((_polarity == BacklightPolarity.Positive) && (value > 0)) ||
                     ((_polarity == BacklightPolarity.Negative) && (value == 0)))
                {
                    _backlightStsMask = _backlightPinMask & LCD_BACKLIGHT;
                }
                else
                {
                    _backlightStsMask = _backlightPinMask & LCD_NOBACKLIGHT;
                }
                _i2cio.Write(_backlightStsMask);
            }
        }

        public override void SetBacklightPin(int value, BacklightPolarity pol)
        {
            _backlightPinMask = (1 << value);
            _polarity = pol;
            SetBacklight(Constants.BackLightOff);
        }

     
    }
}
