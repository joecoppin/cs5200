﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WordGuessMonitor {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class MonitorSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static MonitorSettings defaultInstance = ((MonitorSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new MonitorSettings())));
        
        public static MonitorSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5000")]
        public int RefreshFrequency {
            get {
                return ((int)(this["RefreshFrequency"]));
            }
            set {
                this["RefreshFrequency"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("500")]
        public int Timeout {
            get {
                return ((int)(this["Timeout"]));
            }
            set {
                this["Timeout"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("127.0.0.1:12001,34.234.33.221:12001")]
        public string ServerList {
            get {
                return ((string)(this["ServerList"]));
            }
            set {
                this["ServerList"] = value;
            }
        }
    }
}