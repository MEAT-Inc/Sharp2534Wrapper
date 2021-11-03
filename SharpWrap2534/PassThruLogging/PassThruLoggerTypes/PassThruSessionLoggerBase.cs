﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using SharpWrap2534.PassThruLogging.SessionSetup;

namespace SharpWrap2534.PassThruLogging.PassThruLoggerTypes
{
    /// <summary>
    /// Logging instance class for SimSession logic.
    /// </summary>
    internal class SimSessionLoggerBase
    {
        // Host broker object which provides needed info to these loggers.
        public static SimLoggingBroker BrokerHost;

        // NLog objects
        protected Logger NLogger;                         // NLog object
        internal LoggingConfiguration LoggingConfig;      // Logging config item

        // Log Level Info (0 is Trace, 6 Is Off)
        internal bool DisableLogging = false;             // Toggle to turn logging on or off.
        internal LogLevel MinLevel = LogLevel.Trace;      // Lowest level
        internal LogLevel MaxLevel = LogLevel.Fatal;      // Top most level

        // Time, name, and other info for logger.
        public string LoggerName;           // Name of the logger
        public Guid LoggerGuid;             // Name of the logger
        public DateTime TimeStarted;        // Time the logger was launched.
        public LoggerActions LoggerType;    // Type of logger


        // -----------------------------------------------------------------------------------------------------

        /// <summary>
        /// Builds a new FalconLogger object and adds it to the logger pool.
        /// </summary>
        /// <param name="LoggerName">Name of this logger</param>
        /// <param name="LoggerType">Type of actions</param>
        /// <param name="MinLevel">Min Log Level</param>
        /// <param name="MaxLevel">Max Log Level</param>
        protected SimSessionLoggerBase(LoggerActions LoggerType, [CallerMemberName] string LoggerName = "", int MinLevel = 0, int MaxLevel = 5)
        {
            // Set Min and Max
            this.MinLevel = LogLevel.FromOrdinal(MinLevel);
            this.MaxLevel = LogLevel.FromOrdinal(MaxLevel);

            // Store values and configure now.
            this.LoggerType = LoggerType;
            this.TimeStarted = DateTime.Now;
            this.LoggerGuid = Guid.NewGuid();
            this.LoggerName = LoggerName + "_" + LoggerType.ToString();

            // Add self to queue.
            SimLoggingBroker.LoggerQueue.AddLoggerToPool(this);
        }
        /// <summary>
        /// BUilds a new usable instance of a simsession base logging object.
        /// </summary>
        /// <param name="LoggerName"></param>
        /// <param name="LogFileName"></param>
        /// <param name="MinLevel"></param>
        /// <param name="MaxLevel"></param>
        internal SimSessionLoggerBase([CallerMemberName] string LoggerName = "", string LogFileName = "", int MinLevel = 0, int MaxLevel = 5) : 
            this (LoggerActions.BaseSimLogger, LoggerName, MinLevel, MaxLevel)
        {
            // Check file name.
            if (string.IsNullOrEmpty(LogFileName))
            {
                // Check for broker file.
                LogFileName = SimLoggingBroker.MainLogFileName != null
                    ? SimLoggingBroker.MainLogFileName
                    : Path.Combine(
                        SimLoggingBroker.BaseOutputPath,
                        $"{this.LoggerName}_Logging_{DateTime.Now.ToString("ddMMyyy-hhmmss")}.log"
                    );
            }

            // Build Master Logging Configuration.
            this.LoggingConfig = LogManager.Configuration;
            this.LoggingConfig.AddRule(
                LogLevel.FromOrdinal(MinLevel),
                LogLevel.FromOrdinal(MaxLevel),
                SimLoggerFactory.GenerateFileLogger(LoggerName, LogFileName), $"*{LoggerName}*");
            this.LoggingConfig.AddRule(
                LogLevel.FromOrdinal(MinLevel),
                LogLevel.FromOrdinal(MaxLevel),
                SimLoggerFactory.GenerateConsoleLogger(LoggerName), $"*{LoggerName}*");

            // Store config for the NLog object now.
            LogManager.Configuration = this.LoggingConfig;
            this.NLogger = LogManager.GetCurrentClassLogger();
            this.PrintLoggerInfos();
        }

        // ------------------------------------------ LOG WRITING ACTIONS --------------------------------------------

