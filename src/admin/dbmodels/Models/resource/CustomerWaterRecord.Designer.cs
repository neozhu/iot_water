namespace WebApp.Models.resource {
  using System;
  [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  public class CustomerWaterRecord {
        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CustomerWaterRecord() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebApp.Models.resource.CustomerWaterRecord", typeof(CustomerWaterRecord).Assembly);
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
    public static string CustomerId {
            get {
                return ResourceManager.GetString("CustomerId", resourceCulture);
            }
    }
    public static string Year {
            get {
                return ResourceManager.GetString("Year", resourceCulture);
            }
    }
    public static string Month {
            get {
                return ResourceManager.GetString("Month", resourceCulture);
            }
    }
    public static string meterId {
            get {
                return ResourceManager.GetString("meterId", resourceCulture);
            }
    }
    public static string previousDate {
            get {
                return ResourceManager.GetString("previousDate", resourceCulture);
            }
    }
    public static string previousWater {
            get {
                return ResourceManager.GetString("previousWater", resourceCulture);
            }
    }
    public static string lastWater {
            get {
                return ResourceManager.GetString("lastWater", resourceCulture);
            }
    }
    public static string water {
            get {
                return ResourceManager.GetString("water", resourceCulture);
            }
    }
    public static string RecordDate {
            get {
                return ResourceManager.GetString("RecordDate", resourceCulture);
            }
    }
    public static string User {
            get {
                return ResourceManager.GetString("User", resourceCulture);
            }
    }
    public static string Type {
            get {
                return ResourceManager.GetString("Type", resourceCulture);
            }
    }
    public static string IsDelete {
            get {
                return ResourceManager.GetString("IsDelete", resourceCulture);
            }
    }
 }
}