namespace WebApp.Models.resource {
  using System;
  [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
  [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
  public class BillHeader {
        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal BillHeader() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WebApp.Models.resource.BillHeader", typeof(BillHeader).Assembly);
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
    public static string BillNo {
            get {
                return ResourceManager.GetString("BillNo", resourceCulture);
            }
    }
    public static string CustomerId {
            get {
                return ResourceManager.GetString("CustomerId", resourceCulture);
            }
    }
    public static string CustomerName {
            get {
                return ResourceManager.GetString("CustomerName", resourceCulture);
            }
    }
    public static string Category {
            get {
                return ResourceManager.GetString("Category", resourceCulture);
            }
    }
    public static string WaterPrice {
            get {
                return ResourceManager.GetString("WaterPrice", resourceCulture);
            }
    }
    public static string ServicePrice {
            get {
                return ResourceManager.GetString("ServicePrice", resourceCulture);
            }
    }
    public static string Discount {
            get {
                return ResourceManager.GetString("Discount", resourceCulture);
            }
    }
    public static string BillDate {
            get {
                return ResourceManager.GetString("BillDate", resourceCulture);
            }
    }
    public static string ReceiptDate {
            get {
                return ResourceManager.GetString("ReceiptDate", resourceCulture);
            }
    }
    public static string Month {
            get {
                return ResourceManager.GetString("Month", resourceCulture);
            }
    }
    public static string TotalWater {
            get {
                return ResourceManager.GetString("TotalWater", resourceCulture);
            }
    }
    public static string LastTotalWater {
            get {
                return ResourceManager.GetString("LastTotalWater", resourceCulture);
            }
    }
    public static string PerCent {
            get {
                return ResourceManager.GetString("PerCent", resourceCulture);
            }
    }
    public static string TotalWaterAmount {
            get {
                return ResourceManager.GetString("TotalWaterAmount", resourceCulture);
            }
    }
    public static string TotalServiceAmount {
            get {
                return ResourceManager.GetString("TotalServiceAmount", resourceCulture);
            }
    }
    public static string AdjustWater {
            get {
                return ResourceManager.GetString("AdjustWater", resourceCulture);
            }
    }
    public static string AdjustWaterAmount {
            get {
                return ResourceManager.GetString("AdjustWaterAmount", resourceCulture);
            }
    }
    public static string AdjustServiceAmount {
            get {
                return ResourceManager.GetString("AdjustServiceAmount", resourceCulture);
            }
    }
    public static string TotalAmount {
            get {
                return ResourceManager.GetString("TotalAmount", resourceCulture);
            }
    }
    public static string TotalReceivableAmount {
            get {
                return ResourceManager.GetString("TotalReceivableAmount", resourceCulture);
            }
    }
    public static string Status {
            get {
                return ResourceManager.GetString("Status", resourceCulture);
            }
    }
    public static string Remark {
            get {
                return ResourceManager.GetString("Remark", resourceCulture);
            }
    }
 }
}