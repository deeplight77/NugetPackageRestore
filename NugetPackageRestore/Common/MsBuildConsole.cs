using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuGet.Common;
using NuGet;
using Microsoft.Build.Utilities;
using System.Security;

namespace NugetPackageRestore
{
    public class MsBuildConsole : IConsole
    {
        private TaskLoggingHelper _logger;

        public MsBuildConsole(TaskLoggingHelper Logger)
        {
            _logger = Logger;
        }

        public int CursorLeft { get; set; }
        public int WindowWidth { get; set; }
        public Verbosity Verbosity { get; set; }
        public bool IsNonInteractive { get; set; }

        public void Write(object value)
        {
            _logger.LogMessage(value.ToString());   
        }

        public void Write(string value)
        {
            _logger.LogMessage(value); 
        }

        public void Write(string format, params object[] args)
        {
            if (args == null || !args.Any())
            {
                // Don't try to format strings that do not have arguments. We end up throwing if the original string was not meant to be a format token 
                // and contained braces (for instance html)
                _logger.LogMessage(format); 
            }
            else
            {
                _logger.LogMessage(_logger.FormatString(format, args)); 
            }
        }

        public void WriteLine()
        {
            _logger.LogMessage("");
        }
        public void WriteLine(object value)
        {
            _logger.LogMessage(value.ToString());
        }
        public void WriteLine(string value)
        {
            _logger.LogMessage(value); 
        }
        public void WriteLine(string format, params object[] args)
        {
            if (args == null || !args.Any())
            {
                // Don't try to format strings that do not have arguments. We end up throwing if the original string was not meant to be a format token 
                // and contained braces (for instance html)
                _logger.LogMessage(format);
            }
            else
            {
                _logger.LogMessage(_logger.FormatString(format, args));
            }
        }
        public void WriteLine(ConsoleColor color, string value, params object[] args)
        {
            if (args == null || !args.Any())
            {
                // Don't try to format strings that do not have arguments. We end up throwing if the original string was not meant to be a format token 
                // and contained braces (for instance html)
                _logger.LogMessage(value);
            }
            else
            {
                _logger.LogMessage(_logger.FormatString(value, args));
            }
        }

        public void WriteError(object value)
        {
            _logger.LogMessage(value.ToString()); 
        }
        public void WriteError(string value)
        {
            _logger.LogMessage(value); 
        }
        public void WriteError(string format, params object[] args)
        {
            if (args == null || !args.Any())
            {
                // Don't try to format strings that do not have arguments. We end up throwing if the original string was not meant to be a format token 
                // and contained braces (for instance html)
                _logger.LogMessage(format);
            }
            else
            {
                _logger.LogMessage(_logger.FormatString(format, args));
            }
        }

        public void WriteWarning(string value)
        {
            _logger.LogMessage(value); 
        }

        public void WriteWarning(bool prependWarningText, string value)
        {
            string message = prependWarningText
                                 ? String.Format("Warning: {0}", value)
                                 : value;

            _logger.LogMessage(message); 
        }

        public void WriteWarning(string value, params object[] args)
        {
            if (args == null || !args.Any())
            {
                // Don't try to format strings that do not have arguments. We end up throwing if the original string was not meant to be a format token 
                // and contained braces (for instance html)
                _logger.LogMessage(value);
            }
            else
            {
                _logger.LogMessage(_logger.FormatString(value, args));
            }
        }

        public void WriteWarning(bool prependWarningText, string value, params object[] args)
        {
            string message = prependWarningText
                                 ? String.Format("Warning: {0}", value)
                                 : value;

            if (args == null || !args.Any())
            {
                // Don't try to format strings that do not have arguments. We end up throwing if the original string was not meant to be a format token 
                // and contained braces (for instance html)
                _logger.LogMessage(message);
            }
            else
            {
                _logger.LogMessage(_logger.FormatString(message, args));
            }
        }

        public bool Confirm(string description) 
        { 
            return true; 
        }
        public ConsoleKeyInfo ReadKey() 
        { 
            return new ConsoleKeyInfo(); 
        }
        public string ReadLine() 
        {
            return "";
        }
        public void ReadSecureString(SecureString secureString) { }

        public void PrintJustified(int startIndex, string text) 
        {
            _logger.LogMessage(text); 
        }
        public void PrintJustified(int startIndex, string text, int maxWidth) 
        {
            _logger.LogMessage(text); 
        }

        public void Log(MessageLevel level, string message, params object[] args)
        {
            switch (level)
            {
                case MessageLevel.Info:
                    WriteLine(message, args);
                    break;
                case MessageLevel.Warning:
                    WriteWarning(message, args);
                    break;
                case MessageLevel.Debug:
                    WriteLine(message, args);
                    break;
            }
        }

        public FileConflictResolution ResolveFileConflict(string message)
        {
            return FileConflictResolution.OverwriteAll;
        }
    }
}
