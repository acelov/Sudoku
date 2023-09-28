using System.Text;

namespace Sudoku.Concepts.Parsers;

/// <summary>
/// Represents an Excel grid parser.
/// </summary>
public sealed partial record ExcelGridParser : GridParser
{
	/// <inheritdoc/>
	public override Func<string, Grid> Parser
		=> static str =>
		{
			if (!str.Contains('\t'))
			{
				return Grid.Undefined;
			}

			if (str.SplitBy(['\r', '\n']) is not { Length: 9 } values)
			{
				return Grid.Undefined;
			}

			scoped var sb = new StringHandler(81);
			foreach (var value in values)
			{
				foreach (var digitString in value.Split(['\t']))
				{
					sb.Append(string.IsNullOrEmpty(digitString) ? '.' : digitString[0]);
				}
			}

			return Grid.Parse(sb.ToStringAndClear());
		};
}
