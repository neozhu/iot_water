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
/// File: CustomerBillQuery.cs
/// Purpose: easyui datagrid filter query 
/// Created Date: 3/28/2020 3:02:35 PM
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
   public class CustomerBillQuery:QueryObject<CustomerBill>
   {
		public CustomerBillQuery Withfilter(IEnumerable<filterRule> filters)
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
						if (rule.field == "Year" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Year == val);
                                break;
                            case "notequal":
                                And(x => x.Year != val);
                                break;
                            case "less":
                                And(x => x.Year < val);
                                break;
                            case "lessorequal":
                                And(x => x.Year <= val);
                                break;
                            case "greater":
                                And(x => x.Year > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Year >= val);
                                break;
                            default:
                                And(x => x.Year == val);
                                break;
                        }
						}
						if (rule.field == "Month" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Month == val);
                                break;
                            case "notequal":
                                And(x => x.Month != val);
                                break;
                            case "less":
                                And(x => x.Month < val);
                                break;
                            case "lessorequal":
                                And(x => x.Month <= val);
                                break;
                            case "greater":
                                And(x => x.Month > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Month >= val);
                                break;
                            default:
                                And(x => x.Month == val);
                                break;
                        }
						}
						if (rule.field == "Status"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Status.Contains(rule.value));
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
						if (rule.field == "Price" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Price == val);
                                break;
                            case "notequal":
                                And(x => x.Price != val);
                                break;
                            case "less":
                                And(x => x.Price < val);
                                break;
                            case "lessorequal":
                                And(x => x.Price <= val);
                                break;
                            case "greater":
                                And(x => x.Price > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Price >= val);
                                break;
                            default:
                                And(x => x.Price == val);
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
						if (rule.field == "Amount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Amount == val);
                                break;
                            case "notequal":
                                And(x => x.Amount != val);
                                break;
                            case "less":
                                And(x => x.Amount < val);
                                break;
                            case "lessorequal":
                                And(x => x.Amount <= val);
                                break;
                            case "greater":
                                And(x => x.Amount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Amount >= val);
                                break;
                            default:
                                And(x => x.Amount == val);
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
						if (rule.field == "Remark"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Remark.Contains(rule.value));
						}
						if (rule.field == "CustomerName"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.CustomerName.Contains(rule.value));
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
         public  CustomerBillQuery ByCustomerIdWithfilter(int customerid, IEnumerable<filterRule> filters)
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
						if (rule.field == "Year" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Year == val);
                                break;
                            case "notequal":
                                And(x => x.Year != val);
                                break;
                            case "less":
                                And(x => x.Year < val);
                                break;
                            case "lessorequal":
                                And(x => x.Year <= val);
                                break;
                            case "greater":
                                And(x => x.Year > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Year >= val);
                                break;
                            default:
                                And(x => x.Year == val);
                                break;
                        }
						}
						if (rule.field == "Month" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Month == val);
                                break;
                            case "notequal":
                                And(x => x.Month != val);
                                break;
                            case "less":
                                And(x => x.Month < val);
                                break;
                            case "lessorequal":
                                And(x => x.Month <= val);
                                break;
                            case "greater":
                                And(x => x.Month > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Month >= val);
                                break;
                            default:
                                And(x => x.Month == val);
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
						if (rule.field == "Price" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Price == val);
                                break;
                            case "notequal":
                                And(x => x.Price != val);
                                break;
                            case "less":
                                And(x => x.Price < val);
                                break;
                            case "lessorequal":
                                And(x => x.Price <= val);
                                break;
                            case "greater":
                                And(x => x.Price > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Price >= val);
                                break;
                            default:
                                And(x => x.Price == val);
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
						if (rule.field == "Amount" && !string.IsNullOrEmpty(rule.value) && rule.value.IsDecimal())
						{
							var val = Convert.ToDecimal(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Amount == val);
                                break;
                            case "notequal":
                                And(x => x.Amount != val);
                                break;
                            case "less":
                                And(x => x.Amount < val);
                                break;
                            case "lessorequal":
                                And(x => x.Amount <= val);
                                break;
                            case "greater":
                                And(x => x.Amount > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Amount >= val);
                                break;
                            default:
                                And(x => x.Amount == val);
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
               }
            }
            return this;
         }    
    }
}
