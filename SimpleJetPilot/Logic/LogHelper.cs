using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SimpleJetPilot.Logic
{
    public static class LogHelper
    {
        public static void Log(EntryType type, string message)
        {
            Log(type, message, null);
        }

        public static void Error(string message)
        {
            Error(message, null);
        }
        public static void Error(string messageFormat, params object[] args)
        {
            Log(EntryType.Error, messageFormat, args);
        }

        public static void Warning(string message)
        {
            Warning(message, null);
        }
        public static void Warning(string messageFormat, params object[] args)
        {
            Log(EntryType.Warning, messageFormat, args);
        }

        public static void Info(string message)
        {
            Info(message, null);
        }
        public static void Info(string messageFormat, params object[] args)
        {
            Log(EntryType.Info, messageFormat, args);
        }

        public static void Log(EntryType type, string messageFormat, params object[] args)
        {
            try
            {
                var message = args != null && args.Length > 0 ? String.Format(messageFormat, args) : messageFormat;

                switch (type)
                {
                    case EntryType.Info: { Debug.LogFormat("[SimpleJetPilot]: {0}", message); break; }
                    case EntryType.Warning: { Debug.LogWarningFormat("[SimpleJetPilot]: {0}", message); break; }
                    case EntryType.Error: { Debug.LogErrorFormat("[SimpleJetPilot]: {0}", message); break; }
                }
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("Unable to format message. Message format was: '{0}'", messageFormat);
                Debug.LogException(ex);
            }
        }
    }

    public enum EntryType
    {
        Info,
        Warning,
        Error,
    }
}
