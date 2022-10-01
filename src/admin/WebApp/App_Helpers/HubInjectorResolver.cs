using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Lifetime;
using WebApp.Models;
using WebApp.SmartHubs;

namespace WebApp
{
  public class UnityHubActivator : IHubActivator
  {
    private readonly IUnityContainer _container;

    public UnityHubActivator(IUnityContainer container)
    {
      _container = container;
    }

    public IHub Create(HubDescriptor descriptor)
    {
      return (IHub)_container.Resolve(descriptor.HubType);
    }
  }

  public class HubDependencyResolver : DefaultDependencyResolver
  {
    protected IUnityContainer container;
    private bool isDisposed = false;

    public HubDependencyResolver(IUnityContainer container)
    {
      if (container == null)
      {
        throw new ArgumentNullException("container");
      }

      this.container = container.CreateChildContainer();
    }

    /// <summary>
    /// Gets the Autofac implementation of the dependency resolver.
    /// </summary>
    public static HubDependencyResolver Current
    {
      get { return GlobalHost.DependencyResolver as HubDependencyResolver; }
    }


    public override object GetService(Type serviceType)
    {
      if (container.IsRegistered(serviceType))
      {
        return container.Resolve(serviceType);
      }

      return base.GetService(serviceType);
    }

    public override IEnumerable<object> GetServices(Type serviceType)
    {
      if (container.IsRegistered(serviceType))
      {
        return container.ResolveAll(serviceType);
      }

      return base.GetServices(serviceType);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);

      if (isDisposed)
      {
        return;
      }

      if (disposing)
      {
        container.Dispose();
      }

      isDisposed = true;
    }
  }

  public static class HubUnityConfig
  {
    private static readonly Lazy<IUnityContainer> container =
      new Lazy<IUnityContainer>(() =>
      {
        var container = new UnityContainer();
        RegisterTypes(container);
        return container;
      });
    public static IUnityContainer Container => container.Value;
    public static void RegisterTypes(IUnityContainer container)
    {
      container.RegisterType<StoreContext>(new HierarchicalLifetimeManager());
      container.RegisterType<ChatHub>(new HierarchicalLifetimeManager());
      container.RegisterType<NotificationHub>(new HierarchicalLifetimeManager());
    }
  }

}