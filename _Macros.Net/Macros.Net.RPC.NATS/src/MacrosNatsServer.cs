using System.Reflection;
using Macros.Net.RPC.Core;
using Macros.Net.RPC.Core.Abstraction;
using Macros.Net.RPC.Core.Reflections;
using Macros.Serialization.Core.Abstraction;
using NATS.Client;


namespace Macros.Net.RPC.NATS;

public sealed class MacrosNatsServer : IMacrosServer
{
    internal static string DefaultSerializerId { get; }
    private readonly IConnection connection;
    private readonly List<IAsyncSubscription> subscriptions;
    static MacrosNatsServer()
    {
        DefaultSerializerId = Guid.NewGuid().ToString();
    }
    public MacrosNatsServer(IMacrosSerializer defaultMacrosSerializer)
    {
        Inject.Register.AddSingleton(defaultMacrosSerializer, DefaultSerializerId);
        Inject.Register.AutoRegister();
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
    public ValueTask StartAsync(CancellationToken cancellationToken)
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
        return ValueTask.CompletedTask;
    }
}