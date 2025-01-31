﻿using System;
using System.Collections.Generic;
using System.Linq;

// Static using for Regex lookups and type values
using PassThruRegex = SharpExpressions.PassThruExpressionRegex;

namespace SharpExpressions.PassThruExpressions
{
    /// <summary>
    /// Class object used to find a PTDisconnect command instance from an input line set.
    /// </summary>
    public class PassThruDisconnectExpression : PassThruExpression
    {
        // Regex for the disconnect channel command (PTDisconnect)
        public readonly PassThruRegex PTDisconnectRegex = PassThruRegex.LoadedExpressions[PassThruExpressionTypes.PTDisconnect];

        // Strings of the command and results from the command output.
        [PassThruProperty("Command Line")] public readonly string PtCommand;
        [PassThruProperty("Channel ID", "-1", new[] { "Channel Closed", "Invalid Channel!" }, true)]
        public readonly string ChannelId;

        // -------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Builds a new instance of a PTDisconnect Regex Helper 
        /// </summary>
        /// <param name="CommandInput">Lines to filter out of.</param>
        public PassThruDisconnectExpression(string CommandInput) : base(CommandInput, PassThruExpressionTypes.PTDisconnect)
        {
            // Find command issue request values
            var FieldsToSet = this.GetExpressionProperties();
            bool PtDisconnectResult = this.PTDisconnectRegex.Evaluate(CommandInput, out var PassThruDisconnectStrings);
            if (!PtDisconnectResult) this._expressionLogger.WriteLog($"FAILED TO REGEX OPERATE ON ONE OR MORE TYPES FOR EXPRESSION TYPE {this.GetType().Name}!");

            // Find our values to store here and add them to our list of values.
            List<string> StringsToApply = new List<string> { PassThruDisconnectStrings[0] };
            StringsToApply.AddRange(this.PTDisconnectRegex.ExpressionValueGroups
                .Where(NextIndex => NextIndex <= PassThruDisconnectStrings.Length)
                .Select(NextIndex => PassThruDisconnectStrings[NextIndex]));

            // Now apply values using base method and exit out of this routine
            if (!this.SetExpressionProperties(FieldsToSet, StringsToApply.ToArray()))
                throw new InvalidOperationException($"FAILED TO SET CLASS VALUES FOR EXPRESSION OBJECT OF TYPE {this.GetType().Name}!");
        }
    }
}
