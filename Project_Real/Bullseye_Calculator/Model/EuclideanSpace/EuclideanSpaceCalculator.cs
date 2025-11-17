using System.Text.RegularExpressions;

namespace Bullseye_Calculator.Model.EuclideanSpace;

public partial class EuclideanSpaceCalculator : Calculator
{
	[GeneratedRegex(@"^\[.*\]$")]
	protected static partial Regex BracketedRegex();

	protected static readonly FunctionToken[] functionTokens =
	[
		/*new("ceiling", () => new Ceiling()),
		new("round", () => new Round()),
		new("floor", () => new Floor()),
		new("fact", () => new Fact()),
		new("abs", () => new Abs()),
		new("pi", () => new PI()),
		new("e", () => new E()),*/
	];

	protected readonly Standard.StandardCalculator standardCalculator;

	public EuclideanSpaceCalculator() : base(
	[
		/*// Rational number
		new(null!, value => new Number(value)),
		// Function name
		new(FunctionNameRegex(), name => GetFunctionByName(functionTokens, name)),
		// Operators
		new(AddRegex(), _ => new Add()),
		new(SubtractRegex(), _ => new Subtract()),
		new(MultiplyRegex(), _ => new Multiply()),
		new(DivideRegex(), _ => new Divide()),
		new(PowerRegex(), _ => new Power()),
		new(RootRegex(), _ => new Root()),
		// Separators
		new(OpeningParenthesisRegex(), _ => new OpeningParenthesis()),
		new(ClosingParenthesisRegex(), _ => new ClosingParenthesis()),
		new(ComaRegex(), _ => new Coma()),*/
		// Matrix
		new(BracketedRegex(), match => MakeMatrix(match[1..^2])),
	]) 
	{
		standardCalculator = new Standard.StandardCalculator();
	}

	private static MatrixHolder MakeMatrix(string content)
	{
		string[] rows = content.Split('&', StringSplitOptions.TrimEntries);
        string[][] tokenized = [.. rows.Select(row => row.Split(';', StringSplitOptions.TrimEntries))];

		Standard.ValueHolder[,] matrix = new Standard.ValueHolder[tokenized.Length, tokenized[0].Length];

		try
		{
			for (int row = tokenized.Length - 1; row >= 0; --row)
			{
				for (int col = tokenized[row].Length - 1; col >= 0; --col)
					matrix[row, col] = Evaluate(tokenized[row][col], new Standard.StandardCalculator());
			}
		}
		catch (IndexOutOfRangeException)
		{
			throw new FormatException();
		}

        return new MatrixHolder(matrix);
    }
}