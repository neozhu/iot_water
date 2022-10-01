using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using WebApp.Models;

namespace WebApp.SmartHubs
{
  [System.Web.Mvc.Authorize]
  [HubName("notificationHub")]
  public class NotificationHub : Hub
  {
    private static readonly ConcurrentDictionary<string, UserHubModels> Users =
        new ConcurrentDictionary<string, UserHubModels>(StringComparer.InvariantCultureIgnoreCase);

    private readonly StoreContext context;
    public NotificationHub(StoreContext context)
    {
      this.context = context;
    }
    public async Task Connect() {
      var user = (ClaimsIdentity)this.Context.User.Identity;
      var userName = user.Name;
      var id = this.Context.ConnectionId;
     
      var avatar = user.Claims.FirstOrDefault(x => x.Type == "AvatarsX50")?.Value;
      var fullname = user.Claims.FirstOrDefault(x => x.Type == "FullName")?.Value;
      var tenantid = user.Claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
      var role = user.Claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
      role = string.IsNullOrEmpty(role) ? "Users" : role;
      fullname = string.IsNullOrEmpty(fullname) ? userName : fullname;
      await this.SendNotification(userName);
    }
    //Logged Use Call
    public async Task GetNotification()
    {
      try
      {
        //var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        var me = Context.User.Identity.Name;
        if (Users.TryGetValue(me, out var receiver))
        {
          //Get TotalNotification
          var totalNotif = await LoadNotifData(me);
          //Send To
          foreach (var cid in receiver.ConnectionIds)
          {
            this.Clients.Client(cid).broadcastNotif(totalNotif);
          }
        }
   
      }
      catch (Exception ex)
      {
        ex.ToString();
      }
    }

    //Specific User Call
    public async Task SendNotification(string sentTo)
    {
      try
      {
        //Get TotalNotification
        var totalNotif = await LoadNotifData(sentTo);

        //Send To
        if (Users.TryGetValue(sentTo, out var receiver))
        {
          var cid = receiver.ConnectionIds.FirstOrDefault();
          //var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
          this.Clients.Client(cid).broadcastNotif(totalNotif);
        }
      }
      catch (Exception ex)
      {
        ex.ToString();
      }
    }

    private async Task<int> LoadNotifData(string userId)
    {
      var result =await context.Notifications.Where(t => ( t.To == userId || t.To == "ALL" )
              && t.Read == false).CountAsync();
      return result;
    }
    public  Task AddConnection() {
      var userName = Context.User.Identity.Name;
      var connectionId = Context.ConnectionId;

      var user = Users.GetOrAdd(userName, _ => new UserHubModels
      {
        UserName = userName,
        ConnectionIds = new HashSet<string>()
      });

      lock (user.ConnectionIds)
      {
        user.ConnectionIds.Add(connectionId);
        if (user.ConnectionIds.Count == 1)
        {
          Clients.Others.userConnected(userName);
        }
      }
      return base.OnConnected();
    }
    public override  Task OnConnected()
    {
      var userName = Context.User.Identity.Name;
      var connectionId = Context.ConnectionId;

      var user = Users.GetOrAdd(userName, _ => new UserHubModels
      {
        UserName = userName,
        ConnectionIds = new HashSet<string>()
      });

      lock (user.ConnectionIds)
      {
        user.ConnectionIds.Add(connectionId);
        if (user.ConnectionIds.Count == 1)
        {
          Clients.Others.userConnected(userName);
        }
      }

      return base.OnConnected();
    }

    public override Task OnDisconnected(bool stopCalled)
    {
      var userName = Context.User.Identity.Name;
      var connectionId = Context.ConnectionId;

      Users.TryGetValue(userName, out var user);

      if (user != null)
      {
        lock (user.ConnectionIds)
        {
          user.ConnectionIds.RemoveWhere(cid => cid.Equals(connectionId));
          if (!user.ConnectionIds.Any())
          {
            Users.TryRemove(userName, out var removedUser);
            Clients.Others.userDisconnected(userName);
          }
        }
      }

      return base.OnDisconnected(stopCalled);
    }
  }
  public class UserHubModels
  {
    public string UserName { get; set; }
    public HashSet<string> ConnectionIds { get; set; }
  }

}