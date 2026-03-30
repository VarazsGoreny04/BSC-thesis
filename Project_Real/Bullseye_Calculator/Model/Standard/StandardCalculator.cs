using Project_Real.NumberSet;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Bullseye_Calculator.Model.Standard;

/// <summary>
/// A calculator that understands the basic numbers, operations and trigonometric functions.
/// </summary>
public partial class StandardCalculator<T> : Calculator
where T :
	IAdditiveIdentity<T, T>,
	IAdditionOperators<T, T, T>,
	ISubtractionOperators<T, T, T>,
	IUnaryNegationOperators<T, T>,
	IMultiplyOperators<T, T, T>,
	IDivisionOperators<T, T, T>,
	IPowerOperations<T, T, T>,
	IRootOperations<T, T, T>,
	IParsable<T>
{
	#region Fields

	protected static readonly FunctionToken[] functionTokens =
	[
		new("ceiling", () => new Ceiling()),
		new("round", () => new Round()),
		new("floor", () => new Floor()),
		new("fact", () => new Fact()),
		new("exp", () => new Exp()),
		new("cos", () => new Cos()),
		new("sin", () => new Sin()),
		new("max", () => new Max()),
		new("min", () => new Min()),
		new("abs", () => new Abs()),
		new("ln", () => new Ln()),
		new("pi", () => new PI()),
		new("e", () => new E()),
	];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a calculator that understands the basic numbers, operations and trigonometric functions.
	/// </summary>
	public StandardCalculator() : base(
	[
		// Number
		new(NumberRegex(), value => new Number<T>(T.Parse(value, null))),
		// Function name
		new(FunctionNameRegex(), name => GetFunctionByName(functionTokens, name)),
		// Operators
		new(AddRegex(), _ => new Add<T>()),
		new(SubtractRegex(), _ => new Subtract<T>()),
		new(MultiplyRegex(), _ => new Multiply<T>()),
		new(DivideRegex(), _ => new Divide<T>()),
		new(PowerRegex(), _ => new Power<T>()),
		new(RootRegex(), _ => new Root<T>()),
		// Separators
		new(OpeningParenthesisRegex(), _ => new OpeningParenthesis()),
		new(ClosingParenthesisRegex(), _ => new ClosingParenthesis<T>()),
		new(ComaRegex(), _ => new Coma<T>())
	])
	{ }

	#endregion

	#region Public methods

	public override List<(string Calculation, string State)> FullEvaluation(string input) => FullEvaluation(Evaluate<T>(input, this));

	#endregion
}