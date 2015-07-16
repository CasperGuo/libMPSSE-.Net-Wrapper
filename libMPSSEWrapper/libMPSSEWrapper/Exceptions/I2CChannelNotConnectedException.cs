using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libMPSSEWrapper.Types;

namespace libMPSSEWrapper.Exceptions
{
    public class I2CChannelNotConnectedException : Exception
    {
        public FtResult Reason { get; private set; }

        public I2CChannelNotConnectedException(FtResult res)
        {
            Reason = res;
        }


    }
}
