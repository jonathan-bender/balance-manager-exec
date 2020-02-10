using System.Web.Http;
using BalanceManager.DAL;
using BalanceManager.Services;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace BalanceManager
{
    public static class UnityConfig
    {
        public static void Register(HttpConfiguration httpConfiguration)
        {
            var container = new UnityContainer();

            container.RegisterType<BalanceService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IBalancesContext, BalancesContext>(new ContainerControlledLifetimeManager());

            httpConfiguration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}