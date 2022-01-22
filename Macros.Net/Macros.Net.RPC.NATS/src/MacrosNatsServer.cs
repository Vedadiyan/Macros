using System.Reflection;
using Macros.Net.RPC.Core;
using Macros.Net.RPC.Core.Abstraction;
using Macros.Net.RPC.Core.Reflections;
using NATS.Client;


namespace Macros.Net.RPC.NATS;

public sealed class MacrosNatsServer : IMacrosServer
{
    private readonly IConnection connection;
    private readonly List<IAsyncSubscription> subscriptions;
    public MacrosNatsServer()
    {
        connection = new ConnectionFactory().CreateConnection();
        subscriptions = new List<IAsyncSubscription>();
        IEnumerable<RpcControllerContext> controllers = Assembly.GetCallingAssembly().GetControllers();
        foreach (var controller in controllers)
        {
            subscriptions.Add(
                connection.SubscribeAsync(
                    controller.Namespace,
                    (sender, e) =>
                        controller.HandleRequest(
                            new MacrosNatsTransport(
                                controller.Namespace,
                                controller.Namespace.Split('.').Length,
                                e.Message)
                            )
                        )
                    );
        }
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        cancellationToken.Register(() =>
        {
            foreach (var subscription in subscriptions)
            {
                subscription.Unsubscribe();
            }
        });
        foreach (var subscription in subscriptions)
        {
            subscription.Start();
        }
        return Task.CompletedTask;
    }
}