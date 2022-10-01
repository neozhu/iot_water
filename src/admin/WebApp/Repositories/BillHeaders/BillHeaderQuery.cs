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
/// File: BillHeaderQuery.cs
/// Purpose: easyui datagrid filter query 
/// Created Date: 2/19/2021 8:29:49 PM
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
   public class BillHeaderQuery:QueryObject<BillHeader>
   {
		public BillHeaderQuery Withfilter(IEnumerable<filterRule> filters)
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
						if (rule.field == "BillNo"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.BillNo.Contains(rule.value));
						}
						if (rule.field == "CustomerId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
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
                            case "greaterorequal" :
                                And(x => x.CustomerId >= val);
                                break;
                            default:
                                And(x => x.CustomerId == val);
                                break;
                        }
						}
						if (rule.field == "CustomerName"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.CustomerName.Contains(rule.value));
						}
						if (rule.field == "Category"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Category.Contains(rule.value));
						}
						if (rule.field == "WaterPrice" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.WaterPrice == val);
                                break;
                            case "notequal":
                                And(x => x.WaterPrice != val);
                                break;
                            case "less":
                                And(x => x.WaterPrice < val);
                                break;
                            case "lessorequal":
                                And(x => x.WaterPrice <= val);
                                break;
                            case "greater":
                                And(x => x.WaterPrice > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.WaterPrice >= val);
                                break;
                            default:
                                And(x => x.WaterPrice == val);
                                break;
                        }
						}
						if (rule.field == "ServicePrice" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.ServicePrice == val);
                                break;
                            case "notequal":
                                And(x => x.ServicePrice != val);
                                break;
                            case "less":
                                And(x => x.ServicePrice < val);
                                break;
                            case "lessorequal":
                                And(x => x.ServicePrice <= val);
                                break;
                            case "greater":
                                And(x => x.ServicePrice > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.ServicePrice >= val);
                                break;
                            default:
                                And(x => x.ServicePrice == val);
                                break;
                        }
						}
						if (rule.field == "Discount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Discount == val);
                                break;
                            case "notequal":
                                And(x => x.Discount != val);
                                break;
                            case "less":
                                And(x => x.Discount < val);
                                break;
                            case "lessorequal":
                                And(x => x.Discount <= val);
                                break;
                            case "greater":
                                And(x => x.Discount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Discount >= val);
                                break;
                            default:
                                And(x => x.Discount == val);
                                break;
                        }
						}
						if (rule.field == "BillDate" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.BillDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.BillDate) <= 0);
						    }
						}
						if (rule.field == "ReceiptDate" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.ReceiptDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.ReceiptDate) <= 0);
						    }
						}
						if (rule.field == "Month"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Month.Contains(rule.value));
						}
						if (rule.field == "TotalWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.TotalWater == val);
                                break;
                            case "notequal":
                                And(x => x.TotalWater != val);
                                break;
                            case "less":
                                And(x => x.TotalWater < val);
                                break;
                            case "lessorequal":
                                And(x => x.TotalWater <= val);
                                break;
                            case "greater":
                                And(x => x.TotalWater > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.TotalWater >= val);
                                break;
                            default:
                                And(x => x.TotalWater == val);
                                break;
                        }
						}
						if (rule.field == "LastTotalWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.LastTotalWater == val);
                                break;
                            case "notequal":
                                And(x => x.LastTotalWater != val);
                                break;
                            case "less":
                                And(x => x.LastTotalWater < val);
                                break;
                            case "lessorequal":
                                And(x => x.LastTotalWater <= val);
                                break;
                            case "greater":
                                And(x => x.LastTotalWater > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.LastTotalWater >= val);
                                break;
                            default:
                                And(x => x.LastTotalWater == val);
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
						if (rule.field == "TotalWaterAmount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.TotalWaterAmount == val);
                                break;
                            case "notequal":
                                And(x => x.TotalWaterAmount != val);
                                break;
                            case "less":
                                And(x => x.TotalWaterAmount < val);
                                break;
                            case "lessorequal":
                                And(x => x.TotalWaterAmount <= val);
                                break;
                            case "greater":
                                And(x => x.TotalWaterAmount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.TotalWaterAmount >= val);
                                break;
                            default:
                                And(x => x.TotalWaterAmount == val);
                                break;
                        }
						}
						if (rule.field == "TotalServiceAmount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.TotalServiceAmount == val);
                                break;
                            case "notequal":
                                And(x => x.TotalServiceAmount != val);
                                break;
                            case "less":
                                And(x => x.TotalServiceAmount < val);
                                break;
                            case "lessorequal":
                                And(x => x.TotalServiceAmount <= val);
                                break;
                            case "greater":
                                And(x => x.TotalServiceAmount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.TotalServiceAmount >= val);
                                break;
                            default:
                                And(x => x.TotalServiceAmount == val);
                                break;
                        }
						}
						if (rule.field == "AdjustWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.AdjustWater == val);
                                break;
                            case "notequal":
                                And(x => x.AdjustWater != val);
                                break;
                            case "less":
                                And(x => x.AdjustWater < val);
                                break;
                            case "lessorequal":
                                And(x => x.AdjustWater <= val);
                                break;
                            case "greater":
                                And(x => x.AdjustWater > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.AdjustWater >= val);
                                break;
                            default:
                                And(x => x.AdjustWater == val);
                                break;
                        }
						}
						if (rule.field == "AdjustWaterAmount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.AdjustWaterAmount == val);
                                break;
                            case "notequal":
                                And(x => x.AdjustWaterAmount != val);
                                break;
                            case "less":
                                And(x => x.AdjustWaterAmount < val);
                                break;
                            case "lessorequal":
                                And(x => x.AdjustWaterAmount <= val);
                                break;
                            case "greater":
                                And(x => x.AdjustWaterAmount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.AdjustWaterAmount >= val);
                                break;
                            default:
                                And(x => x.AdjustWaterAmount == val);
                                break;
                        }
						}
						if (rule.field == "AdjustServiceAmount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.AdjustServiceAmount == val);
                                break;
                            case "notequal":
                                And(x => x.AdjustServiceAmount != val);
                                break;
                            case "less":
                                And(x => x.AdjustServiceAmount < val);
                                break;
                            case "lessorequal":
                                And(x => x.AdjustServiceAmount <= val);
                                break;
                            case "greater":
                                And(x => x.AdjustServiceAmount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.AdjustServiceAmount >= val);
                                break;
                            default:
                                And(x => x.AdjustServiceAmount == val);
                                break;
                        }
						}
						if (rule.field == "TotalAmount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.TotalAmount == val);
                                break;
                            case "notequal":
                                And(x => x.TotalAmount != val);
                                break;
                            case "less":
                                And(x => x.TotalAmount < val);
                                break;
                            case "lessorequal":
                                And(x => x.TotalAmount <= val);
                                break;
                            case "greater":
                                And(x => x.TotalAmount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.TotalAmount >= val);
                                break;
                            default:
                                And(x => x.TotalAmount == val);
                                break;
                        }
						}
						if (rule.field == "TotalReceivableAmount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.TotalReceivableAmount == val);
                                break;
                            case "notequal":
                                And(x => x.TotalReceivableAmount != val);
                                break;
                            case "less":
                                And(x => x.TotalReceivableAmount < val);
                                break;
                            case "lessorequal":
                                And(x => x.TotalReceivableAmount <= val);
                                break;
                            case "greater":
                                And(x => x.TotalReceivableAmount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.TotalReceivableAmount >= val);
                                break;
                            default:
                                And(x => x.TotalReceivableAmount == val);
                                break;
                        }
						}
						if (rule.field == "Status"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Status.Contains(rule.value));
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
         public  BillHeaderQuery ByCustomerIdWithfilter(int customerid, IEnumerable<filterRule> filters)
         {
            And(x => x.CustomerId == customerid);
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
						if (rule.field == "BillNo"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.BillNo == rule.value);
                           } 
                           else
                           {
							And(x => x.BillNo.Contains(rule.value));
						    }
                        }
						if (rule.field == "CustomerId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
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
                            case "greaterorequal" :
                                And(x => x.CustomerId >= val);
                                break;
                            default:
                                And(x => x.CustomerId == val);
                                break;
                        }
						}
						if (rule.field == "CustomerName"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.CustomerName == rule.value);
                           } 
                           else
                           {
							And(x => x.CustomerName.Contains(rule.value));
						    }
                        }
						if (rule.field == "Category"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.Category == rule.value);
                           } 
                           else
                           {
							And(x => x.Category.Contains(rule.value));
						    }
                        }
						if (rule.field == "WaterPrice" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.WaterPrice == val);
                                break;
                            case "notequal":
                                And(x => x.WaterPrice != val);
                                break;
                            case "less":
                                And(x => x.WaterPrice < val);
                                break;
                            case "lessorequal":
                                And(x => x.WaterPrice <= val);
                                break;
                            case "greater":
                                And(x => x.WaterPrice > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.WaterPrice >= val);
                                break;
                            default:
                                And(x => x.WaterPrice == val);
                                break;
                        }
						}
						if (rule.field == "ServicePrice" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.ServicePrice == val);
                                break;
                            case "notequal":
                                And(x => x.ServicePrice != val);
                                break;
                            case "less":
                                And(x => x.ServicePrice < val);
                                break;
                            case "lessorequal":
                                And(x => x.ServicePrice <= val);
                                break;
                            case "greater":
                                And(x => x.ServicePrice > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.ServicePrice >= val);
                                break;
                            default:
                                And(x => x.ServicePrice == val);
                                break;
                        }
						}
						if (rule.field == "Discount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Discount == val);
                                break;
                            case "notequal":
                                And(x => x.Discount != val);
                                break;
                            case "less":
                                And(x => x.Discount < val);
                                break;
                            case "lessorequal":
                                And(x => x.Discount <= val);
                                break;
                            case "greater":
                                And(x => x.Discount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Discount >= val);
                                break;
                            default:
                                And(x => x.Discount == val);
                                break;
                        }
						}
						if (rule.field == "BillDate" && !string.IsNullOrEmpty(rule.value) )
						{	
                            if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.BillDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.BillDate) <= 0);
						    }
                        }
						if (rule.field == "ReceiptDate" && !string.IsNullOrEmpty(rule.value) )
						{	
                            if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.ReceiptDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.ReceiptDate) <= 0);
						    }
                        }
						if (rule.field == "Month"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.Month == rule.value);
                           } 
                           else
                           {
							And(x => x.Month.Contains(rule.value));
						    }
                        }
						if (rule.field == "TotalWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.TotalWater == val);
                                break;
                            case "notequal":
                                And(x => x.TotalWater != val);
                                break;
                            case "less":
                                And(x => x.TotalWater < val);
                                break;
                            case "lessorequal":
                                And(x => x.TotalWater <= val);
                                break;
                            case "greater":
                                And(x => x.TotalWater > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.TotalWater >= val);
                                break;
                            default:
                                And(x => x.TotalWater == val);
                                break;
                        }
						}
						if (rule.field == "LastTotalWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.LastTotalWater == val);
                                break;
                            case "notequal":
                                And(x => x.LastTotalWater != val);
                                break;
                            case "less":
                                And(x => x.LastTotalWater < val);
                                break;
                            case "lessorequal":
                                And(x => x.LastTotalWater <= val);
                                break;
                            case "greater":
                                And(x => x.LastTotalWater > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.LastTotalWater >= val);
                                break;
                            default:
                                And(x => x.LastTotalWater == val);
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
						if (rule.field == "TotalWaterAmount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.TotalWaterAmount == val);
                                break;
                            case "notequal":
                                And(x => x.TotalWaterAmount != val);
                                break;
                            case "less":
                                And(x => x.TotalWaterAmount < val);
                                break;
                            case "lessorequal":
                                And(x => x.TotalWaterAmount <= val);
                                break;
                            case "greater":
                                And(x => x.TotalWaterAmount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.TotalWaterAmount >= val);
                                break;
                            default:
                                And(x => x.TotalWaterAmount == val);
                                break;
                        }
						}
						if (rule.field == "TotalServiceAmount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.TotalServiceAmount == val);
                                break;
                            case "notequal":
                                And(x => x.TotalServiceAmount != val);
                                break;
                            case "less":
                                And(x => x.TotalServiceAmount < val);
                                break;
                            case "lessorequal":
                                And(x => x.TotalServiceAmount <= val);
                                break;
                            case "greater":
                                And(x => x.TotalServiceAmount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.TotalServiceAmount >= val);
                                break;
                            default:
                                And(x => x.TotalServiceAmount == val);
                                break;
                        }
						}
						if (rule.field == "AdjustWater" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.AdjustWater == val);
                                break;
                            case "notequal":
                                And(x => x.AdjustWater != val);
                                break;
                            case "less":
                                And(x => x.AdjustWater < val);
                                break;
                            case "lessorequal":
                                And(x => x.AdjustWater <= val);
                                break;
                            case "greater":
                                And(x => x.AdjustWater > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.AdjustWater >= val);
                                break;
                            default:
                                And(x => x.AdjustWater == val);
                                break;
                        }
						}
						if (rule.field == "AdjustWaterAmount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.AdjustWaterAmount == val);
                                break;
                            case "notequal":
                                And(x => x.AdjustWaterAmount != val);
                                break;
                            case "less":
                                And(x => x.AdjustWaterAmount < val);
                                break;
                            case "lessorequal":
                                And(x => x.AdjustWaterAmount <= val);
                                break;
                            case "greater":
                                And(x => x.AdjustWaterAmount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.AdjustWaterAmount >= val);
                                break;
                            default:
                                And(x => x.AdjustWaterAmount == val);
                                break;
                        }
						}
						if (rule.field == "AdjustServiceAmount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.AdjustServiceAmount == val);
                                break;
                            case "notequal":
                                And(x => x.AdjustServiceAmount != val);
                                break;
                            case "less":
                                And(x => x.AdjustServiceAmount < val);
                                break;
                            case "lessorequal":
                                And(x => x.AdjustServiceAmount <= val);
                                break;
                            case "greater":
                                And(x => x.AdjustServiceAmount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.AdjustServiceAmount >= val);
                                break;
                            default:
                                And(x => x.AdjustServiceAmount == val);
                                break;
                        }
						}
						if (rule.field == "TotalAmount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.TotalAmount == val);
                                break;
                            case "notequal":
                                And(x => x.TotalAmount != val);
                                break;
                            case "less":
                                And(x => x.TotalAmount < val);
                                break;
                            case "lessorequal":
                                And(x => x.TotalAmount <= val);
                                break;
                            case "greater":
                                And(x => x.TotalAmount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.TotalAmount >= val);
                                break;
                            default:
                                And(x => x.TotalAmount == val);
                                break;
                        }
						}
						if (rule.field == "TotalReceivableAmount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.TotalReceivableAmount == val);
                                break;
                            case "notequal":
                                And(x => x.TotalReceivableAmount != val);
                                break;
                            case "less":
                                And(x => x.TotalReceivableAmount < val);
                                break;
                            case "lessorequal":
                                And(x => x.TotalReceivableAmount <= val);
                                break;
                            case "greater":
                                And(x => x.TotalReceivableAmount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.TotalReceivableAmount >= val);
                                break;
                            default:
                                And(x => x.TotalReceivableAmount == val);
                                break;
                        }
						}
						if (rule.field == "Status"  && !string.IsNullOrEmpty(rule.value))
						{
                           if (rule.op == "equal")
                           {
                             And(x => x.Status == rule.value);
                           } 
                           else
                           {
							And(x => x.Status.Contains(rule.value));
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
