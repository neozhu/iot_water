namespace WebApp.Models.resource {
  using System;
  [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  public class WaterManualReport {
        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal WaterManualReport() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebApp.Models.resource.WaterManualReport", typeof(WaterManualReport).Assembly);
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
    public static string meterId {
            get {
                return ResourceManager.GetString("meterId", resourceCulture);
            }
    }
    public static string Name {
            get {
                return ResourceManager.GetString("Name", resourceCulture);
            }
    }
    public static string LineNo {
            get {
                return ResourceManager.GetString("LineNo", resourceCulture);
            }
    }
    public static string Name1 {
            get {
                return ResourceManager.GetString("Name1", resourceCulture);
            }
    }
    public static string Water {
            get {
                return ResourceManager.GetString("Water", resourceCulture);
            }
    }
    public static string RecordDate {
            get {
                return ResourceManager.GetString("RecordDate", resourceCulture);
            }
    }
    public static string LastWater {
            get {
                return ResourceManager.GetString("LastWater", resourceCulture);
            }
    }
    public static string LastRecordDate {
            get {
                return ResourceManager.GetString("LastRecordDate", resourceCulture);
            }
    }
    public static string CalWater {
            get {
                return ResourceManager.GetString("CalWater", resourceCulture);
            }
    }
    public static string Remark {
            get {
                return ResourceManager.GetString("Remark", resourceCulture);
            }
    }
 }
}