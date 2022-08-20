using MediatR;
namespace MyCommands;

/// <summary>
/// Отправить сообщение в параллельное измерение. )))
/// </summary>
/// <param name="Message">Текст сообщения.</param>
public record SendMessage(string Message): IRequest<Unit>;