namespace WebApp.Models.resource {
  using System;
  [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  public class Customer {
        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Customer() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebApp.Models.resource.Customer", typeof(Customer).Assembly);
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
    public static string Name {
            get {
                return ResourceManager.GetString("Name", resourceCulture);
            }
    }
    public static string Level {
            get {
                return ResourceManager.GetString("Level", resourceCulture);
            }
    }
    public static string Dept {
            get {
                return ResourceManager.GetString("Dept", resourceCulture);
            }
    }
    public static string Contact {
            get {
                return ResourceManager.GetString("Contact", resourceCulture);
            }
    }
    public static string ContactInfo {
            get {
                return ResourceManager.GetString("ContactInfo", resourceCulture);
            }
    }
    public static string MobilePhone {
            get {
                return ResourceManager.GetString("MobilePhone", resourceCulture);
            }
    }
    public static string Quota {
            get {
                return ResourceManager.GetString("Quota", resourceCulture);
            }
    }
    public static string Threshold {
            get {
                return ResourceManager.GetString("Threshold", resourceCulture);
            }
    }
    public static string IsFee {
            get {
                return ResourceManager.GetString("IsFee", resourceCulture);
            }
    }
    public static string Discount {
            get {
                return ResourceManager.GetString("Discount", resourceCulture);
            }
    }
    public static string RegisterDate {
            get {
                return ResourceManager.GetString("RegisterDate", resourceCulture);
            }
    }
    public static string Remark {
            get {
                return ResourceManager.GetString("Remark", resourceCulture);
            }
    }
 }
}