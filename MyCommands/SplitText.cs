using MediatR;
namespace MyCommands;

/// <summary>
/// Разбить строку на части, используя ';' в качестве разделителя. )))
/// </summary>
/// <param name="Text">Исходная строка.</param>
public record SplitText(string Text): IRequest<IEnumerable<string>>;