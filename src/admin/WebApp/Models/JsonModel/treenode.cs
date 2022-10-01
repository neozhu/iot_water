using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.JsonModel
{
  public class treenode
  {
      public string text { get; set; }
    public string label { get; set; }
			public string icon { get; set; }
    public string data { get; set; }
      public treenode[] nodes { get; set; }
  }

  public class groupdetail {
    public string ZoneName { get; set; }
    public string ManageDept { get; set; }
    public string Name { get; set; }
    public string Points { get; set; }
    public string meterId {get;set;}
  }
  public class categorydata
  {
    public string Category { get; set; }
    public string Name { get; set; }
    public string Dept { get; set; }
    public string Points { get; set; }
    public string Id { get; set; }
  }

  public class logtotal
  {
    public DateTime time { get; set; }
    public int total { get; set; }
  }
  public class leveltotal
  {
    public string level { get; set; }
    public string total { get; set; }
  }
}