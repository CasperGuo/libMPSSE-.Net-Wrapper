using System;
using System.Diagnostics;
using libMPSSEWrapper;
using libMPSSEWrapper.I2C;
using libMPSSEWrapper.Types;

namespace Test
{
    class Program
    {
        private const int ConnectionSpeed = 2000; // Hz
        private const int LatencyTimer = 255; // Hz

        static void Main(string[] args)
        {



            var adcSpiConfig = new FtChannelConfig
            {
                ClockRate = ConnectionSpeed,
                LatencyTimer = LatencyTimer,
                configOptions = FtConfigOptions.Mode0 | FtConfigOptions.CsDbus3 | FtConfigOptions.CsActivelow
            };
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
