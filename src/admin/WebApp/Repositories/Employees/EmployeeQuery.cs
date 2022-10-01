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
/// File: EmployeeQuery.cs
/// Purpose: easyui datagrid filter query 
/// Created Date: 12/26/2019 9:30:41 AM
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
   public class EmployeeQuery:QueryObject<Employee>
   {
		public EmployeeQuery Withfilter(IEnumerable<filterRule> filters)
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
						if (rule.field == "Name"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Name.Contains(rule.value));
						}
						if (rule.field == "Title"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Title.Contains(rule.value));
						}
						if (rule.field == "PhoneNumber"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.PhoneNumber.Contains(rule.value));
						}
						if (rule.field == "WX"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.WX.Contains(rule.value));
						}
						if (rule.field == "Sex"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Sex.Contains(rule.value));
						}
						if (rule.field == "Age" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Age == val);
                                break;
                            case "notequal":
                                And(x => x.Age != val);
                                break;
                            case "less":
                                And(x => x.Age < val);
                                break;
                            case "lessorequal":
                                And(x => x.Age <= val);
                                break;
                            case "greater":
                                And(x => x.Age > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Age >= val);
                                break;
                            default:
                                And(x => x.Age == val);
                                break;
                        }
						}
						if (rule.field == "Brithday" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.Brithday) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.Brithday) <= 0);
						    }
						}
						if (rule.field == "EntryDate" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.EntryDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.EntryDate) <= 0);
						    }
						}
						if (rule.field == "IsDeleted" && !string.IsNullOrEmpty(rule.value) && rule.value.IsBool())
						{	
							 var boolval=Convert.ToBoolean(rule.value);
							 And(x => x.IsDeleted == boolval);
						}
						if (rule.field == "LeaveDate" && !string.IsNullOrEmpty(rule.value) )
						{	
							if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.LeaveDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.LeaveDate) <= 0);
						    }
						}
						if (rule.field == "CompanyId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.CompanyId == val);
                                break;
                            case "notequal":
                                And(x => x.CompanyId != val);
                                break;
                            case "less":
                                And(x => x.CompanyId < val);
                                break;
                            case "lessorequal":
                                And(x => x.CompanyId <= val);
                                break;
                            case "greater":
                                And(x => x.CompanyId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.CompanyId >= val);
                                break;
                            default:
                                And(x => x.CompanyId == val);
                                break;
                        }
						}
						if (rule.field == "DepartmentId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.DepartmentId == val);
                                break;
                            case "notequal":
                                And(x => x.DepartmentId != val);
                                break;
                            case "less":
                                And(x => x.DepartmentId < val);
                                break;
                            case "lessorequal":
                                And(x => x.DepartmentId <= val);
                                break;
                            case "greater":
                                And(x => x.DepartmentId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.DepartmentId >= val);
                                break;
                            default:
                                And(x => x.DepartmentId == val);
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
         public  EmployeeQuery ByCompanyIdWithfilter(int companyid, IEnumerable<filterRule> filters)
         {
            And(x => x.CompanyId == companyid);
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
						if (rule.field == "Name"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Name.Contains(rule.value));
						}
						if (rule.field == "Title"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Title.Contains(rule.value));
						}
						if (rule.field == "PhoneNumber"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.PhoneNumber.Contains(rule.value));
						}
						if (rule.field == "WX"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.WX.Contains(rule.value));
						}
						if (rule.field == "Sex"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Sex.Contains(rule.value));
						}
						if (rule.field == "Age" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Age == val);
                                break;
                            case "notequal":
                                And(x => x.Age != val);
                                break;
                            case "less":
                                And(x => x.Age < val);
                                break;
                            case "lessorequal":
                                And(x => x.Age <= val);
                                break;
                            case "greater":
                                And(x => x.Age > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Age >= val);
                                break;
                            default:
                                And(x => x.Age == val);
                                break;
                        }
						}
						if (rule.field == "Brithday" && !string.IsNullOrEmpty(rule.value) )
						{	
                            if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.Brithday) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.Brithday) <= 0);
						    }
                        }
						if (rule.field == "EntryDate" && !string.IsNullOrEmpty(rule.value) )
						{	
                            if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.EntryDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.EntryDate) <= 0);
						    }
                        }
						if (rule.field == "IsDeleted" && !string.IsNullOrEmpty(rule.value) && rule.value.IsBool())
						{	
							 var boolval=Convert.ToBoolean(rule.value);
							 And(x => x.IsDeleted == boolval);
						}
						if (rule.field == "LeaveDate" && !string.IsNullOrEmpty(rule.value) )
						{	
                            if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.LeaveDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.LeaveDate) <= 0);
						    }
                        }
						if (rule.field == "CompanyId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.CompanyId == val);
                                break;
                            case "notequal":
                                And(x => x.CompanyId != val);
                                break;
                            case "less":
                                And(x => x.CompanyId < val);
                                break;
                            case "lessorequal":
                                And(x => x.CompanyId <= val);
                                break;
                            case "greater":
                                And(x => x.CompanyId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.CompanyId >= val);
                                break;
                            default:
                                And(x => x.CompanyId == val);
                                break;
                        }
						}
						if (rule.field == "DepartmentId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.DepartmentId == val);
                                break;
                            case "notequal":
                                And(x => x.DepartmentId != val);
                                break;
                            case "less":
                                And(x => x.DepartmentId < val);
                                break;
                            case "lessorequal":
                                And(x => x.DepartmentId <= val);
                                break;
                            case "greater":
                                And(x => x.DepartmentId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.DepartmentId >= val);
                                break;
                            default:
                                And(x => x.DepartmentId == val);
                                break;
                        }
						}
               }
            }
            return this;
         }    
         public  EmployeeQuery ByDepartmentIdWithfilter(int departmentid, IEnumerable<filterRule> filters)
         {
            And(x => x.DepartmentId == departmentid);
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
						if (rule.field == "Name"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Name.Contains(rule.value));
						}
						if (rule.field == "Title"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Title.Contains(rule.value));
						}
						if (rule.field == "PhoneNumber"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.PhoneNumber.Contains(rule.value));
						}
						if (rule.field == "WX"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.WX.Contains(rule.value));
						}
						if (rule.field == "Sex"  && !string.IsNullOrEmpty(rule.value))
						{
							And(x => x.Sex.Contains(rule.value));
						}
						if (rule.field == "Age" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.Age == val);
                                break;
                            case "notequal":
                                And(x => x.Age != val);
                                break;
                            case "less":
                                And(x => x.Age < val);
                                break;
                            case "lessorequal":
                                And(x => x.Age <= val);
                                break;
                            case "greater":
                                And(x => x.Age > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.Age >= val);
                                break;
                            default:
                                And(x => x.Age == val);
                                break;
                        }
						}
						if (rule.field == "Brithday" && !string.IsNullOrEmpty(rule.value) )
						{	
                            if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.Brithday) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.Brithday) <= 0);
						    }
                        }
						if (rule.field == "EntryDate" && !string.IsNullOrEmpty(rule.value) )
						{	
                            if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.EntryDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.EntryDate) <= 0);
						    }
                        }
						if (rule.field == "IsDeleted" && !string.IsNullOrEmpty(rule.value) && rule.value.IsBool())
						{	
							 var boolval=Convert.ToBoolean(rule.value);
							 And(x => x.IsDeleted == boolval);
						}
						if (rule.field == "LeaveDate" && !string.IsNullOrEmpty(rule.value) )
						{	
                            if (rule.op == "between")
                            {
                                var datearray = rule.value.Split(new char[] { '-' });
                                var start = Convert.ToDateTime(datearray[0]);
                                var end = Convert.ToDateTime(datearray[1]);
 
							    And(x => SqlFunctions.DateDiff("d", start, x.LeaveDate) >= 0);
                                And(x => SqlFunctions.DateDiff("d", end, x.LeaveDate) <= 0);
						    }
                        }
						if (rule.field == "CompanyId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.CompanyId == val);
                                break;
                            case "notequal":
                                And(x => x.CompanyId != val);
                                break;
                            case "less":
                                And(x => x.CompanyId < val);
                                break;
                            case "lessorequal":
                                And(x => x.CompanyId <= val);
                                break;
                            case "greater":
                                And(x => x.CompanyId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.CompanyId >= val);
                                break;
                            default:
                                And(x => x.CompanyId == val);
                                break;
                        }
						}
						if (rule.field == "DepartmentId" && !string.IsNullOrEmpty(rule.value) && rule.value.IsInt())
						{
							var val = Convert.ToInt32(rule.value);
							switch (rule.op) {
                            case "equal":
                                And(x => x.DepartmentId == val);
                                break;
                            case "notequal":
                                And(x => x.DepartmentId != val);
                                break;
                            case "less":
                                And(x => x.DepartmentId < val);
                                break;
                            case "lessorequal":
                                And(x => x.DepartmentId <= val);
                                break;
                            case "greater":
                                And(x => x.DepartmentId > val);
                                break;
                            case "greaterorequal" :
                                And(x => x.DepartmentId >= val);
                                break;
                            default:
                                And(x => x.DepartmentId == val);
                                break;
                        }
						}
               }
            }
            return this;
         }    
    }
}
