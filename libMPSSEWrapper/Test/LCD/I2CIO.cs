using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using libMPSSEWrapper.I2C;
using libMPSSEWrapper.Types;

namespace Test.LCD
{
    public class I2CIO
    {
        int _shadow;      // Shadow output
        int _dirMask;     // Direction mask
        int _i2cAddr;     // I2C address
        private bool _initialised;

        private I2CDevice _i2cDevice;

        private const int ConnectionSpeed = 40000; // Hz
        private const int LatencyTimer = 255; // Hz


        public I2CIO()
        {
            _i2cAddr = 0x0;
            _dirMask = 0xFF;    // mark all as INPUTs
            _shadow = 0x0;     // no values set
            _initialised = false;
        }

        public int Begin(int address)
        {
            if (_i2cDevice == null)
            {
                var adci2cConfig = new FtChannelConfig
                {
                    ClockRate = ConnectionSpeed,
                    LatencyTimer = LatencyTimer
                };

                if (_i2cDevice == null)
                {
                    _i2cDevice = new I2CDevice(adci2cConfig, address);

                    var b = _i2cDevice.Read(1);

                    if (b == null)
                    {
                        Debugger.Break();
                    }

                    _shadow = Convert.ToInt32(b[0]);
                    _initialised = true;
                }
            }

            return 1;
        }

        public void PinMode(int pin, int dir)
        {
            if (_initialised)
            {
                if (Constants.OUTPUT == dir)
                {
                    _dirMask &= ~(1 << pin);
                }
                else
                {
                    _dirMask |= (1 << pin);
                }
            }
        }

        public void PortMode(int dir)
        {
            if (_initialised)
            {
                if (dir == Constants.INPUT)
                {
                    _dirMask = 0xFF;
                }
                else
                {
                    _dirMask = 0x00;
                }
            }
        }

        //public int Read()
        //{
            
        //}

       
        public int Write(int value)
        {
            if (_initialised)
            {
                // Only write HIGH the values of the ports that have been initialised as
                // outputs updating the output shadow of the device
                _shadow = (value & ~(_dirMask));

                var result = _i2cDevice.Write(_shadow);
                
            }
            return 1;
        }

        //void portMode ( uint8_t dir );

        //void pinMode ( uint8_t pin, uint8_t dir );
    }
}
