using Calculators;
using Calculators.EuclideanSpace;
using Calculators.Interpolation;
using Calculators.Standard;
using ProjectReal.Number;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BullseyeCalculator.Persistence;

public class CalculatorData
{
	#region Fields

	internal readonly Calculator.FunctionToken<Rational>[] standardFunctionTokens;
	internal readonly Calculator.FunctionToken<Matrix<Rational>>[] euclideanSpaceFunctionTokens;

	internal readonly StandardCalculator<Rational> standardCalculator;
	internal readonly EuclideanSpaceCalculator<Rational> euclideanSpaceCalculator;
	internal readonly InterpolationCalculator<Rational> interpolationCalculator;

	internal Mode mode;
	internal Calculator calculator;

	internal readonly List<string> input;

	internal readonly ObservableCollection<string> evaluation;
	internal string result;

	#endregion

	#region Properties

	public Calculator Calculator => calculator;
	public Mode Mode => mode;
	public string Input => string.Concat(input);

	public static char Separator => Rational.Separator;
	public static char RowSeparator => Matrix<Rational>.RowSeparator;
	public static char ColumnSeparator => Matrix<Rational>.ColumnSeparator;

	public static bool FractionalFormat { get => Rational.FractionalFormat; set => Rational.FractionalFormat = value; }
	public static int FractionCalculationLength { get => Rational.FractionCalculationLength; set => Rational.FractionCalculationLength = value; }

	public ObservableCollection<string> Evaluation => evaluation;
	public string Result => result;

	#endregion

	#region Constructors

	public CalculatorData()
	{
		standardFunctionTokens = [
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
		euclideanSpaceFunctionTokens = [
			new("diag", () => new Diagonalize<Rational>()),
			new("inv", () => new Inverse<Rational>())
		];

		Rational.WriteSign = false;

		standardCalculator = new StandardCalculator<Rational>(standardFunctionTokens);
		euclideanSpaceCalculator = new EuclideanSpaceCalculator<Rational>(euclideanSpaceFunctionTokens, standardCalculator);
		interpolationCalculator = new InterpolationCalculator<Rational>(standardCalculator);
		
		mode = Mode.Standard;
		calculator = standardCalculator;

		input = [];

		evaluation = [];
		result = string.Empty;
	}

	#endregion

	#region Public methods

	public void SwitchMode(Mode mode)
	{
		this.mode = mode;
		calculator = mode switch
		{
			Mode.Standard => standardCalculator,
			Mode.Matrix => euclideanSpaceCalculator,
			Mode.Interpolation => interpolationCalculator,
			_ => throw new NotImplementedException("No corresponding calculator found for this mode!")
		};
	}

	#endregion
}