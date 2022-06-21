﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using Newtonsoft.Json;
using SharpWrap2534.SupportingLogic;

namespace SharpWrap2534.PassThruTypes
{
    /// <summary>
    /// Set of structure objects without signed value sets.
    /// </summary>
    public class PassThruStructs
    {
        /// <summary>
        /// PassThur message struct
        /// </summary>
        [JsonConverter(typeof(PtMsgConverter))]
        public struct PassThruMsg
        {
            public ProtocolId ProtocolId;
            public uint RxStatus;
            public uint TxFlags;
            public uint Timestamp;
            public uint DataSize;
            public uint ExtraDataIndex;
            public byte[] Data;
            
            /// <summary>
            /// Builds a new PassThru message from managed types.
            /// </summary>
            /// <param name="ByteCount"></param>
            public PassThruMsg(uint ByteCount = 0)
            {
                ProtocolId = 0;
                RxStatus = 0;
                TxFlags = 0;
                Timestamp = 0;
                DataSize = 0;
                ExtraDataIndex = 0;
                
                // Build String Data and Bytes Value;
                Data = new byte[ByteCount];
            }

            /// <summary>
            /// Converts the Message Data into a hex string with the formatting requested
            /// </summary>
            /// <returns></returns>
            public string DataToAsciiString()
            {
                // Convert the data into the given format here.
                if (this.Data.All(ByteObj => ByteObj == 0x00)) return "No Data!";
                string AsciiString = Encoding.Default.GetString(this.Data);
                return AsciiString;
            }
            /// <summary>
            /// Converts message data output into a custom string of Hex Values
            /// </summary>
            /// <param name="Use0x">Sets if we should use 0x or not.</param>
            /// <returns></returns>
            public string DataToHexString(bool Use0x = false)
            {
                // Ensure we have data contents here
                if (this.Data.All(ByteObj => ByteObj == 0x00)) return "No Data!";

                // Convert to a string Array by splitting on '-'
                string[] BytesAsStrings = BitConverter
                    .ToString(this.Data ?? Array.Empty<byte>())
                    .Split('-');

                // If not using 0x, then just return the split values
                if (!Use0x) { return string.Join(" ", BytesAsStrings); }
                BytesAsStrings = BytesAsStrings.Select(ByteString => $"0x{ByteString}").ToArray();
                return string.Join(" ", BytesAsStrings);
            }
        }
        /// <summary>
        /// SConfig list setup
        /// </summary>
        public struct SConfigList
        {
            public uint NumberOfParams;
            public List<SConfig> ConfigList;

            /// <summary>
            /// Builds a new managed SConfig List
            /// </summary>
            public SConfigList(uint ParamCount = 0)
            {
                ConfigList = new List<SConfig>();
                NumberOfParams = ParamCount;
            }
        }
        /// <summary>
        /// Single SConfig instance.
        /// </summary>
        public struct SConfig
        {
            public uint SConfigValue;
            public ConfigParamId SConfigParamId;

            /// <summary>
            /// Builds a new Sconfig Param
            /// </summary>
            /// <param name="ConfigParam"></param>
            public SConfig(ConfigParamId ConfigParam)
            {
                SConfigParamId = ConfigParam;
                SConfigValue = (uint)ConfigParam;
            }
        }
        /// <summary>
        /// SByte array. Used for sending sets of SConfigs.
        /// </summary>
        public struct SByteArray
        {
            public uint NumberOfBytes;
            public byte[] Data;

            /// <summary>
            /// Builds a new SByte Array
            /// </summary>
            /// <param name="ByteCount"></param>
            public SByteArray(uint ByteCount)
            {
                NumberOfBytes = ByteCount;
                Data = new byte[ByteCount];
            }
        }
        /// <summary>
        /// SDevice instances show all the values of a device when a PTOpen command is run.
        /// </summary>
        public struct SDevice
        {
            public string DeviceName;
            public uint DeviceAvailable;
            public uint DeviceDllFWStatus;
            public uint DeviceConnectMedia;
            public uint DeviceConnectSpeed;
            public uint DeviceSignalQuality;
            public uint DeviceSignalStrength;

            // to control what shows up in the combobox
            public override string ToString() { return DeviceName; }
        };
        /// <summary>
        /// Resource structs show used controls on a connector instance type for a device.
        /// </summary>
        public struct ResourceStruct
        {
            public Connector ConnectorType;
            public uint ResourceCount;
            public List<int> ResourceList;

            /// <summary>
            /// Builds a new resource struct object
            /// </summary>
            /// <param name="NumResources"></param>
            public ResourceStruct(int NumResources = 0)
            {
                ResourceCount = (uint)NumResources;
                ResourceList = new List<int>(NumResources);
                ConnectorType = default;
            }
        }

        // ---------------------------------------------- VERSION 0500 USE CASES ONLY! ----------------------------------------

        /// <summary>
        /// ISO15765 Channel Descriptor for Logical commands
        /// </summary>
        public struct ISO15765ChannelDescriptor
        {
            public uint LocalTxFlags;
            public uint RemoteTxFlags;
            public byte[] LocalAddress;
            public byte[] RemoteAddress;

            /// <summary>
            /// Builds a new Channel descriptor
            /// </summary>
            public ISO15765ChannelDescriptor(byte[] LocalAddress, byte[] RemoteAddress, uint LocalFlags, uint RemoteFlags)
            {
                // Store the values here.
                this.LocalAddress = LocalAddress;
                this.RemoteAddress = RemoteAddress;
                this.LocalTxFlags = LocalFlags;
                this.RemoteTxFlags = RemoteFlags;
            }
        }
        /// <summary>
        /// SChannel set object for logical operations
        /// </summary>
        public struct SChannelSet
        {
            public uint ChannelCount;
            public uint ChannelThreshold;
            public List<int> ChannelList;

            /// <summary>
            /// Builds a new Channel set object
            /// </summary>
            /// <param name="ChannelCount"></param>
            public SChannelSet(uint ChannelCount, uint ChannelThreshold)
            {
                // Store the values here.
                this.ChannelList = new List<int>();
                this.ChannelCount = ChannelCount;
                this.ChannelThreshold = ChannelThreshold;
            }
        }
    }
}
