using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

namespace WebApp
{
  public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
  {
    private readonly string _publicClientId;

    public ApplicationOAuthProvider(string publicClientId)
    {
      if (publicClientId == null)
      {
        throw new ArgumentNullException("publicClientId");
      }

      this._publicClientId = publicClientId;
    }

    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {
      context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
      var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
      var user = await userManager.FindAsync(context.UserName, context.Password);
      
   
      if (user == null)
      {
        context.SetError("invalid_grant", "The user name or password is incorrect.");
        context.Rejected();
        return;
      }
   

      var oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
         OAuthDefaults.AuthenticationType);
   
       context.Validated(oAuthIdentity);
     }

    public override Task TokenEndpoint(OAuthTokenEndpointContext context)
    {
      foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
      {
        context.AdditionalResponseParameters.Add(property.Key, property.Value);
      }

      return Task.FromResult<object>(null);
     
    }

    public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {
      // Resource owner password credentials does not provide a client ID.
      if (context.ClientId == null)
      {
        context.Validated();
      }

      return Task.FromResult<object>(null);
    }

    public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
    {
      if (context.ClientId == this._publicClientId)
      {
        var expectedRootUri = new Uri(context.Request.Uri, "/");

        if (expectedRootUri.AbsoluteUri == context.RedirectUri)
        {
          context.Validated();
        }
      }

      return Task.FromResult<object>(null);
    }

    public static AuthenticationProperties CreateProperties(string userName,int tenantId)
    {
      IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "tenantId", tenantId.ToString() }
            };
      return new AuthenticationProperties(data);
    }
  }
}