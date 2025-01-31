﻿using System;
using System.Runtime.InteropServices;
using System.Text;
using SharpWrapper.PassThruTypes;

namespace SharpWrapper.PassThruImport
{
    /// <summary>
    /// Delegates used for controlling API Logic.
    /// </summary>
    internal class PassThruDelegates
    {
        // ------------------------------------ API DELEGATES FOR DEVICE SEARCHING --------------------------------

        // DELEGATE: INITNEXTPASSTHRU DEVICE
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegateInitGetNextCarDAQ(IntPtr DeviceName, IntPtr Version, IntPtr IPAddress);
        public DelegateInitGetNextCarDAQ PTInitNextPassThruDevice;

        // DELEGATE: GETNEXTPASSTHRU DEVICE
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegateGetNextCarDAQ([In, Out] ref IntPtr DeviceName, out uint Version, [In, Out] ref IntPtr IPAddress);
        public DelegateGetNextCarDAQ PTGetNextPassThruDevice;

        // DELEGATE: SCAN FOR DEVICES (V0500 ONLY!)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruScanForDevices(out uint DeviceCont);
        public DelegatePassThruScanForDevices PTScanForDevices;

        // DELEGATE: GETS THE NEXT PASSTHRU DEVICE USING IT's DEVICE STRUCT
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruGetNextDevice(out PassThruStructsNative.SDEVICE InputSDevice);
        public DelegatePassThruGetNextDevice PTGetNextDevice;

        // ------------------------------------- API DELEGATES FOR PASSTHRU FUNCTIONS -------------------------------

        // DELEGATE: PASSTHRU OPEN
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruOpen(IntPtr DllPointer, out uint DeviceId);
        public DelegatePassThruOpen PTOpen;

        // DELEGATE: PASSTHRU CLOSE
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruClose(uint DeviceId);
        public DelegatePassThruClose PTClose;

        // DELEGATE: PASSTHRU CONNECT
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruConnect(uint DeviceId, uint ProtocolId, uint MsgFlags, uint BaudRate, out uint ChannelId);
        public DelegatePassThruConnect PTConnect;

        // DELEGATE: PASSTHRU DISCONNECT
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruDisconnect(uint ChannelId);
        public DelegatePassThruDisconnect PTDisconnect;

        // DELEGATE: PASSTHRU READ MESSAGE
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruReadMsgs(uint ChannelId, [In, Out] PassThruStructsNative.PASSTHRU_MSG[] PassThruMsg, out uint MsgCount, uint MsgTimeout);
        public DelegatePassThruReadMsgs PTReadMsgs;

        // DELEGATE: PASSTHRU WRITE MESSAGE
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruWriteMsgs(uint ChannelId, [In] PassThruStructsNative.PASSTHRU_MSG[] PassThruMsg, ref uint MsgCount, uint MsgTimeout);
        public DelegatePassThruWriteMsgs PTWriteMsgs;

        // DELEGATE: PASSTHRU START PERIODIC
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruStartPeriodicMsg(uint ChannelId, [In] ref PassThruStructsNative.PASSTHRU_MSG PassThruMsg, out uint PassThruMsgId, uint TimeInterval);
        public DelegatePassThruStartPeriodicMsg PTStartPeriodicMsg;

        // DELEGATE: PASSTHRU STOP PERIODIC
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruStopPeriodicMsg(uint ChannelId, uint MsgId);
        public DelegatePassThruStopPeriodicMsg PTStopPeriodicMsg;

        // DELEGATE: PASSTHRU START FILTER (With message flow Ctl)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruStartMsgFilter(uint ChannelId, uint FilterType, [In] ref PassThruStructsNative.PASSTHRU_MSG PassThruMask, [In] ref PassThruStructsNative.PASSTHRU_MSG PassThruPattern, [In] ref PassThruStructsNative.PASSTHRU_MSG PassThruFlowCtl, out uint FilterId);
        public DelegatePassThruStartMsgFilter PTStartMsgFilter;

        // DELEGATE: PASSTHRU START FILTER
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruStartMsgFilterFlowPtr(uint ChannelId, uint FilterType, [In] ref PassThruStructsNative.PASSTHRU_MSG PassThruMask, [In] ref PassThruStructsNative.PASSTHRU_MSG PassThruPattern, [In] IntPtr PassThruFlowCtl, out uint FilterId);
        public DelegatePassThruStartMsgFilterFlowPtr PTStartMsgFilterFlowPtr;

        // DELEGATE: PASSTHRU STOP FILTER
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruStopMsgFilter(uint ChannelId, uint FilterId);
        public DelegatePassThruStopMsgFilter PTStopMsgFilter;

        // DELEGATE: PASSTHRU SET PROGRAMMING VOLTAGE
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruSetProgrammingVoltage(uint DeviceId, uint DevicePin, uint SetVoltage);
        public DelegatePassThruSetProgrammingVoltage PTSetProgrammingVoltage;

        // DELEGATE: PASSTHRU READ VERSIONS
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruReadVersion(uint DeviceId, StringBuilder FirmwareVersion, StringBuilder DllVersion, StringBuilder ApiVersion);
        public DelegatePassThruReadVersion PTReadVersion;

        // DELEGATE: PASSTHRU GET LAST ERROR
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruGetLastError(StringBuilder ErrorDescription);
        public DelegatePassThruGetLastError PTGetLastError;

        // DELEGATE: PASSTHRU IOCTL
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruIoctl(uint ChannelId, uint IoctlId, IntPtr InputPtr, IntPtr OutputPtr);
        public DelegatePassThruIoctl PTIoctl;

        // ------------------------------------- API DELEGATES FOR VERSION 0500 PASSTHRU FUNCTIONS ONLY! -------------------------------

        // DELEGATE PASSTHRU LOGICAL CONNECT
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruLogicalConnect(uint PhysicalChannelId, uint ProtocolId, uint Flags, IntPtr ChannelDescriptor, out uint ChannelId);
        public DelegatePassThruLogicalConnect PTLogicalConnect;

        // DELEGATE: PASSTHRU LOGCIAL DISCONNECT
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruLogicalDisconnect(uint PhysicalChannelId);
        public DelegatePassThruLogicalDisconnect PTLogicalDisconnect;

        // DELEGATE: PASSTHRU SELECT MESSAGE
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruSelect(IntPtr ChannelSetPointer, uint SelectType, uint Timeout);
        public DelegatePassThruSelect PTSelect;

        // DELEGATE: PASSTHRU QUEUE MESSAGE
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePassThruQueueMsgs(uint ChannelId, [In] PassThruStructsNative.PASSTHRU_MSG[] MessagePointer, ref uint MessageCount);
        public DelegatePassThruQueueMsgs PTQueueMsgs;

        // -------------------------------------------- API DELEGATES FOR FULCRUM SHIM LOGGING FUNCTIONS -------------------------------------

        // PT Write Log Append Method object
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePTWriteLogA([MarshalAs(UnmanagedType.LPStr)] string MessageValue);
        public DelegatePTWriteLogA PTWriteLogA;

        // PT Write Log Write Method object
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePTWriteLogW([MarshalAs(UnmanagedType.LPWStr)] string MessageValue);
        public DelegatePTWriteLogW PTWriteLogW;

        // PT Save Log Method object
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DelegatePTSaveLog([MarshalAs(UnmanagedType.LPWStr)] string LogFilePath);
        public DelegatePTSaveLog PTSaveLog;
    }
}
