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

    // Команды и запросы могут быть определены в одних сборках, а их обработчики - в других.
    // По этой причине у нас в конфиге мы создали две настройки: одна содержит
    // массив имён сборок, в которых определены CQRS команды и запросы, а другая - имена сборок,
    // содержащих обработчики этих команд и запросов.
    var commandAssemblyNames = context.Configuration.GetSection("MediatR:CommandAssemblies").Get<string[]>();
    var handlerAssemblyNames = context.Configuration.GetSection("MediatR:HandlerAssemblies").Get<string[]>();
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
