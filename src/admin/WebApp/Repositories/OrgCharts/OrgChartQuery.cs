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
/// File: OrgChartQuery.cs
/// Purpose: easyui datagrid filter query 
/// Created Date: 2020/11/3 19:39:54
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
   public class OrgChartQuery:QueryObject<OrgChart>
   {
		public OrgChartQuery Withfilter(IEnumerable<filterRule> filters)
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
						if (rule.field == "No"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.No.Contains(rule.value));
						}
						if (rule.field == "Level"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Level.Contains(rule.value));
						}
						if (rule.field == "LevelNo"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.LevelNo.Contains(rule.value));
						}
						if (rule.field == "Location"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Location.Contains(rule.value));
						}
						if (rule.field == "Precision"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Precision.Contains(rule.value));
						}
						if (rule.field == "DN"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.DN.Contains(rule.value));
						}
						if (rule.field == "Year"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Year.Contains(rule.value));
						}
						if (rule.field == "Remark"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Remark.Contains(rule.value));
						}
						if (rule.field == "ParentId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.ParentId == val);
                                break;
                            case "notequal":
                                And(x => x.ParentId != val);
                                break;
                            case "less":
                                And(x => x.ParentId < val);
                                break;
                            case "lessorequal":
                                And(x => x.ParentId <= val);
                                break;
                            case "greater":
                                And(x => x.ParentId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.ParentId >= val);
                                break;
                            default:
                                And(x => x.ParentId == val);
                                break;
                        }
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
         public  OrgChartQuery ByParentIdWithfilter(int parentid, IEnumerable<filterRule> filters)
         {
            And(x => x.ParentId == parentid);
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
						if (rule.field == "No"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.No == rule.value);
                           } 
                           else
                           {
							And(x => x.No.Contains(rule.value));
						    }
                        }
						if (rule.field == "Level"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.Level == rule.value);
                           } 
                           else
                           {
							And(x => x.Level.Contains(rule.value));
						    }
                        }
						if (rule.field == "LevelNo"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.LevelNo == rule.value);
                           } 
                           else
                           {
							And(x => x.LevelNo.Contains(rule.value));
						    }
                        }
						if (rule.field == "Location"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.Location == rule.value);
                           } 
                           else
                           {
							And(x => x.Location.Contains(rule.value));
						    }
                        }
						if (rule.field == "Precision"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.Precision == rule.value);
                           } 
                           else
                           {
							And(x => x.Precision.Contains(rule.value));
						    }
                        }
						if (rule.field == "DN"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.DN == rule.value);
                           } 
                           else
                           {
							And(x => x.DN.Contains(rule.value));
						    }
                        }
						if (rule.field == "Year"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.Year == rule.value);
                           } 
                           else
                           {
							And(x => x.Year.Contains(rule.value));
						    }
                        }
						if (rule.field == "Remark"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.Remark == rule.value);
                           } 
                           else
                           {
							And(x => x.Remark.Contains(rule.value));
						    }
                        }
						if (rule.field == "ParentId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.ParentId == val);
                                break;
                            case "notequal":
                                And(x => x.ParentId != val);
                                break;
                            case "less":
                                And(x => x.ParentId < val);
                                break;
                            case "lessorequal":
                                And(x => x.ParentId <= val);
                                break;
                            case "greater":
                                And(x => x.ParentId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.ParentId >= val);
                                break;
                            default:
                                And(x => x.ParentId == val);
                                break;
                        }
						}
               }
            }
            return this;
         }    
    }
}
