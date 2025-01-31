﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpWrapper.PassThruTypes;

namespace SharpWrapper.PassThruSupport
{
    /// <summary>
    /// Json Conversion helper for PTMessages.
    /// Used mainly to write the data in 0x00 format to JSON and convert it back when pulled in.
    /// </summary>
    internal class PtMsgConverter : JsonConverter
    {
        // Sample JSON message object output
        /*
         * {
         *   ProtocolId: "ISO15765",
         *   RxStatus: "No RxStatus",
         *   TxFlags: "ISO15765_FRAME_PAD",
         *   Timestamp: "10ms",
         *   DataSize: 12 Bytes
         *   ExtraDataIndex: 0,
         *   Data: 0x00 0x00 0x07 0xDF 0x02 0x09 0x02 0x00 0x00 0x00 0x00 0x00
         * }
         */

        /// <summary>
        /// Sets if we can convert into JSON and from JSON or not.
        /// </summary>
        /// <param name="ObjectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type ObjectType) { return ObjectType == typeof(PassThruStructs.PassThruMsg); }

        /// <summary>
        /// Writes out a new PTMessage as JSON
        /// </summary>
        /// <param name="JWriter"></param>
        /// <param name="ValueObject"></param>
        /// <param name="JSerializer"></param>
        public override void WriteJson(JsonWriter JWriter, object? ValueObject, JsonSerializer JSerializer)
        {
            // Check if object is null. Build output
            if (ValueObject == null) { return; }
            PassThruStructs.PassThruMsg CastMessage = (PassThruStructs.PassThruMsg)ValueObject;

            // Get all the string JSON properties we want to use for this object
            string TimeStampString = CastMessage.Timestamp + "ms";
            string TxFlagsString = CastMessage.TxFlags.ToString();
            string RxStatusString = CastMessage.RxStatus.ToString();
            string ProtocolString = CastMessage.ProtocolId.ToString();
            string DataValueString = CastMessage.DataToHexString(true);
            string ExtraDataIndexString = CastMessage.ExtraDataIndex.ToString();
            string DataSizeString = CastMessage.DataSize + (CastMessage.DataSize == 1 ? " Byte" : " Bytes");

            // Create our dynamic object for JSON output
            var OutputObject = JObject.FromObject(new
            {
                Timestamp = TimeStampString,
                ProtocolId = ProtocolString,
                TxFlags = TxFlagsString,
                RxStatus = RxStatusString,
                DataSize = DataSizeString,
                ExtraDataIndex = ExtraDataIndexString,
                Data = DataValueString
            });

            // Now write this built object.
            JWriter.WriteRawValue(JsonConvert.SerializeObject(OutputObject, Formatting.Indented));
        }
        /// <summary>
        /// Reads in a new JSON object as a PTMessage
        /// </summary>
        /// <param name="JReader"></param>
        /// <param name="ObjectType"></param>
        /// <param name="ExistingValue"></param>
        /// <param name="JSerializer"></param>
        /// <returns></returns>
        public override object? ReadJson(JsonReader JReader, Type ObjectType, object? ExistingValue, JsonSerializer JSerializer)
        {
            // Check if input is null. Build object from it.
            JObject InputObject = JObject.Load(JReader);
            if (InputObject.HasValues == false) { return default; }

            // Enum values pulled in here
            ProtocolId ProtocolRead = InputObject["ProtocolId"].Type == JTokenType.Integer ?
                (ProtocolId)InputObject["ProtocolId"].Value<uint>() :
                (ProtocolId)Enum.Parse(typeof(ProtocolId), InputObject["ProtocolId"].Value<string>());
            RxStatus RxStatusRead = InputObject["RxStatus"].Type == JTokenType.Integer ?
                (RxStatus)InputObject["RxStatus"].Value<uint>() :
                (RxStatus)Enum.Parse(typeof(RxStatus), InputObject["RxStatus"].Value<string>());
            TxFlags TxFlagsRead = InputObject["TxFlags"].Type == JTokenType.Integer ?
                (TxFlags)InputObject["TxFlags"].Value<uint>() :
                (TxFlags)Enum.Parse(typeof(TxFlags), InputObject["TxFlags"].Value<string>());

            // Basic Uint Values
            uint TimeStampRead = uint.Parse(Regex.Match(InputObject["Timestamp"].Value<string>(), "\\d+").Value);
            uint DataSizeRead = uint.Parse(InputObject["DataSize"].Value<string>().Split(' ')[0]);
            uint ExtraDataIndexRead = uint.Parse(InputObject["ExtraDataIndex"].Value<string>());

            // Message Data value
            byte[] MessageDataAsBytes ;
            if (InputObject["Data"].Value<string>() == "No Data!") MessageDataAsBytes = Array.Empty<byte>();
            else MessageDataAsBytes = InputObject["Data"].Value<string>().Split(' ')
                .Select(BytePart => Convert.ToByte(BytePart.Replace("0x", string.Empty), 16))
                .ToArray();

            // Return built output object
            return new PassThruStructs.PassThruMsg()
            {
                Timestamp = TimeStampRead, 
                ProtocolId = ProtocolRead,
                TxFlags = TxFlagsRead,
                RxStatus = RxStatusRead,
                DataSize = DataSizeRead,
                ExtraDataIndex = ExtraDataIndexRead,
                Data = MessageDataAsBytes
            };
        }
    }
}
