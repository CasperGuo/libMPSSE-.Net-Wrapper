﻿using System;
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
        private readonly int _deviceAddress;

        private bool _isDisposed;
        private I2CConfiguration _i2cConfig;

        public I2CDevice(FtChannelConfig config, int deviceAddress)
        {
            _i2cConfig = _i2cConfig ?? I2CConfiguration.ChannelZeroConfiguration;
            _cfg = config;
            _deviceAddress = deviceAddress;
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

        public bool Write(byte[] array)
        {
            int writtenAmount;

            var result = Write(array, array.Length, out writtenAmount, FtI2CTransferOptions.StartBit);

            return result == FtResult.Ok;
        }

        public bool Write(int value)
        {
            var array = new byte[1];
            array[0] = Convert.ToByte(value);
            //var array = BitConverter.GetBytes(Convert.ToUInt32(value));
            int writtenAmount;

            var result = Write(array, array.Length, out writtenAmount, FtI2CTransferOptions.StartBit | FtI2CTransferOptions.StopBit);

            return result == FtResult.Ok;
        }

        public FtResult Write(byte[] buffer, int sizeToTransfer, out int sizeTransfered, FtI2CTransferOptions options)
        {
            return LibMpsseI2C.I2C_DeviceWrite(_handle, _deviceAddress, sizeToTransfer, buffer, out sizeTransfered, options);
        }

        public FtResult Read(byte[] buffer, int sizeToTransfer, out int sizeTransfered, FtI2CTransferOptions options)
        {
            //EnforceRightConfiguration();
            return LibMpsseI2C.I2C_DeviceRead(_handle, _deviceAddress, sizeToTransfer, buffer, out sizeTransfered, options);
        }

        public byte[] Read(int sizeToTransfer)
        {
            byte[] buffer = new byte[1];
            int sizeTransfered = 0;
            var result = LibMpsseI2C.I2C_DeviceRead(
                _handle, _deviceAddress,
                sizeToTransfer, buffer, out sizeTransfered, FtI2CTransferOptions.StartBit);

            CheckResult(result);
            return buffer;
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
