using MediatR;
using MyCommands;
namespace MyCommandHandlers;

/// <summary>
/// Обработчик команды SplitText. Собственно, разбивает строку на части. )))
/// </summary>
public sealed class SplitTextHandler : IRequestHandler<SplitText, IEnumerable<string>>
{
    public async Task<IEnumerable<string>> Handle(SplitText request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(request.Text.Split(';'));
    }
}