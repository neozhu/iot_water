namespace WebApp.Models.resource {
  using System;
  [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  public class OrgChart {
        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal OrgChart() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebApp.Models.resource.OrgChart", typeof(OrgChart).Assembly);
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
    public static string No {
            get {
                return ResourceManager.GetString("No", resourceCulture);
            }
    }
    public static string Level {
            get {
                return ResourceManager.GetString("Level", resourceCulture);
            }
    }
    public static string LevelNo {
            get {
                return ResourceManager.GetString("LevelNo", resourceCulture);
            }
    }
    public static string Location {
            get {
                return ResourceManager.GetString("Location", resourceCulture);
            }
    }
    public static string Precision {
            get {
                return ResourceManager.GetString("Precision", resourceCulture);
            }
    }
    public static string DN {
            get {
                return ResourceManager.GetString("DN", resourceCulture);
            }
    }
    public static string Year {
            get {
                return ResourceManager.GetString("Year", resourceCulture);
            }
    }
    public static string Remark {
            get {
                return ResourceManager.GetString("Remark", resourceCulture);
            }
    }
    public static string ParentId {
            get {
                return ResourceManager.GetString("ParentId", resourceCulture);
            }
    }
 }
}