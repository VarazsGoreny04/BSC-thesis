using Calculators;
using Calculators.EuclideanSpace;
using Calculators.Interpolation;
using Calculators.Standard;
using ProjectReal.Number;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BullseyeCalculator.Process;

internal class Program
{
	/// <summary>
	/// The entry point of the program.
	/// </summary>
	/// <param name="args">The program arguments.</param>
	/// <exception cref="ArgumentException">The length of the argument list must be equal to 4.</exception>
	public static void Main(string[] args)
	{
		Console.OutputEncoding = Encoding.Unicode;

		try
		{
			if (args.Length != 4)
				throw new ArgumentException($"The length of the argument list ({args.Length}) must be equal to 4!");

			char calculatorType = args[0].Length == 1 ? args[0][0] : throw new ArgumentException("The calculator type must be a single character!");
			bool fractionalFormat = bool.Parse(args[1]);
			int fractionCalculationLength = int.Parse(args[2]);
			string formula = args[3];

			Calculator calculator = SelectCalculator(calculatorType);
			Rational.WriteSign = false;
			Rational.FractionalFormat = fractionalFormat;
			Rational.FractionCalculationLength = fractionCalculationLength;

			WriteEvaluation(formula, calculator.FullEvaluation(formula));
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
		}
	}

	/// <summary>
	/// Creates a calculator instance based on the given calculator type.
	/// </summary>
	/// <param name="calculatorType">The calculator type.</param>
	/// <returns>The created calculator.</returns>
	/// <exception cref="ArgumentException">The given calculator type is unknown.</exception>
	private static Calculator SelectCalculator(char calculatorType)
	{
		Calculator.FunctionToken<Rational>[] standardFunctionTokens = [
			new("ceiling", () => new Ceiling()),
			new("round", () => new Round()),
			new("floor", () => new Floor()),
			new("exp", () => new Exp()),
			new("cos", () => new Cos()),
			new("sin", () => new Sin()),
			new("max", () => new Max<Rational>()),
			new("min", () => new Min<Rational>()),
			new("abs", () => new Abs()),
			new("ln", () => new Ln()),
			new("pi", () => new PI()),
			new("e", () => new E())
		];

		Calculator.FunctionToken<Matrix<Rational>>[] euclideanSpaceFunctionTokens = [
			new("diag", () => new Diagonalize<Rational>()),
			new("inv", () => new Inverse<Rational>())
		];

		StandardCalculator<Rational> standardCalculator = new(standardFunctionTokens);

		return calculatorType switch
		{
			'S' => standardCalculator,
			'M' => new EuclideanSpaceCalculator<Rational>(euclideanSpaceFunctionTokens, standardCalculator),
			'I' => new InterpolationCalculator<Rational>(standardCalculator),
			_ => throw new ArgumentException($"Unknown calculator type \"{calculatorType}\"!")
		};
	}

	/// <summary>
	/// Writes each step of the the evaluation, starting with the result of the calculation.
	/// </summary>
	/// <param name="input">The initial formula.</param>
	/// <param name="evaluation">The steps of the evaluation.</param>
	private static void WriteEvaluation(string input, List<(string Calculation, string State)> evaluation)
	{
		if (evaluation.Count > 0)
		{
			Console.WriteLine(evaluation.Last().State);

			int maxLength = evaluation.Max(step => step.Calculation.Length);
			evaluation.ForEach(step => Console.WriteLine($"{step.Calculation}{new string(' ', maxLength - step.Calculation.Length)} ─→ {step.State}"));
		}
		else
			Console.WriteLine(input);
	}
}