        /// <summary>
        /// Methods for writing log values
        /// </summary>
        /// <param name="LogMessage">Message to write</param>
        /// <param name="Level">Level to log</param>
        public virtual void WriteLog(string LogMessage, LogType Level = LogType.DebugLog)
        {
            // Set Context here and store
            string ClassName = this.GetCallingClass();
            MappedDiagnosticsContext.Set("custom-name", this.LoggerName);
            MappedDiagnosticsContext.Set("calling-class", ClassName);
            MappedDiagnosticsContext.Set("calling-class-short",
                ClassName.Contains('.') ? ClassName.Split('.').Last() : ClassName);

            // Write vaule
            this.NLogger.Log(Level.ToNLevel(), LogMessage);
        }
        /// <summary>
        /// Writes an exceptions contents out to the logger
        /// </summary>
        /// <param name="Ex">Ex to write</param>
        /// <param name="Level">Level to log it</param>
        public virtual void WriteLog(Exception Ex, LogType Level = LogType.ErrorLog)
        {
            // Set Context here and store
            string ClassName = this.GetCallingClass();
            MappedDiagnosticsContext.Set("custom-name", this.LoggerName);
            MappedDiagnosticsContext.Set("calling-class", ClassName);
            MappedDiagnosticsContext.Set("calling-class-short",
                ClassName.Contains('.') ? ClassName.Split('.').Last() : ClassName);

            // Write Exception values
            this.NLogger.Log(Level.ToNLevel(), $"Ex Message {Ex.Message}");
            this.NLogger.Log(Level.ToNLevel(), $"Ex Source  {Ex.Source}");
            this.NLogger.Log(Level.ToNLevel(), $"Ex Target  {Ex.TargetSite.Name}");

            // Write the Ex Stack trace
            if (Ex.StackTrace == null) { return; }
            this.NLogger.Log(Level.ToNLevel(), $"Ex Stack\n{Ex.StackTrace}");
        }
        /// <summary>
        /// Writes an exception object out.
        /// </summary>
        /// <param name="MessageExInfo">Info message</param>
        /// <param name="Ex">Ex to write</param>
        /// <param name="LevelTypes">Levels. Msg and then Ex</param>
        public virtual void WriteLog(string MessageExInfo, Exception Ex, LogType[] LevelTypes = null)
        {
            // Check level count.
            if (LevelTypes == null) { LevelTypes = new LogType[2] { LogType.ErrorLog, LogType.ErrorLog }; }
            if (LevelTypes.Length == 1) { LevelTypes = LevelTypes.Append(LevelTypes[0]).ToArray(); }

            // Store Calling Class
            // Set Context here and store
            string ClassName = this.GetCallingClass();
            MappedDiagnosticsContext.Set("custom-name", this.LoggerName);
            MappedDiagnosticsContext.Set("calling-class", ClassName);
            MappedDiagnosticsContext.Set("calling-class-short",
                ClassName.Contains('.') ? ClassName.Split('.').Last() : ClassName);

            // Write Log Message then exception
            this.NLogger.Log(LevelTypes[0].ToNLevel(), MessageExInfo);
            this.NLogger.Log(LevelTypes[0].ToNLevel(), $"EXCEPTION THROWN FROM {Ex.TargetSite}. DETAILS ARE SHOWN BELOW");

            // Write Exception
            this.NLogger.Log(LevelTypes[1].ToNLevel(), $"\tEX MESSAGE {Ex.Message}");
            this.NLogger.Log(LevelTypes[1].ToNLevel(), $"\tEX SOURCE  {Ex.Source}");
            this.NLogger.Log(LevelTypes[1].ToNLevel(), $"\tEX TARGET  {Ex.TargetSite.Name}");

            // Write the Ex Stack trace
            if (Ex.StackTrace == null) { this.NLogger.Log(LevelTypes[1].ToNLevel(), "FURTHER DIAGNOSTIC INFO IS NOT AVALIABLE AT THIS TIME."); }
            this.NLogger.Log(LevelTypes[1].ToNLevel(), $"\tEX STACK\n{Ex.StackTrace.Replace("\n", "\n\t")}");
        }

        // -------------------------------------- BASE CONFIG METHODS AND INFO ---------------------------------------

        /// <summary>
        /// Writes all the logger info out to the current file.
        /// </summary>
        public virtual void PrintLoggerInfos(LogType LogType = LogType.DebugLog)
        {
            // Store a log type first.
            MappedDiagnosticsContext.Set("custom-name", this.LoggerName);
            MappedDiagnosticsContext.Set("calling-class", "StartupLogger (" + this.LoggerName + ")");
            MappedDiagnosticsContext.Set("calling-class-short", "StartupLogger");
            this.NLogger.Log(LogType.ToNLevel(), $"LOGGER NAME: {this.LoggerName}");
            this.NLogger.Log(LogType.ToNLevel(), $"--> TIME STARTED:  {this.TimeStarted}");
            this.NLogger.Log(LogType.ToNLevel(), $"--> LOGGER GUID:   {this.LoggerGuid.ToString("D").ToUpper()}");
        }
        /// <summary>
        /// Gets the name of the calling method.
        /// </summary>
        /// <returns>String of the full method name.</returns>
        public string GetCallingClass(bool SplitString = false)
        {
            // Setup values.
            string FullCallName; Type DeclaredType; int SkipFrames = 2;
            do
            {
                // Find the current method caller and store the stack. 
                MethodBase MethodBase = new StackFrame(SkipFrames, false).GetMethod();
                DeclaredType = MethodBase.DeclaringType;
                if (DeclaredType == null) { return MethodBase.Name; }

                // Skip frame increased and keep checking.
                SkipFrames++;
                FullCallName = DeclaredType.FullName + "." + MethodBase.Name;
            }
            while (DeclaredType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            // Check for split values.
            if (!SplitString) { return FullCallName; }
            var FullNameSplit = FullCallName.Split('.');
            FullCallName = FullNameSplit[FullNameSplit.Length - 1];

            // Return the name here.
            return FullCallName;
        }
    }
}
