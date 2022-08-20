using MediatR;
using MyCommands;
namespace MyCommandHandlers;

/// <summary>
/// Обработчик команды SendMessage. Полученное сообщение отправляет в консоль,
/// дополнив его своим префиксом. )))
/// </summary>
public sealed class SendMessageHandler : IRequestHandler<SendMessage, Unit>
{
    public async Task<Unit> Handle(SendMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Mediator -> {request.Message}");
        return await Task.FromResult(Unit.Value);
    }
}
