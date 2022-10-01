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
/// File: WaterMeterRecordQuery.cs
/// Purpose: easyui datagrid filter query 
/// Created Date: 3/25/2020 8:17:43 PM
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
   public class WaterMeterRecordQuery:QueryObject<WaterMeterRecord>
   {
		public WaterMeterRecordQuery Withfilter(IEnumerable<filterRule> filters)
        {
           if (filters != null)
           {
               foreach (var rule in filters)
               {
						if (rule.field == "Id" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
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
                            case "greaterorequal" :
                                And(x => x.Id >= val);
                                break;
                            default:
                                And(x => x.Id == val);
                                break;
                        }
						}
						if (rule.field == "meterStatus"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.meterStatus.Contains(rule.value));
						}
						if (rule.field == "datetime" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.datetime) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.datetime) <= 0);
						    }
						}
						if (rule.field == "meterId"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.meterId.Contains(rule.value));
						}
						if (rule.field == "water" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
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
                            case "greaterorequal" :
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
							switch (rule.op) {
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
                            case "greaterorequal" :
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
							switch (rule.op) {
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
                            case "greaterorequal" :
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
							switch (rule.op) {
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
                            case "greaterorequal" :
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
							switch (rule.op) {
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
                            case "greaterorequal" :
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
							switch (rule.op) {
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
                            case "greaterorequal" :
                                And(x => x.voltage >= val);
                                break;
                            default:
                                And(x => x.voltage == val);
                                break;
                        }
						}
						if (rule.field == "valveStatus"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.valveStatus.Contains(rule.value));
						}
						if (rule.field == "userId"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.userId.Contains(rule.value));
						}
						if (rule.field == "imei"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.imei.Contains(rule.value));
						}
						if (rule.field == "OrgName"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.OrgName.Contains(rule.value));
						}
						if (rule.field == "CreatedDate" && !string.IsNullOrEmpty(rule.value) )
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
						if (rule.field == "CreatedBy"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.CreatedBy.Contains(rule.value));
						}
						if (rule.field == "LastModifiedDate" && !string.IsNullOrEmpty(rule.value) )
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
						if (rule.field == "LastModifiedBy"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.LastModifiedBy.Contains(rule.value));
						}
						if (rule.field == "TenantId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
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
                            case "greaterorequal" :
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
    public WaterMeterRecordQuery WithfilterWithMeterId(string meterid,IEnumerable<filterRule> filters)
    {
      And(x => x.meterId == meterid);
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
          if (rule.field == "meterStatus" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.meterStatus.Contains(rule.value));
          }
          if (rule.field == "datetime" && !string.IsNullOrEmpty(rule.value))
          {
            if (rule.op == "between")
            {
              var datearray = rule.value.Split(new char[] { '-' });
              var start = Convert.ToDateTime(datearray[0]);
              var end = Convert.ToDateTime(datearray[1]);

              And(x => SqlFunctions.DateDiff("d", start, x.datetime) >= 0);
              And(x => SqlFunctions.DateDiff("d", end, x.datetime) <= 0);
            }
          }
          if (rule.field == "meterId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.meterId.Contains(rule.value));
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
          if (rule.field == "valveStatus" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.valveStatus.Contains(rule.value));
          }
          if (rule.field == "userId" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.userId.Contains(rule.value));
          }
          if (rule.field == "imei" && !string.IsNullOrEmpty(rule.value))
          {
            And(x => x.imei.Contains(rule.value));
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
