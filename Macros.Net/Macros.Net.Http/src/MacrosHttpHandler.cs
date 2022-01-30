using System.Net;

namespace Macros.Net.Http;

public class MacrosHttpHandler
{
    private readonly HttpListenerContext httpListenerContext;
    public MacrosHttpHandler(HttpListenerContext httpListenerContext)
    {
        this.httpListenerContext = httpListenerContext;
    }
    public Task HandleAsync()
    {
        MacrosHttpRoute macrosHttpRoute = MacrosHttpRouter.FindBestRoute(httpListenerContext.Request);
        if(macrosHttpRoute != default) {
            return macrosHttpRoute.ActionContext.ControllerContext.HandleRequestAsync(default!);
        }
        return Task.CompletedTask;
    }
}