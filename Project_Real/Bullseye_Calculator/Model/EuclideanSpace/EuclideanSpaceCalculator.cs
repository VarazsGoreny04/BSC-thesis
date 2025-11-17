using Project_Real;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bullseye_Calculator.Model.EuclideanSpace;

public partial class EuclideanSpaceCalculator : Calculator
{
	[GeneratedRegex(@"^\[.*\]$")]
	protected static partial Regex BracketedRegex();

	protected static readonly FunctionToken[] functionTokens =
	[

	];

	public EuclideanSpaceCalculator() : base(
	[
		// Matrix
		new(BracketedRegex(), match => MakeMatrix(match[1..^2])),
	])
	{ }

	private static MatrixHolder MakeMatrix(string content)
	{
		string[] rows = content.Split('&', StringSplitOptions.TrimEntries);
		string[][] tokenized = [.. rows.Select(row => row.Split(';', StringSplitOptions.TrimEntries))];

		ValueHolder<Rational>[,] matrix = new ValueHolder<Rational>[tokenized.Length, tokenized[0].Length];
		Standard.StandardCalculator standardCalculator = new();

		try
		{
			for (int row = tokenized.Length - 1; row >= 0; --row)
			{
				for (int col = tokenized[row].Length - 1; col >= 0; --col)
					matrix[row, col] = Evaluate<Rational>(tokenized[row][col], standardCalculator);
			}
		}
		catch (IndexOutOfRangeException)
		{
			throw new FormatException();
		}

		return new MatrixHolder(matrix);
	}

	public override List<(string Calculation, string State)> FullEvaluation(string input) => FullEvaluation(Evaluate<MatrixHolder>(input, this));
}