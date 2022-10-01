using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity.SqlServer;
using Repository.Pattern.Repositories;
using Repository.Pattern.Ef6;
using System.Web.WebPages;
using WebApp.Models;

namespace WebApp.Repositories
{
  /// <summary>
  /// File: WaterMeterQuery.cs
  /// Purpose: easyui datagrid filter query 
  /// Created Date: 3/25/2020 9:40:42 AM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  public class WaterMeterQuery : QueryObject<WaterMeter>
  {
    public WaterMeterQuery Withfilter(IEnumerable<filterRule> filters, string allmeterid = "")
    {
      if (!string.IsNullOrEmpty(allmeterid))
      {
        var array = allmeterid.Split(',');
        And(x => array.Contains(x.meterId));
      }

      if (filters != null)
      {
        foreach (var rule in filters)
        {
          if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.Id == val);
                break;
              case "notequal":
                And(x => x.Id != val);
                break;
              case "less":
                And(x => x.Id < val);
                break;
              case "lessorequal":
                And(x => x.Id <= val);
                break;
              case "greater":
                And(x => x.Id > val);
                break;
              case "greaterorequal":
                And(x => x.Id >= val);
                break;
              default:
                And(x => x.Id == val);
                break;
            }
          }
          if (rule.field == "meterId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.meterId.Contains(rule.value));
          }
          if (rule.field == "Level" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Level.Contains(rule.value));
          }
          if (rule.field == "Status" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Status.Contains(rule.value));
          }
          if (rule.field == "valveStatus" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.valveStatus.Contains(rule.value));
          }
          if (rule.field == "address" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.address.Contains(rule.value));
          }
          if (rule.field == "meterSize" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.meterSize.Contains(rule.value));
          }
          if (rule.field == "meterType" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.meterType.Contains(rule.value));
          }
          if (rule.field == "vender" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.vender.Contains(rule.value));
          }
          if (rule.field == "repairCycle" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.repairCycle == val);
                break;
              case "notequal":
                And(x => x.repairCycle != val);
                break;
              case "less":
                And(x => x.repairCycle < val);
                break;
              case "lessorequal":
                And(x => x.repairCycle <= val);
                break;
              case "greater":
                And(x => x.repairCycle > val);
                break;
              case "greaterorequal":
                And(x => x.repairCycle >= val);
                break;
              default:
                And(x => x.repairCycle == val);
                break;
            }
          }
          if (rule.field == "precision" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.precision == val);
                break;
              case "notequal":
                And(x => x.precision != val);
                break;
              case "less":
                And(x => x.precision < val);
                break;
              case "lessorequal":
                And(x => x.precision <= val);
                break;
              case "greater":
                And(x => x.precision > val);
                break;
              case "greaterorequal":
                And(x => x.precision >= val);
                break;
              default:
                And(x => x.precision == val);
                break;
            }
          }
          if (rule.field == "produced" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.produced) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.produced) <= 0);
            }
          }
          if (rule.field == "period" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.period) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.period) <= 0);
            }
          }
          if (rule.field == "sealNumber1" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.sealNumber1.Contains(rule.value));
          }
          if (rule.field == "sealNumber2" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.sealNumber2.Contains(rule.value));
          }
          if (rule.field == "sealNumber3" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.sealNumber3.Contains(rule.value));
          }
          if (rule.field == "imei" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.imei.Contains(rule.value));
          }
          if (rule.field == "water" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.water == val);
                break;
              case "notequal":
                And(x => x.water != val);
                break;
              case "less":
                And(x => x.water < val);
                break;
              case "lessorequal":
                And(x => x.water <= val);
                break;
              case "greater":
                And(x => x.water > val);
                break;
              case "greaterorequal":
                And(x => x.water >= val);
                break;
              default:
                And(x => x.water == val);
                break;
            }
          }
          if (rule.field == "reverseWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.reverseWater == val);
                break;
              case "notequal":
                And(x => x.reverseWater != val);
                break;
              case "less":
                And(x => x.reverseWater < val);
                break;
              case "lessorequal":
                And(x => x.reverseWater <= val);
                break;
              case "greater":
                And(x => x.reverseWater > val);
                break;
              case "greaterorequal":
                And(x => x.reverseWater >= val);
                break;
              default:
                And(x => x.reverseWater == val);
                break;
            }
          }
          if (rule.field == "temperature" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.temperature == val);
                break;
              case "notequal":
                And(x => x.temperature != val);
                break;
              case "less":
                And(x => x.temperature < val);
                break;
              case "lessorequal":
                And(x => x.temperature <= val);
                break;
              case "greater":
                And(x => x.temperature > val);
                break;
              case "greaterorequal":
                And(x => x.temperature >= val);
                break;
              default:
                And(x => x.temperature == val);
                break;
            }
          }
          if (rule.field == "flowrate" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.flowrate == val);
                break;
              case "notequal":
                And(x => x.flowrate != val);
                break;
              case "less":
                And(x => x.flowrate < val);
                break;
              case "lessorequal":
                And(x => x.flowrate <= val);
                break;
              case "greater":
                And(x => x.flowrate > val);
                break;
              case "greaterorequal":
                And(x => x.flowrate >= val);
                break;
              default:
                And(x => x.flowrate == val);
                break;
            }
          }
          if (rule.field == "pressure" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.pressure == val);
                break;
              case "notequal":
                And(x => x.pressure != val);
                break;
              case "less":
                And(x => x.pressure < val);
                break;
              case "lessorequal":
                And(x => x.pressure <= val);
                break;
              case "greater":
                And(x => x.pressure > val);
                break;
              case "greaterorequal":
                And(x => x.pressure >= val);
                break;
              default:
                And(x => x.pressure == val);
                break;
            }
          }
          if (rule.field == "voltage" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.voltage == val);
                break;
              case "notequal":
                And(x => x.voltage != val);
                break;
              case "less":
                And(x => x.voltage < val);
                break;
              case "lessorequal":
                And(x => x.voltage <= val);
                break;
              case "greater":
                And(x => x.voltage > val);
                break;
              case "greaterorequal":
                And(x => x.voltage >= val);
                break;
              default:
                And(x => x.voltage == val);
                break;
            }
          }
          if (rule.field == "threshold1" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.threshold1 == val);
                break;
              case "notequal":
                And(x => x.threshold1 != val);
                break;
              case "less":
                And(x => x.threshold1 < val);
                break;
              case "lessorequal":
                And(x => x.threshold1 <= val);
                break;
              case "greater":
                And(x => x.threshold1 > val);
                break;
              case "greaterorequal":
                And(x => x.threshold1 >= val);
                break;
              default:
                And(x => x.threshold1 == val);
                break;
            }
          }
          if (rule.field == "threshold2" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.threshold2 == val);
                break;
              case "notequal":
                And(x => x.threshold2 != val);
                break;
              case "less":
                And(x => x.threshold2 < val);
                break;
              case "lessorequal":
                And(x => x.threshold2 <= val);
                break;
              case "greater":
                And(x => x.threshold2 > val);
                break;
              case "greaterorequal":
                And(x => x.threshold2 >= val);
                break;
              default:
                And(x => x.threshold2 == val);
                break;
            }
          }
          if (rule.field == "threshold3" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.threshold3 == val);
                break;
              case "notequal":
                And(x => x.threshold3 != val);
                break;
              case "less":
                And(x => x.threshold3 < val);
                break;
              case "lessorequal":
                And(x => x.threshold3 <= val);
                break;
              case "greater":
                And(x => x.threshold3 > val);
                break;
              case "greaterorequal":
                And(x => x.threshold3 >= val);
                break;
              default:
                And(x => x.threshold3 == val);
                break;
            }
          }
          if (rule.field == "threshold4" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
          {
            var val = Convert.ToDecimal(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.threshold4 == val);
                break;
              case "notequal":
                And(x => x.threshold4 != val);
                break;
              case "less":
                And(x => x.threshold4 < val);
                break;
              case "lessorequal":
                And(x => x.threshold4 <= val);
                break;
              case "greater":
                And(x => x.threshold4 > val);
                break;
              case "greaterorequal":
                And(x => x.threshold4 >= val);
                break;
              default:
                And(x => x.threshold4 == val);
                break;
            }
          }
          if (rule.field == "userCode" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.userCode.Contains(rule.value));
          }
          if (rule.field == "userName" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.userName.Contains(rule.value));
          }
          if (rule.field == "Remark" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.Remark.Contains(rule.value));
          }
          if (rule.field == "CustomerId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.CustomerId == val);
                break;
              case "notequal":
                And(x => x.CustomerId != val);
                break;
              case "less":
                And(x => x.CustomerId < val);
                break;
              case "lessorequal":
                And(x => x.CustomerId <= val);
                break;
              case "greater":
                And(x => x.CustomerId > val);
                break;
              case "greaterorequal":
                And(x => x.CustomerId >= val);
                break;
              default:
                And(x => x.CustomerId == val);
                break;
            }
          }
          if (rule.field == "CustomerName" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CustomerName.Contains(rule.value));
          }
          if (rule.field == "OrgName" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.OrgName.Contains(rule.value));
          }
          if (rule.field == "CreatedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.CreatedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.CreatedDate) <= 0);
            }
          }
          if (rule.field == "CreatedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.CreatedBy.Contains(rule.value));
          }
          if (rule.field == "LastModifiedDate" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.LastModifiedDate) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.LastModifiedDate) <= 0);
            }
          }
          if (rule.field == "LastModifiedBy" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.LastModifiedBy.Contains(rule.value));
          }
          if (rule.field == "TenantId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
          {
            var val = Convert.ToInt32(rule.value);
            switch (rule.op)
            {
              case "equal":
                And(x => x.TenantId == val);
                break;
              case "notequal":
                And(x => x.TenantId != val);
                break;
              case "less":
                And(x => x.TenantId < val);
                break;
              case "lessorequal":
                And(x => x.TenantId <= val);
                break;
              case "greater":
                And(x => x.TenantId > val);
                break;
              case "greaterorequal":
                And(x => x.TenantId >= val);
                break;
              default:
                And(x => x.TenantId == val);
                break;
            }
          }

        }
      }
      return this;
    }
  }
}
