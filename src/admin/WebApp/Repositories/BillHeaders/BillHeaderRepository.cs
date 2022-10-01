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
/// File: BillHeaderRepository.cs
/// Purpose: The repository and unit of work patterns are intended
/// to create an abstraction layer between the data access layer and
/// the business logic layer of an application.
/// Created Date: 2/19/2021 8:29:48 PM
/// Author: neo.zhu
/// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
/// Copyright (c) 2012-2018 All Rights Reserved
/// </summary>
  public static class BillHeaderRepository  
    {
                 public static async Task<IEnumerable<BillHeader>> GetByCustomerId(this IRepositoryAsync<BillHeader> repository, int customerid)
          => await repository
                .Queryable()
                .Where(x => x.CustomerId==customerid).ToListAsync();
              
          
                        public static async Task<IEnumerable<BillDetail>>   GetBillDetailsByBillId (this IRepositoryAsync<BillHeader> repository,int billid)
          => await  repository.GetRepositoryAsync<BillDetail>()
                    .Queryable()
                    .Include(x => x.BillHeader)
                    .Where(n => n.BillId == billid)
                    .ToListAsync();
        
         
	}
}



