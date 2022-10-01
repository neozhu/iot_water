
     
 
 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Repository.Pattern.Repositories;
using Service.Pattern;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Services
{
    public interface IRoleMenuService:IService<RoleMenu>
    {

                  Task<IEnumerable<RoleMenu>> GetByMenuIdAsync(int  menuid);

    Task<IEnumerable<RoleMenu>> GetByRoleNameAsync(string roleName);
                  Task AuthorizeAsync(RoleMenusView[] list);

                  IEnumerable<MenuItem> RenderMenus(string[] roleNames);
      
 
	}
}