namespace WebApp.Models.resource {
  using System;
  [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  public class Log {
        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Log() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebApp.Models.resource.Log", typeof(Log).Assembly);
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
    public static string MachineName {
            get {
                return ResourceManager.GetString("MachineName", resourceCulture);
            }
    }
    public static string Logged {
            get {
                return ResourceManager.GetString("Logged", resourceCulture);
            }
    }
    public static string Level {
            get {
                return ResourceManager.GetString("Level", resourceCulture);
            }
    }
    public static string Message {
            get {
                return ResourceManager.GetString("Message", resourceCulture);
            }
    }
    public static string Exception {
            get {
                return ResourceManager.GetString("Exception", resourceCulture);
            }
    }
    public static string Properties {
            get {
                return ResourceManager.GetString("Properties", resourceCulture);
            }
    }
    public static string User {
            get {
                return ResourceManager.GetString("User", resourceCulture);
            }
    }
    public static string Logger {
            get {
                return ResourceManager.GetString("Logger", resourceCulture);
            }
    }
    public static string Callsite {
            get {
                return ResourceManager.GetString("Callsite", resourceCulture);
            }
    }
    public static string Resolved {
            get {
                return ResourceManager.GetString("Resolved", resourceCulture);
            }
    }
 }
}