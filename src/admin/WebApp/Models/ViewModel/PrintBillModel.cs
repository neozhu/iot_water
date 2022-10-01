using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.ViewModel
{
  public class PrintBillModel
  {
    public PrintBillModel()
    {
      this.Headers = new List<BillHeader>();
    }
    public List<BillHeader> Headers { get; set; }
  }
}