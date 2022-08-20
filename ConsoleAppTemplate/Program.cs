using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);

hostBuilder.ConfigureAppConfiguration(configBuilder =>
{
    // Динамически закидываем в конфиг новую настройку (просто так, для примера).
    // Одна из команд позднее будет её читать.
    configBuilder.AddInMemoryCollection(new[] { new KeyValuePair<string, string>("Count", "4") });
});

hostBuilder.ConfigureServices((context, services) =>
{
    // Регистрируем наше приложение, чтобы можно было ему при создании подтянуть инфу из DI.
    services.AddSingleton<IMyApp, MyApp>();
    // Команды и запросы могут быть определены в одной сборке, а их обработчики - в другой.
    // Поэтому у нас в конфиге две настройки, у которых в качестве значений - имена сборок,
    // разделённые символом ';'.
    var commandAssemblyNames = context.Configuration.GetValue<string>("MediatR:CommandAssemblies").Split(';');
    var handlerAssemblyNames = context.Configuration.GetValue<string>("MediatR:HandlerAssemblies").Split(';');
    var names = commandAssemblyNames.Union(handlerAssemblyNames).Distinct();

    var assemblies = names.Select(n => Assembly.Load(n)).ToArray();
    // Указываем конкретные сборки, в которых медиатору следует искать команды, запросы и их обработчики
    services.AddMediatR(assemblies);
});

using (IHost host = hostBuilder.Build())
{
    using (var scope = host.Services.CreateScope())
    {
        // Запускаем нашу программку...
        var app = ActivatorUtilities.GetServiceOrCreateInstance<IMyApp>(scope.ServiceProvider);
        await app.Run();
    }
}
