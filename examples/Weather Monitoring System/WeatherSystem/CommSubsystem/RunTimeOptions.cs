#region License
//
// Command Line Library: Program.cs
//
// Author:
//   Giacomo Stelluti Scala (gsscoder@gmail.com)
//
// Copyright (C) 2005 - 2013 Giacomo Stelluti Scala
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
#endregion
#region Using Directives
using CommandLine;
using CommandLine.Text;
#endregion

namespace CommSubsystem
{
    /// <summary>
    /// Run-time options class
    /// 
    /// Instances of this class hold runtime options parsed from command-line arguments and defaulted by properties.
    /// This class uses the Option attributed in CommandLine library.
    /// </summary>
    public abstract class RuntimeOptions
    {
        [Option("pid", MetaValue = "INT", Required = true, HelpText = "Process Id")]
        public short ProcessId { get; set; }
        
        [Option("gma", MetaValue = "STRING", Required = false, HelpText = "Group Multicast Address that this proecss should join")]
        public string GroupMulticastAddress { get; set; }

        [Option("label", MetaValue = "STRING", Required = false, HelpText = "Process label")]
        public string Label { get; set; }

        [Option("minport", MetaValue = "INT", Required = false, HelpText = "Min port in a range of possible ports for this process's communications")]
        public int? MinPortNullable { get; set; }
        public int MinPort => MinPortNullable ?? 0;

        [Option("maxport", MetaValue = "INT", Required = false, HelpText = "Max port in a range of possible ports for this process's communications")]
        public int? MaxPortNullable { get; set; }
        public int MaxPort => MaxPortNullable ?? 0;

        [Option("timeout", MetaValue = "INT", Required = false, HelpText = "Default timeout for request-reply communications")]
        public int? TimeoutNullable { get; set; }
        public int Timeout => TimeoutNullable ?? 0;

        [Option("retries", MetaValue = "INT", Required = false, HelpText = "Default max retries for request-reply communications")]
        public int? MaxRetriesNullable { get; set; }
        public int MaxRetries => MaxRetriesNullable ?? 0;

        [Option('s', "autostart", Required = false, HelpText = "Autostart")]
        public bool AutoStart { get; set; }

        [Option("dsEndPoint", MetaValue = "STRING", Required = false, HelpText = "Group Multicast Address for all Data Servers")]
        public string DsGroupMultiCastAddress { get; set; }

        [Option("dMinutes", MetaValue = "INT", Required = false, HelpText = "Minutes between data server discovery")]
        public int? DsDiscoveryIntervalNullable { get; set; }
        public int DsDiscoveryInterval => DsDiscoveryIntervalNullable ?? 0;


        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        public abstract void SetDefaults();
    }
}
