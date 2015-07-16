using System;
using System.Diagnostics;
using libMPSSEWrapper;
using libMPSSEWrapper.I2C;
using libMPSSEWrapper.Types;
using Test.LCD;

namespace Test
{
    class Program
    {
        private const int ConnectionSpeed = 40000; // Hz
        private const int LatencyTimer = 255; // Hz

        static void Main(string[] args)
        {

            var start = 0x21;
            var end = 0x28;

            for (var address = start; address < end; address++)
            {
                Console.WriteLine(address);
                var lcd = new I2CLCD(address, 2, 1, 0, 4, 5, 6, 7, 3, BacklightPolarity.Positive);
                lcd.Begin(16, 2);
                //for (int i = 0; i < 3; i++)
                //{
                //    lcd.BackLight();
                //    StopwatchDelay.Delay(250);
                //    lcd.NoBacklight();
                //    StopwatchDelay.Delay(250);
                //}

                lcd.BackLight();

                lcd.SetCursor(0, 0); //Start at character 4 on line 0
                lcd.Write("Hello, world!");
                StopwatchDelay.Delay(250);
                lcd.SetCursor(0, 1);
                lcd.Write("HI!YourDuino.com");
                

            }

            Console.WriteLine("Finished");


            //var adcSpiConfig = new FtChannelConfig
            //{
            //    ClockRate = ConnectionSpeed,
            //    LatencyTimer = LatencyTimer,
            //    configOptions = FtConfigOptions.Mode0 | FtConfigOptions.CsDbus3 | FtConfigOptions.CsActivelow
            //};
            // var c = new I2CDevice(adcSpiConfig);

            //var b = new byte[1];
            //b[0] = 0x02;
            //int transferred;



            //var result = c.Write(0x27, b, 0, out transferred, FtI2CTransferOptions.FastTransfer);

            Console.Read();
            //  Debug.WriteLine(result);

            return;


            //var adcConfig = new Maxim186Configuration
            //                    {
            //                        Channel = Maxim186.Channel.Channel0,
            //                        ConversionType = Maxim186.ConversionType.SingleEnded,
            //                        Polarity = Maxim186.Polarity.Unipolar,
            //                        PowerMode = Maxim186.PowerMode.InternalClockMode
            //                    };



            //var adc = new Maxim186(adcConfig, adcSpiConfig);

            //do
            //{
            //    Console.WriteLine(adc.GetConvertedSample());



            //} while (true);
            //*/
        }
    }
}
