namespace WebApp.Models.resource {
  using System;
  [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  public class ZoneWaterMeter {
        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ZoneWaterMeter() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebApp.Models.resource.ZoneWaterMeter", typeof(ZoneWaterMeter).Assembly);
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
    public static string ZoneId {
            get {
                return ResourceManager.GetString("ZoneId", resourceCulture);
            }
    }
    public static string Direct {
            get {
                return ResourceManager.GetString("Direct", resourceCulture);
            }
    }
    public static string meterId {
            get {
                return ResourceManager.GetString("meterId", resourceCulture);
            }
    }
    public static string ZoneName {
            get {
                return ResourceManager.GetString("ZoneName", resourceCulture);
            }
    }
    public static string longitude {
            get {
                return ResourceManager.GetString("longitude", resourceCulture);
            }
    }
    public static string latitude {
            get {
                return ResourceManager.GetString("latitude", resourceCulture);
            }
    }
    public static string Detail {
            get {
                return ResourceManager.GetString("Detail", resourceCulture);
            }
    }
 }
}