﻿//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================


using System;
using System.Diagnostics;
using System.Globalization;
using System.Security;
using System.Threading;

namespace Infrastructure.Crosscutting.Logging.TraceSource
{
    /// <summary>
    /// Implementation of contract <see cref="Infrastructure.Crosscutting.Logging.ILogger"/>
    /// using System.Diagnostics API.
    /// </summary>
    public sealed class TraceSourceProvider
        :ILogger
    {
        #region Members

        public static System.Diagnostics.TraceSource Source = new System.Diagnostics.TraceSource("TraceSourceApp", SourceLevels.All); 

        #endregion

        #region  Constructor

        /// <summary>
        /// Create a new instance of this trace manager
        /// </summary>
        public TraceSourceProvider(TraceListener listener)
        {
            // Create default source
           // source = new System.Diagnostics.TraceSource("TraceSourceApp", SourceLevels.All); 
            if (listener != null)
                Source.Listeners.Add(listener);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Trace internal message in configured listeners
        /// </summary>
        /// <param name="eventType">Event type to trace</param>
        /// <param name="message">Message of event</param>
        void TraceInternal(TraceEventType eventType, string message)
        {

            if (Source != null)
            {
                try
                {
                    Source.TraceEvent(eventType, (int)eventType, string.Format("{0}||thread:{1}| -【{2}】", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff "), Thread.CurrentThread.ManagedThreadId.ToString(), message));
                }
                catch (SecurityException)
                {
                    //Cannot access to file listener or cannot have
                    //privileges to write in event log etc...
                }
            }
        }
        #endregion

        #region ILogger Members

        /// <summary>
        /// <see cref="Infrastructure.Crosscutting.Logging.ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="Infrastructure.Crosscutting.Logging.ILogger"/></param>
        /// <param name="args"><see cref="Infrastructure.Crosscutting.Logging.ILogger"/></param>
        public void LogInfo(string message, params object[] args)
        {
            if (!String.IsNullOrEmpty(message)
                &&
                !String.IsNullOrEmpty(message))
            {
                var traceData = string.Format(CultureInfo.InvariantCulture, message, args);

                TraceInternal(TraceEventType.Information, traceData);
            }
        }
        /// <summary>
        /// <see cref="Infrastructure.Crosscutting.Logging.ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="Infrastructure.Crosscutting.Logging.ILogger"/></param>
        /// <param name="args"><see cref="Infrastructure.Crosscutting.Logging.ILogger"/></param>
        public void LogWarning(string message, params object[] args)
        {
            
            if (!String.IsNullOrEmpty(message)
                &&
                !String.IsNullOrEmpty(message))
            {
                var traceData = string.Format(CultureInfo.InvariantCulture, message,args);

                TraceInternal(TraceEventType.Warning, traceData);
            }
        }

        /// <summary>
        /// <see cref="Infrastructure.Crosscutting.Logging.ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="Infrastructure.Crosscutting.Logging.ILogger"/></param>
        /// <param name="args"><see cref="Infrastructure.Crosscutting.Logging.ILogger"/></param>
        public void LogError(string message, params object[] args)
        {
            if (!String.IsNullOrEmpty(message)
                &&
                !String.IsNullOrEmpty(message))
            {
                var traceData = string.Format(CultureInfo.InvariantCulture, message, args);

                TraceInternal(TraceEventType.Error, traceData);
            }
        }

        /// <summary>
        /// <see cref="Infrastructure.Crosscutting.Logging.ILogger"/>
        /// </summary>
        /// <param name="message"><see cref="Infrastructure.Crosscutting.Logging.ILogger"/></param>
        /// <param name="exception"><see cref="Infrastructure.Crosscutting.Logging.ILogger"/></param>
        /// <param name="args"><see cref="Infrastructure.Crosscutting.Logging.ILogger"/></param>
        public void LogError(string message, Exception exception, params object[] args)
        {
            if (!String.IsNullOrEmpty(message)
                &&
                !String.IsNullOrEmpty(message))
            {
                var traceData = string.Format(CultureInfo.InvariantCulture, message, args);

                var exceptionData = exception.ToString(); // The ToString() create a string representation of the current exception

                TraceInternal(TraceEventType.Error, string.Format(CultureInfo.InvariantCulture,"错误信息：{0} Exception:{1},行数：{2}",traceData,exceptionData,exception.StackTrace));
            }
        }

        #endregion
    }
}
