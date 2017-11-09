using System;
using System.IO;
using System.Reflection;

using log4net.Layout;

namespace Utils
{
    /// <summary>
    /// A specialized version of the <c>log4net.Appenders.FileAppender</c> class.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The primary difference between this class and its base class is that this one generates a default 
    /// log file name of "Logs\%appname-Log_%datetime.log".
    /// </para>
    /// <para>
    /// The only other difference is that this one will provide a default Header & Footer.  The default header is 
    /// "Starting '[full application path] Version: [application version]".  The default footer is "Closing Application."
    /// </para>
    /// <para>
    /// This class is sealed because the constructor calls a virtual method (set_File).
    /// </para>
     /// </remarks>
    /// <seealso cref="File"/>
    public sealed class FileAppender : log4net.Appender.FileAppender
    {
        /// <summary>
        /// Default constructor that uses the default log file path.
        /// </summary>
        public FileAppender()
        {
            AppendToFile = false;
            string folder = Path.Combine(PathUtilities.ApplicationPath, "Logs");
            Random random = new Random(DateTime.Now.Millisecond);
            bool unique = false;
            while (!unique)
            {
                File = Path.Combine(folder, $"%appname-Log_{DateTime.Now.Ticks}_{random.Next(Int32.MaxValue)}.log");
                if (!System.IO.File.Exists(File))
                    unique = true;
            }
        }

        /// <summary>
        /// This property behaves just like the <c>log4net.Appender.FileAppender.Layout</c> property except that if 
        /// no header or footer is specified, this class will provide one.
        /// </summary>
        public override ILayout Layout
        {
            get { return base.Layout; }
            set
            {
                // If not header is specified, give a default header
                LayoutSkeleton layout = value as LayoutSkeleton;
                if (layout != null)
                {
                    if (layout.Header == null)
                    {
                        Assembly entryAssembly = Assembly.GetEntryAssembly();
                        layout.Header =
                            $"Starting '{entryAssembly.Location}' -- Version: {entryAssembly.GetName().Version}\r\n";
                    }
                    // Make sure there's a newline there.
                    else if (layout.Header.Length > 0)
                        layout.Header += "\r\n";

                    if (layout.Footer == null)
                    {
                        Assembly entryAssembly = Assembly.GetEntryAssembly();
                        layout.Footer =
                            $"Closing '{entryAssembly.Location}' -- Version: {entryAssembly.GetName().Version}\r\n";
                    }
                }

                base.Layout = value;
            }
        }

        /// <summary>
        /// The filename with possible replacement flags.
        /// </summary>
        /// <remarks>
        /// <list type="table">
        ///     <listheader>
        ///         <term>Keyword</term>
        ///         <description>Replacement</description>
        ///     </listheader>
        ///     <item>
        ///         <term>%date</term>
        ///         <description>Replaced with filesystem-safe date (yyyy-MM-dd).</description>
        ///     </item>
        ///     <item>
        ///         <term>%time</term>
        ///         <description>Replaced with filesystem-safe time (hh-mm-ss).</description>
        ///     </item>
        ///     <item>
        ///         <term>%datetime</term>
        ///         <description>Replaced with filesystem-safe date & time (yy-MM-dd--hh-mm-ss).</description>
        ///     </item>
        ///     <item>
        ///         <term>%appname</term>
        ///         <description>Replaced with the name of the application without the extension or parent folder.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public override string File
        {
            get { return base.File; }
            set
            {
                // Parse and replace keywords
                DateTime t = DateTime.Now;
                string tempValue = value;

                // %datetime
                tempValue =
                    tempValue.Replace("%datetime",
                        $"{t.Year:d4}{t.Month:d2}{t.Day:d2}-{t.Hour:d2}{t.Minute:d2}{t.Second:d2}");

                // $date
                tempValue = tempValue.Replace("%date", $"{t.Year:d4}{t.Month:d2}{t.Day:d2}");

                // $time
                tempValue = tempValue.Replace("%time", $"{t.Hour:d2}{t.Minute:d2}{t.Second:d2}");

                // %appname
                tempValue = tempValue.Replace("%appname", PathUtilities.ApplicationName);

                // All identifiers replaced
                base.File = tempValue;
            }
        }
    }
}
