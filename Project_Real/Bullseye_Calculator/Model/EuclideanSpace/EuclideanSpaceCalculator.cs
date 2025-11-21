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
		new(BracketedRegex(), match => MakeMatrix(match[1..^1])),
	])
	{ }

	private static Matrix MakeMatrix(string content)
	{
		string[] rows = content.Split('&', StringSplitOptions.TrimEntries);
		string[][] tokenized = [.. rows.Select(row => row.Split(';', StringSplitOptions.TrimEntries))];

		for (int i = 1; i < tokenized.Length; ++i)
		{
			if (tokenized[0].Length != tokenized[i].Length)
				throw new FormatException();
		}

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

		return new Matrix(matrix);
	}

	public override List<(string Calculation, string State)> FullEvaluation(string input) => FullEvaluation(Evaluate<ValueHolder<Rational>[,]>(input, this));
}