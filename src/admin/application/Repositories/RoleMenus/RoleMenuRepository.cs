                    
      
    
 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Repository.Pattern.Repositories;

using WebApp.Models;

namespace WebApp.Repositories
{
  public static class RoleMenuRepository
    {

    public static async Task<IEnumerable<RoleMenu>> GetByMenuIdAsync(this IRepositoryAsync<RoleMenu> repository, int menuid) =>await repository
           .Queryable()
           .Where(x => x.MenuId == menuid).ToListAsync();



  }
}



