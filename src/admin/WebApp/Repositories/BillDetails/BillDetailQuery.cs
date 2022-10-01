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
/// File: BillDetailQuery.cs
/// Purpose: easyui datagrid filter query 
/// Created Date: 2/20/2021 9:15:28 PM
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
   public class BillDetailQuery:QueryObject<BillDetail>
   {
		public BillDetailQuery Withfilter(IEnumerable<filterRule> filters)
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
						if (rule.field == "BillId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.BillId == val);
                                break;
                            case "notequal":
                                And(x => x.BillId != val);
                                break;
                            case "less":
                                And(x => x.BillId < val);
                                break;
                            case "lessorequal":
                                And(x => x.BillId <= val);
                                break;
                            case "greater":
                                And(x => x.BillId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.BillId >= val);
                                break;
                            default:
                                And(x => x.BillId == val);
                                break;
                        }
						}
						if (rule.field == "MeterName"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.MeterName.Contains(rule.value));
						}
						if (rule.field == "LineNo"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.LineNo.Contains(rule.value));
						}
						if (rule.field == "MeterId"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.MeterId.Contains(rule.value));
						}
						if (rule.field == "MeterName1"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.MeterName1.Contains(rule.value));
						}
						if (rule.field == "MeterPoint"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.MeterPoint.Contains(rule.value));
						}
						if (rule.field == "Negitive"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Negitive.Contains(rule.value));
						}
						if (rule.field == "Ratio" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Ratio == val);
                                break;
                            case "notequal":
                                And(x => x.Ratio != val);
                                break;
                            case "less":
                                And(x => x.Ratio < val);
                                break;
                            case "lessorequal":
                                And(x => x.Ratio <= val);
                                break;
                            case "greater":
                                And(x => x.Ratio > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Ratio >= val);
                                break;
                            default:
                                And(x => x.Ratio == val);
                                break;
                        }
						}
						if (rule.field == "Water" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Water == val);
                                break;
                            case "notequal":
                                And(x => x.Water != val);
                                break;
                            case "less":
                                And(x => x.Water < val);
                                break;
                            case "lessorequal":
                                And(x => x.Water <= val);
                                break;
                            case "greater":
                                And(x => x.Water > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Water >= val);
                                break;
                            default:
                                And(x => x.Water == val);
                                break;
                        }
						}
						if (rule.field == "LastWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.LastWater == val);
                                break;
                            case "notequal":
                                And(x => x.LastWater != val);
                                break;
                            case "less":
                                And(x => x.LastWater < val);
                                break;
                            case "lessorequal":
                                And(x => x.LastWater <= val);
                                break;
                            case "greater":
                                And(x => x.LastWater > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.LastWater >= val);
                                break;
                            default:
                                And(x => x.LastWater == val);
                                break;
                        }
						}
						if (rule.field == "PerCent" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.PerCent == val);
                                break;
                            case "notequal":
                                And(x => x.PerCent != val);
                                break;
                            case "less":
                                And(x => x.PerCent < val);
                                break;
                            case "lessorequal":
                                And(x => x.PerCent <= val);
                                break;
                            case "greater":
                                And(x => x.PerCent > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.PerCent >= val);
                                break;
                            default:
                                And(x => x.PerCent == val);
                                break;
                        }
						}
						if (rule.field == "ActualWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.ActualWater == val);
                                break;
                            case "notequal":
                                And(x => x.ActualWater != val);
                                break;
                            case "less":
                                And(x => x.ActualWater < val);
                                break;
                            case "lessorequal":
                                And(x => x.ActualWater <= val);
                                break;
                            case "greater":
                                And(x => x.ActualWater > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.ActualWater >= val);
                                break;
                            default:
                                And(x => x.ActualWater == val);
                                break;
                        }
						}
						if (rule.field == "WaterDt1" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.WaterDt1) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.WaterDt1) <= 0);
						    }
						}
						if (rule.field == "Water1" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Water1 == val);
                                break;
                            case "notequal":
                                And(x => x.Water1 != val);
                                break;
                            case "less":
                                And(x => x.Water1 < val);
                                break;
                            case "lessorequal":
                                And(x => x.Water1 <= val);
                                break;
                            case "greater":
                                And(x => x.Water1 > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Water1 >= val);
                                break;
                            default:
                                And(x => x.Water1 == val);
                                break;
                        }
						}
						if (rule.field == "WaterDt2" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.WaterDt2) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.WaterDt2) <= 0);
						    }
						}
						if (rule.field == "Water2" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Water2 == val);
                                break;
                            case "notequal":
                                And(x => x.Water2 != val);
                                break;
                            case "less":
                                And(x => x.Water2 < val);
                                break;
                            case "lessorequal":
                                And(x => x.Water2 <= val);
                                break;
                            case "greater":
                                And(x => x.Water2 > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Water2 >= val);
                                break;
                            default:
                                And(x => x.Water2 == val);
                                break;
                        }
						}
						if (rule.field == "Remark"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Remark.Contains(rule.value));
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
         public  BillDetailQuery ByBillIdWithfilter(int billid, IEnumerable<filterRule> filters)
         {
            And(x => x.BillId == billid);
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
						if (rule.field == "BillId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.BillId == val);
                                break;
                            case "notequal":
                                And(x => x.BillId != val);
                                break;
                            case "less":
                                And(x => x.BillId < val);
                                break;
                            case "lessorequal":
                                And(x => x.BillId <= val);
                                break;
                            case "greater":
                                And(x => x.BillId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.BillId >= val);
                                break;
                            default:
                                And(x => x.BillId == val);
                                break;
                        }
						}
						if (rule.field == "MeterName"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.MeterName == rule.value);
                           } 
                           else
                           {
							And(x => x.MeterName.Contains(rule.value));
						    }
                        }
						if (rule.field == "LineNo"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.LineNo == rule.value);
                           } 
                           else
                           {
							And(x => x.LineNo.Contains(rule.value));
						    }
                        }
						if (rule.field == "MeterId"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.MeterId == rule.value);
                           } 
                           else
                           {
							And(x => x.MeterId.Contains(rule.value));
						    }
                        }
						if (rule.field == "MeterName1"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.MeterName1 == rule.value);
                           } 
                           else
                           {
							And(x => x.MeterName1.Contains(rule.value));
						    }
                        }
						if (rule.field == "MeterPoint"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.MeterPoint == rule.value);
                           } 
                           else
                           {
							And(x => x.MeterPoint.Contains(rule.value));
						    }
                        }
						if (rule.field == "Negitive"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.Negitive == rule.value);
                           } 
                           else
                           {
							And(x => x.Negitive.Contains(rule.value));
						    }
                        }
						if (rule.field == "Ratio" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Ratio == val);
                                break;
                            case "notequal":
                                And(x => x.Ratio != val);
                                break;
                            case "less":
                                And(x => x.Ratio < val);
                                break;
                            case "lessorequal":
                                And(x => x.Ratio <= val);
                                break;
                            case "greater":
                                And(x => x.Ratio > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Ratio >= val);
                                break;
                            default:
                                And(x => x.Ratio == val);
                                break;
                        }
						}
						if (rule.field == "Water" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Water == val);
                                break;
                            case "notequal":
                                And(x => x.Water != val);
                                break;
                            case "less":
                                And(x => x.Water < val);
                                break;
                            case "lessorequal":
                                And(x => x.Water <= val);
                                break;
                            case "greater":
                                And(x => x.Water > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Water >= val);
                                break;
                            default:
                                And(x => x.Water == val);
                                break;
                        }
						}
						if (rule.field == "LastWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.LastWater == val);
                                break;
                            case "notequal":
                                And(x => x.LastWater != val);
                                break;
                            case "less":
                                And(x => x.LastWater < val);
                                break;
                            case "lessorequal":
                                And(x => x.LastWater <= val);
                                break;
                            case "greater":
                                And(x => x.LastWater > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.LastWater >= val);
                                break;
                            default:
                                And(x => x.LastWater == val);
                                break;
                        }
						}
						if (rule.field == "PerCent" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.PerCent == val);
                                break;
                            case "notequal":
                                And(x => x.PerCent != val);
                                break;
                            case "less":
                                And(x => x.PerCent < val);
                                break;
                            case "lessorequal":
                                And(x => x.PerCent <= val);
                                break;
                            case "greater":
                                And(x => x.PerCent > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.PerCent >= val);
                                break;
                            default:
                                And(x => x.PerCent == val);
                                break;
                        }
						}
						if (rule.field == "ActualWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.ActualWater == val);
                                break;
                            case "notequal":
                                And(x => x.ActualWater != val);
                                break;
                            case "less":
                                And(x => x.ActualWater < val);
                                break;
                            case "lessorequal":
                                And(x => x.ActualWater <= val);
                                break;
                            case "greater":
                                And(x => x.ActualWater > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.ActualWater >= val);
                                break;
                            default:
                                And(x => x.ActualWater == val);
                                break;
                        }
						}
						if (rule.field == "WaterDt1" && !string.IsNullOrEmpty(rule.value) )
						{	
                            if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.WaterDt1) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.WaterDt1) <= 0);
						    }
                        }
						if (rule.field == "Water1" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Water1 == val);
                                break;
                            case "notequal":
                                And(x => x.Water1 != val);
                                break;
                            case "less":
                                And(x => x.Water1 < val);
                                break;
                            case "lessorequal":
                                And(x => x.Water1 <= val);
                                break;
                            case "greater":
                                And(x => x.Water1 > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Water1 >= val);
                                break;
                            default:
                                And(x => x.Water1 == val);
                                break;
                        }
						}
						if (rule.field == "WaterDt2" && !string.IsNullOrEmpty(rule.value) )
						{	
                            if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.WaterDt2) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.WaterDt2) <= 0);
						    }
                        }
						if (rule.field == "Water2" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Water2 == val);
                                break;
                            case "notequal":
                                And(x => x.Water2 != val);
                                break;
                            case "less":
                                And(x => x.Water2 < val);
                                break;
                            case "lessorequal":
                                And(x => x.Water2 <= val);
                                break;
                            case "greater":
                                And(x => x.Water2 > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Water2 >= val);
                                break;
                            default:
                                And(x => x.Water2 == val);
                                break;
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
               }
            }
            return this;
         }    
    }
}
