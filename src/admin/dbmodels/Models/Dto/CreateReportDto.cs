using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Dto
{
  public class CreateReportDto
  {
    public string meterId { get; set; }
    public string Month { get; set; }
    public int Day { get; set; }
  }
}