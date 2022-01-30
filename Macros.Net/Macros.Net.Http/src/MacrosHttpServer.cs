using System.Net;
using System.Reflection;
using Macros.Net.Core;
using Macros.Net.Core.Abstraction;
using Macros.Net.Core.Annotations;
using Macros.Net.Core.Reflections;

namespace Macros.Net.Http;

public sealed class MacrosHttpServer : IMacrosServer
{
    private readonly HttpListener httpListener;
    private readonly IEnumerable<Lazy<ControllerContext>> controllers;
    public MacrosHttpServer()
    {
        httpListener = new HttpListener();
        controllers = Assembly.GetExecutingAssembly().GetControllers();
        createRoutes();
    }
    public async ValueTask StartAsync(CancellationToken cancellationToken)
    {
        httpListener.Start();
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                HttpListenerContext httpListenerContext = await httpListener.GetContextAsync();
                MacrosHttpBackgroundWorker.Submit(new MacrosHttpHandler(httpListenerContext));
            }
            catch (Exception ex)
            {

            }
        }
        httpListener.Stop();
    }
    private void createRoutes()
    {
        MacrosHttpRouteRepository macrosHttpRouteRepository = MacrosHttpRouteRepository.Current.Value;
        foreach (var controller in controllers)
        {
            foreach (var action in controller.Value.Actions)
            {
                macrosHttpRouteRepository.AddRoute(new MacrosHttpRoute(action.Value));
            }
        }
    }
}