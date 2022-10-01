namespace WebApp.Models.resource {
  using System;
  [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  public class WaterMeterRecord {
        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal WaterMeterRecord() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebApp.Models.resource.WaterMeterRecord", typeof(WaterMeterRecord).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
    public static string Id {
            get {
                return ResourceManager.GetString("Id", resourceCulture);
            }
    }
    public static string meterStatus {
            get {
                return ResourceManager.GetString("meterStatus", resourceCulture);
            }
    }
    public static string datetime {
            get {
                return ResourceManager.GetString("datetime", resourceCulture);
            }
    }
    public static string meterId {
            get {
                return ResourceManager.GetString("meterId", resourceCulture);
            }
    }
    public static string water {
            get {
                return ResourceManager.GetString("water", resourceCulture);
            }
    }
    public static string reverseWater {
            get {
                return ResourceManager.GetString("reverseWater", resourceCulture);
            }
    }
    public static string temperature {
            get {
                return ResourceManager.GetString("temperature", resourceCulture);
            }
    }
    public static string flowrate {
            get {
                return ResourceManager.GetString("flowrate", resourceCulture);
            }
    }
    public static string pressure {
            get {
                return ResourceManager.GetString("pressure", resourceCulture);
            }
    }
    public static string voltage {
            get {
                return ResourceManager.GetString("voltage", resourceCulture);
            }
    }
    public static string valveStatus {
            get {
                return ResourceManager.GetString("valveStatus", resourceCulture);
            }
    }
    public static string userId {
            get {
                return ResourceManager.GetString("userId", resourceCulture);
            }
    }
    public static string imei {
            get {
                return ResourceManager.GetString("imei", resourceCulture);
            }
    }
    public static string OrgName {
            get {
                return ResourceManager.GetString("OrgName", resourceCulture);
            }
    }
 }
}