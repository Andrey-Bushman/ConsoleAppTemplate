using MediatR;
using Microsoft.Extensions.Configuration;
using MyCommands;

/// <summary>
/// Наша аппликушка.
/// </summary>
sealed class MyApp : IMyApp
{
    private readonly IConfiguration _config;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMediator _mediator;

    public MyApp(IConfiguration config, IServiceProvider serviceProvider, IMediator mediator)
    {
        _config = config;
        _serviceProvider = serviceProvider;
        _mediator = mediator;
    }

    /// <summary>
    /// Наше приложение просто выполняет две команды.
    /// </summary>
    /// <returns></returns>
    public async Task Run()
    {
        // Первая команда
        var count = _config.GetValue<int>("Count");
        var command = new SendMessage($"Cats count: {count}");
        await _mediator.Send(command);

        // Вторая команда
        var command2 = new SplitText("Bob;Jack;Tom;Bill");
        var names = await _mediator.Send(command2);

        // Выводим в консоль результат выполнения второй команды.
        foreach (var name in names)
        {
            Console.WriteLine(name);
        }
    }
}
