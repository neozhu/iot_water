namespace WebApp.Models.resource {
  using System;
  [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  public class AlarmLog {
        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AlarmLog() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebApp.Models.resource.AlarmLog", typeof(AlarmLog).Assembly);
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
    public static string InitDateTime {
            get {
                return ResourceManager.GetString("InitDateTime", resourceCulture);
            }
    }
    public static string DeviceId {
            get {
                return ResourceManager.GetString("DeviceId", resourceCulture);
            }
    }
    public static string Content {
            get {
                return ResourceManager.GetString("Content", resourceCulture);
            }
    }
    public static string Status {
            get {
                return ResourceManager.GetString("Status", resourceCulture);
            }
    }
    public static string Type {
            get {
                return ResourceManager.GetString("Type", resourceCulture);
            }
    }
    public static string Level {
            get {
                return ResourceManager.GetString("Level", resourceCulture);
            }
    }
    public static string User {
            get {
                return ResourceManager.GetString("User", resourceCulture);
            }
    }
    public static string ToUser {
            get {
                return ResourceManager.GetString("ToUser", resourceCulture);
            }
    }
    public static string BeginDateTime {
            get {
                return ResourceManager.GetString("BeginDateTime", resourceCulture);
            }
    }
    public static string CompletedDateTime {
            get {
                return ResourceManager.GetString("CompletedDateTime", resourceCulture);
            }
    }
    public static string Result {
            get {
                return ResourceManager.GetString("Result", resourceCulture);
            }
    }
 }
}