using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using WebApp.Models;

namespace WebApp.SmartHubs
{
  [HubName("chatHub")]
  [Authorize]
  public class ChatHub : Hub
  {
    private static readonly List<Users> ConnectedUsers = new List<Users>();
    private static readonly List<Messages> CurrentMessage = new List<Messages>();
    //ConnClass ConnC = new ConnClass();
    public ChatHub()
    {

    }

    public async Task Connect()
    {
      var connectionId = this.Context.ConnectionId;
      var user = (ClaimsIdentity)this.Context.User.Identity;
      var userid = user.GetUserId();
      var userName = user.Name;
      var avatar = user.Claims.FirstOrDefault(x => x.Type == "AvatarsX50")?.Value;
      var fullname= user.Claims.FirstOrDefault(x => x.Type == "FullName")?.Value;
      var tenantid= user.Claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
      var connectuser = ConnectedUsers.FirstOrDefault(x => x.UserName == userName);
      var role=user.Claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
      role = string.IsNullOrEmpty(role) ? "Users" : role;
      fullname= string.IsNullOrEmpty(fullname) ? userName : fullname;
      if (connectuser==null)
      {
        var UserImg = this.GetUserImage(avatar);
        var logintime = DateTime.Now.ToString();

        ConnectedUsers.Add(new Users
        {
          ConnectionId = connectionId,
          FullName = fullname,
          UserName = userName,
          UserImage = UserImg,
          LoginTime = logintime,
          ConnectionIds=new HashSet<string>() { connectionId }
        });
        // send to caller
        this.Clients.Caller.onConnected(connectionId, userName, fullname, ConnectedUsers, CurrentMessage);

        // send to all except caller client
        this.Clients.AllExcept(connectionId).onNewUserConnected(connectionId, userName, fullname, UserImg, logintime);
      }
      else
      {
        //this.Clients.Client(connectuser.ConnectionId).serverOrderedDisconnect();
        //this.Clients.All.onUserDisconnected(connectuser.ConnectionId, connectuser.UserName);
        this.Clients.AllExcept(connectuser.ConnectionIds.ToArray()).onUserDisconnected(connectuser.ConnectionId, connectuser.UserName);
        lock (connectuser.ConnectionIds)
        {
          connectuser.ConnectionIds.Add(connectionId);
          connectuser.ConnectionId = connectionId;
        }
    
        //connectedUsers.Remove(connectuser);
        var UserImg = this.GetUserImage(avatar);
        var logintime = DateTime.Now.ToString();

        //connectedUsers.Add(new Users { ConnectionId = connectionId, UserName = userName, FullName = fullname, UserImage = UserImg, LoginTime = logintime });
        // send to caller
        this.Clients.Client(connectionId).onConnected(connectionId, userName, fullname, ConnectedUsers, CurrentMessage);

        // send to all except caller client
        //this.Clients.AllExcept(connectionId).onNewUserConnected(connectionId, userName, fullname, UserImg, logintime);
      }
      await Auth.SetOnlineAsync(userid);
    }

    public void SendMessageToAll(string userName, string fullName, string message, string time,string avatar)
    {
      var UserImg = avatar;
      // store last 100 messages in cache
      this.AddMessageinCache(userName, fullName, message, time, UserImg);

      // Broad cast message
      this.Clients.All.messageReceived(userName, fullName, message, time, UserImg);

    }

    private void AddMessageinCache(string userName,string fullName, string message, string time, string UserImg)
    {
      CurrentMessage.Add(new Messages { UserName = userName,FullName= fullName, Message = message, Time = time, UserImage = UserImg });

      if (CurrentMessage.Count > 100)
      {
        CurrentMessage.RemoveAt(0);
      }

    }

    // Clear Chat History
    public void ClearTimeout() => CurrentMessage.Clear();

    public string GetUserImage(string img)
    {
      var RetimgName = "/content/img/avatars/male.png";
      try
      {
        //string query = "select Photo from tbl_Users where UserName='" + username + "'";
        var ImageName = $"/content/img/avatars/{img}.png";

        if (ImageName != "")
        {
          RetimgName = ImageName;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      return RetimgName;
    }

    public override async Task OnDisconnected(bool stopCalled)
    {
      var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == this.Context.ConnectionId);
      if (item != null)
      {
        ConnectedUsers.Remove(item);

        var id = this.Context.ConnectionId;
        this.Clients.All.onUserDisconnected(id, item.UserName);

      }
      await Auth.SetOfflineAsync(this.Context.User.Identity.GetUserId());
      await  base.OnDisconnected(stopCalled);
    }

    public void SendPrivateMessage(string toUserId, string message)
    {

      var fromUserId = this.Context.ConnectionId;

      var toUser = ConnectedUsers.FirstOrDefault(x =>x.ConnectionIds.Contains(toUserId));
      var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionIds.Contains(fromUserId));

      if (toUser != null && fromUser != null)
      {
        var CurrentDateTime = DateTime.Now.ToString();
        var UserImg = fromUser.UserImage;
        // send to
        foreach(var toid in toUser.ConnectionIds)
        {
          this.Clients.Client(toid).sendPrivateMessage(fromUserId, fromUser.FullName, toUserId, toUser.FullName, message, UserImg, CurrentDateTime);
        }
        

        // send to caller user
        this.Clients.Caller.sendPrivateMessage(fromUserId, fromUser.FullName, toUserId, toUser.FullName, message, UserImg, CurrentDateTime);
      }

    }
  }


  public class Users
  {
    public string ConnectionId { get; set; }
    public string UserName { get; set; }
    public string UserImage { get; set; }
    public string LoginTime { get; set; }
    public string FullName { get; set; }

    public HashSet<string> ConnectionIds { get; set; }
  }

  public class Messages
  {

    public string UserName { get; set; }
    public string FullName { get; set; }

    public string Message { get; set; }

    public string Time { get; set; }

    public string UserImage { get; set; }

  }
}