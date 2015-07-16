using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using libMPSSEWrapper.Exceptions;
using libMPSSEWrapper.Types;

namespace libMPSSEWrapper.I2C
{
    public class I2CDevice
    {
        private static IntPtr _handle = IntPtr.Zero;
        private static FtChannelConfig _currentGlobalConfig;

        private FtChannelConfig _cfg;

        private bool _isDisposed;
        private I2CConfiguration _i2cConfig;

        public I2CDevice(FtChannelConfig config)
        {
            _i2cConfig = _i2cConfig ?? I2CConfiguration.ChannelZeroConfiguration;
            _cfg = config;
            InitLibAndHandle();
        }

        void InitLibAndHandle()
        {
            FtResult result;
            if (_handle != IntPtr.Zero)
                return;


            LibMpsse.Init();
            var num_channels = 0;

            var channels = LibMpsseI2C.I2C_GetNumChannels(out num_channels);

            CheckResult(channels);

            if (num_channels > 0)
            {
                for (var i = 0; i < num_channels; i++)
                {
                    FtDeviceInfo cInfo;
                    var channelInfoStatus = LibMpsseI2C.I2C_GetChannelInfo(i, out cInfo);
                    CheckResult(channelInfoStatus);
                    Debug.WriteLine($"Flags: {cInfo.Flags}");
                    Debug.WriteLine($"Type: {cInfo.Type}");
                    Debug.WriteLine($"ID: {cInfo.ID}");
                    Debug.WriteLine($"LocId: {cInfo.LocId}");
                    Debug.WriteLine($"SerialNumber: {cInfo.SerialNumber}");
                    Debug.WriteLine($"Description: {cInfo.Description}");
                    Debug.WriteLine($"ftHandle: {cInfo.ftHandle}");
                }
            }

            result = LibMpsseI2C.I2C_OpenChannel(_i2cConfig.ChannelIndex, out _handle);

            CheckResult(result);

            if (_handle == IntPtr.Zero)
                throw new I2CChannelNotConnectedException(FtResult.InvalidHandle);

            result = LibMpsseI2C.I2C_InitChannel(_handle, ref _cfg);

            CheckResult(result);
            _currentGlobalConfig = _cfg;

        }

        protected FtResult Write(int deviceAddress, byte[] buffer, int sizeToTransfer, out int sizeTransfered, FtI2CTransferOptions options)
        {
            return LibMpsseI2C.I2C_DeviceWrite(_handle, deviceAddress, sizeToTransfer, buffer, out sizeTransfered, options);
        }

        protected FtResult Read(int deviceAddress, byte[] buffer, int sizeToTransfer, out int sizeTransfered, FtI2CTransferOptions options)
        {
            //EnforceRightConfiguration();
            return LibMpsseI2C.I2C_DeviceRead(_handle, deviceAddress, sizeToTransfer, buffer, out sizeTransfered, options);
        }

        protected static void CheckResult(FtResult result)
        {
            if (result != FtResult.Ok)
                throw new I2CChannelNotConnectedException(result);
        }

        //public int GetNumberOfChannels()
        //{
        //    IntPtr channels;
        //    //var reuslt = LibMpsseI2C.I2C_GetNumChannels(0, out channels);

        //    return channels.ToInt32();
        //}
    }
}
