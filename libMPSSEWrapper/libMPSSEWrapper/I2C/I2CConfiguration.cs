﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libMPSSEWrapper.I2C
{
    public class I2CConfiguration
    {
        public static readonly I2C.I2CConfiguration ChannelZeroConfiguration = new I2C.I2CConfiguration(0);

        public int ChannelIndex { get; private set; }

        public I2CConfiguration(int channelIndex)
        {
            ChannelIndex = channelIndex;
        }

    }
}
