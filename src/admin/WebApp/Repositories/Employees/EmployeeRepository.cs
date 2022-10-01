using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using System.Threading.Tasks;
using WebApp.Models;
namespace WebApp.Repositories
{
/// <summary>
/// File: EmployeeRepository.cs
/// Purpose: The repository and unit of work patterns are intended
/// to create an abstraction layer between the data access layer and
/// the business logic layer of an application.
/// Created Date: 12/26/2019 9:30:38 AM
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
  public static class EmployeeRepository  
    {
                 public static async Task<IEnumerable<Employee>> GetByCompanyIdAsync(this IRepositoryAsync<Employee> repository, int companyid)
          => await repository
                .Queryable()
                .Where(x => x.CompanyId==companyid).ToListAsync();
              
          
                 public static async Task<IEnumerable<Employee>> GetByDepartmentIdAsync(this IRepositoryAsync<Employee> repository, int departmentid)
          => await repository
                .Queryable()
                .Where(x => x.DepartmentId==departmentid).ToListAsync();
              
          
                 
	}
}



