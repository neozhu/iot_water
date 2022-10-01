namespace WebApp.Models.resource {
  using System;
  [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  public class Employee {
        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Employee() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebApp.Models.resource.Employee", typeof(Employee).Assembly);
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
    public static string Title {
            get {
                return ResourceManager.GetString("Title", resourceCulture);
            }
    }
    public static string PhoneNumber {
            get {
                return ResourceManager.GetString("PhoneNumber", resourceCulture);
            }
    }
    public static string WX {
            get {
                return ResourceManager.GetString("WX", resourceCulture);
            }
    }
    public static string Sex {
            get {
                return ResourceManager.GetString("Sex", resourceCulture);
            }
    }
    public static string Age {
            get {
                return ResourceManager.GetString("Age", resourceCulture);
            }
    }
    public static string Brithday {
            get {
                return ResourceManager.GetString("Brithday", resourceCulture);
            }
    }
    public static string EntryDate {
            get {
                return ResourceManager.GetString("EntryDate", resourceCulture);
            }
    }
    public static string IsDeleted {
            get {
                return ResourceManager.GetString("IsDeleted", resourceCulture);
            }
    }
    public static string LeaveDate {
            get {
                return ResourceManager.GetString("LeaveDate", resourceCulture);
            }
    }
    public static string CompanyId {
            get {
                return ResourceManager.GetString("CompanyId", resourceCulture);
            }
    }
    public static string DepartmentId {
            get {
                return ResourceManager.GetString("DepartmentId", resourceCulture);
            }
    }
 }
}