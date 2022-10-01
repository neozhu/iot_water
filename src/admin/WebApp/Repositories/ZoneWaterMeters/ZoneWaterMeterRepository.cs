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
/// File: ZoneWaterMeterRepository.cs
/// Purpose: The repository and unit of work patterns are intended
/// to create an abstraction layer between the data access layer and
/// the business logic layer of an application.
/// Created Date: 3/28/2020 6:45:06 PM
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
  public static class ZoneWaterMeterRepository  
    {
                 public static async Task<IEnumerable<ZoneWaterMeter>> GetByZoneIdAsync(this IRepositoryAsync<ZoneWaterMeter> repository, int zoneid)
          => await repository
                .Queryable()
                .Where(x => x.ZoneId==zoneid).ToListAsync();
              
          
                 
	}
}



