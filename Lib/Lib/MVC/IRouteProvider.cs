
using Microsoft.AspNetCore.Routing;

namespace Lib.Mvc
{
    public interface IRouteProvider
    {
        void RegisterRoutes(RouteCollection routes);

        int Priority { get; }
    }
}
