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
/// File: OrgChartRepository.cs
/// Purpose: The repository and unit of work patterns are intended
/// to create an abstraction layer between the data access layer and
/// the business logic layer of an application.
/// Created Date: 2020/11/3 19:39:48
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
  public static class OrgChartRepository  
    {
                 public static async Task<IEnumerable<OrgChart>> GetByParentId(this IRepositoryAsync<OrgChart> repository, int parentid)
          => await repository
                .Queryable()
                .Where(x => x.ParentId==parentid).ToListAsync();
              
          
                 
	}
}



