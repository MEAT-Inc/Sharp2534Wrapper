﻿using System;
using JBoxInvoker.PassThruLogic.SupportingLogic;

namespace JBoxInvoker.PassThruLogic.J2534Objects
{
    /// <summary>
    /// J2534 filter object for PassThru channels.
    /// </summary>
    public class J2534Filter : IComparable
    {
        // Filter Type info
        public string FilterType;
        public uint FilterFlags;
        public PTInstanceStatus FilterStatus;

        // Filter values.
        public string FilterMask;
        public string FilterPattern;
        public string FilterFlowCtl;
        public uint FilterId;

        // ---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Empty CTOR For filter type output.
        /// </summary>
        public J2534Filter() { this.FilterStatus = PTInstanceStatus.NULL; }
            
        /// <summary>
        /// Builds a new filter using a mask, pattern, and the Id of it.
        /// </summary>
        /// <param name="FilterMask"></param>
        /// <param name="FilterPattern"></param>
        /// <param name="FilterId"></param>
        public J2534Filter(string FilterMask, string FilterPattern, uint FilterId)
        {
            // Store filter values
            this.FilterMask = FilterMask;
            this.FilterPattern = FilterPattern;
            this.FilterId = FilterId;

            // Set status.
            FilterStatus = PTInstanceStatus.INITIALIZED;
        }
        /// <summary>
        /// Builds a new flow control filter using the passed string values of it.
        /// </summary>
        /// <param name="FilterType"></param>
        /// <param name="FilterMask"></param>
        /// <param name="FilterPattern"></param>
        /// <param name="FilterFlowCtl"></param>
        /// <param name="FilterFlags"></param>
        /// <param name="FilterId"></param>
        public J2534Filter(string FilterType, string FilterMask, string FilterPattern, string FilterFlowCtl, uint FilterFlags, uint FilterId)
        {
            // Store filter values.
            this.FilterType = FilterType;
            this.FilterMask = FilterMask;
            this.FilterPattern = FilterPattern;
            this.FilterFlowCtl = FilterFlowCtl;
            this.FilterId = FilterId;
            this.FilterFlags = FilterFlags;

            // Set status.
            FilterStatus = PTInstanceStatus.INITIALIZED;
        }
        /// <summary>
        /// Builds a new filter using a specified type.
        /// </summary>
        /// <param name="FilterType"></param>
        /// <param name="FilterMask"></param>
        /// <param name="FilterPattern"></param>
        /// <param name="FilterFlags"></param>
        /// <param name="FilterId"></param>
        public J2534Filter(string FilterType, string FilterMask, string FilterPattern, uint FilterFlags, uint FilterId)
        {
            // Set filter values
            this.FilterType = FilterType;
            this.FilterFlags = FilterFlags;
            this.FilterMask = FilterMask;
            this.FilterPattern = FilterPattern;
            this.FilterId = FilterId;

            // Set status.
            FilterStatus = PTInstanceStatus.INITIALIZED;
        }
        /// <summary>
        /// Builds a new Filter using a mask and pattern
        /// </summary>
        /// <param name="FilterMask"></param>
        /// <param name="FilterPattern"></param>
        public J2534Filter(string FilterMask, string FilterPattern)
        {
            // Set filter values.
            this.FilterMask = FilterMask;
            this.FilterPattern = FilterPattern;
            FilterId = 0;

            // Set status.
            FilterStatus = PTInstanceStatus.INITIALIZED;
        }

        // ----------------------------------- OVERRIDES FOR STRING AND COMPARISON ----------------------------------------

        /// <summary>
        /// Builds a string of the filter info.
        /// </summary>
        /// <returns>String built version of the filter.</returns>
        public override string ToString()
        {
            // Build string to convert with.
            string OutputString = $"Type: {FilterType} -- Flags: 0x{FilterFlags.ToString("X8").ToUpper()}";
            OutputString += $"-- MessageData: {(FilterMask ?? "NO_MASK")},{(FilterPattern ?? "NO_PATTERN")},{(FilterFlowCtl ?? "NO_FLOW")}";

            // Return string built.
            return OutputString;
        }
        /// <summary>
        /// Builds output string containing just the message values.
        /// </summary>
        /// <returns>String of message data.</returns>
        public string ToMessageDataString()
        {
            // Build and return string.
            return $"MessageData: {(FilterMask ?? "NO_MASK")},{(FilterPattern ?? "NO_PATTERN")},{(FilterFlowCtl ?? "NO_FLOW")}";
        }

        /// <summary>
        /// Compares two filter values.
        /// </summary>
        /// <param name="FilterObj">Filter to compare.</param>
        /// <returns></returns>
        public int CompareTo(object FilterObj)
        {
            // Make sure the type is correctly setup
            if (FilterObj.GetType() != typeof(J2534Filter)) 
                throw new InvalidCastException($"Can not convert a type of {FilterObj.GetType().Name} to a J2534 Filter!");

            // Compare here.
            J2534Filter CastFilter = (J2534Filter)FilterObj;
            return string.Compare(ToString(), CastFilter.ToString());
        }
    }
}
