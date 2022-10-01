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
/// File: ZoneWaterMeterQuery.cs
/// Purpose: easyui datagrid filter query 
/// Created Date: 3/28/2020 6:45:12 PM
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
   public class ZoneWaterMeterQuery:QueryObject<ZoneWaterMeter>
   {
		public ZoneWaterMeterQuery Withfilter(IEnumerable<filterRule> filters)
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
						if (rule.field == "ZoneId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.ZoneId == val);
                                break;
                            case "notequal":
                                And(x => x.ZoneId != val);
                                break;
                            case "less":
                                And(x => x.ZoneId < val);
                                break;
                            case "lessorequal":
                                And(x => x.ZoneId <= val);
                                break;
                            case "greater":
                                And(x => x.ZoneId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.ZoneId >= val);
                                break;
                            default:
                                And(x => x.ZoneId == val);
                                break;
                        }
						}
						if (rule.field == "Direct" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Direct == val);
                                break;
                            case "notequal":
                                And(x => x.Direct != val);
                                break;
                            case "less":
                                And(x => x.Direct < val);
                                break;
                            case "lessorequal":
                                And(x => x.Direct <= val);
                                break;
                            case "greater":
                                And(x => x.Direct > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Direct >= val);
                                break;
                            default:
                                And(x => x.Direct == val);
                                break;
                        }
						}
						if (rule.field == "meterId"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.meterId.Contains(rule.value));
						}
						if (rule.field == "ZoneName"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.ZoneName.Contains(rule.value));
						}
						if (rule.field == "longitude" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.longitude == val);
                                break;
                            case "notequal":
                                And(x => x.longitude != val);
                                break;
                            case "less":
                                And(x => x.longitude < val);
                                break;
                            case "lessorequal":
                                And(x => x.longitude <= val);
                                break;
                            case "greater":
                                And(x => x.longitude > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.longitude >= val);
                                break;
                            default:
                                And(x => x.longitude == val);
                                break;
                        }
						}
						if (rule.field == "latitude" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.latitude == val);
                                break;
                            case "notequal":
                                And(x => x.latitude != val);
                                break;
                            case "less":
                                And(x => x.latitude < val);
                                break;
                            case "lessorequal":
                                And(x => x.latitude <= val);
                                break;
                            case "greater":
                                And(x => x.latitude > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.latitude >= val);
                                break;
                            default:
                                And(x => x.latitude == val);
                                break;
                        }
						}
						if (rule.field == "Detail"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Detail.Contains(rule.value));
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
         public  ZoneWaterMeterQuery ByZoneIdWithfilter(int zoneid, IEnumerable<filterRule> filters)
         {
            And(x => x.ZoneId == zoneid);
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
						if (rule.field == "ZoneId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.ZoneId == val);
                                break;
                            case "notequal":
                                And(x => x.ZoneId != val);
                                break;
                            case "less":
                                And(x => x.ZoneId < val);
                                break;
                            case "lessorequal":
                                And(x => x.ZoneId <= val);
                                break;
                            case "greater":
                                And(x => x.ZoneId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.ZoneId >= val);
                                break;
                            default:
                                And(x => x.ZoneId == val);
                                break;
                        }
						}
						if (rule.field == "Direct" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Direct == val);
                                break;
                            case "notequal":
                                And(x => x.Direct != val);
                                break;
                            case "less":
                                And(x => x.Direct < val);
                                break;
                            case "lessorequal":
                                And(x => x.Direct <= val);
                                break;
                            case "greater":
                                And(x => x.Direct > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Direct >= val);
                                break;
                            default:
                                And(x => x.Direct == val);
                                break;
                        }
						}
						if (rule.field == "meterId"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.meterId == rule.value);
                           } 
                           else
                           {
							And(x => x.meterId.Contains(rule.value));
						    }
                        }
						if (rule.field == "ZoneName"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.ZoneName == rule.value);
                           } 
                           else
                           {
							And(x => x.ZoneName.Contains(rule.value));
						    }
                        }
						if (rule.field == "longitude" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.longitude == val);
                                break;
                            case "notequal":
                                And(x => x.longitude != val);
                                break;
                            case "less":
                                And(x => x.longitude < val);
                                break;
                            case "lessorequal":
                                And(x => x.longitude <= val);
                                break;
                            case "greater":
                                And(x => x.longitude > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.longitude >= val);
                                break;
                            default:
                                And(x => x.longitude == val);
                                break;
                        }
						}
						if (rule.field == "latitude" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.latitude == val);
                                break;
                            case "notequal":
                                And(x => x.latitude != val);
                                break;
                            case "less":
                                And(x => x.latitude < val);
                                break;
                            case "lessorequal":
                                And(x => x.latitude <= val);
                                break;
                            case "greater":
                                And(x => x.latitude > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.latitude >= val);
                                break;
                            default:
                                And(x => x.latitude == val);
                                break;
                        }
						}
						if (rule.field == "Detail"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.Detail == rule.value);
                           } 
                           else
                           {
							And(x => x.Detail.Contains(rule.value));
						    }
                        }
               }
            }
            return this;
         }    
    }
}
