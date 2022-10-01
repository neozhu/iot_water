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
/// File: MainMeterQuery.cs
/// Purpose: easyui datagrid filter query 
/// Created Date: 4/25/2020 11:13:49 AM
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
   public class MainMeterQuery:QueryObject<MainMeter>
   {
		public MainMeterQuery Withfilter(IEnumerable<filterRule> filters)
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
						if (rule.field == "date" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.date) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.date) <= 0);
						    }
						}
						if (rule.field == "inwater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.inwater == val);
                                break;
                            case "notequal":
                                And(x => x.inwater != val);
                                break;
                            case "less":
                                And(x => x.inwater < val);
                                break;
                            case "lessorequal":
                                And(x => x.inwater <= val);
                                break;
                            case "greater":
                                And(x => x.inwater > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.inwater >= val);
                                break;
                            default:
                                And(x => x.inwater == val);
                                break;
                        }
						}
						if (rule.field == "involume" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.involume == val);
                                break;
                            case "notequal":
                                And(x => x.involume != val);
                                break;
                            case "less":
                                And(x => x.involume < val);
                                break;
                            case "lessorequal":
                                And(x => x.involume <= val);
                                break;
                            case "greater":
                                And(x => x.involume > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.involume >= val);
                                break;
                            default:
                                And(x => x.involume == val);
                                break;
                        }
						}
						if (rule.field == "outwater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.outwater == val);
                                break;
                            case "notequal":
                                And(x => x.outwater != val);
                                break;
                            case "less":
                                And(x => x.outwater < val);
                                break;
                            case "lessorequal":
                                And(x => x.outwater <= val);
                                break;
                            case "greater":
                                And(x => x.outwater > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.outwater >= val);
                                break;
                            default:
                                And(x => x.outwater == val);
                                break;
                        }
						}
						if (rule.field == "outvolume" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.outvolume == val);
                                break;
                            case "notequal":
                                And(x => x.outvolume != val);
                                break;
                            case "less":
                                And(x => x.outvolume < val);
                                break;
                            case "lessorequal":
                                And(x => x.outvolume <= val);
                                break;
                            case "greater":
                                And(x => x.outvolume > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.outvolume >= val);
                                break;
                            default:
                                And(x => x.outvolume == val);
                                break;
                        }
						}
						if (rule.field == "remark"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.remark.Contains(rule.value));
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
    }
}
