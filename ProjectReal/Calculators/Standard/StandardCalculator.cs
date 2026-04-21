using ProjectReal.NumberSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Calculators.Standard;

/// <summary>
/// A calculator that understands the basic numbers, operations and trigonometric functions.
/// </summary>
/// <typeparam name="T">The type to calculate with.</typeparam>
public class StandardCalculator<T> : Calculator
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

	protected readonly FunctionToken<T>[] functionTokens = [];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a calculator that understands the basic numbers, operations and trigonometric functions.
	/// </summary>
	public StandardCalculator(FunctionToken<T>[] functionTokens) : base(
		[
			// Number
			new(NumberRegex(), value => new Number<T>(T.Parse(value, null))),
			// Function name
			new(FunctionNameRegex(), name => GetFunctionByName(functionTokens ?? [], name)),
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
		]
	) => this.functionTokens = functionTokens;

	#endregion

	#region Protected methods

	protected override string[] GetFunctions() => [.. functionTokens.Select(t => t.Name)];

	#endregion

	#region Public methods

	public override List<(string Calculation, string State)> FullEvaluation(string input) => FullEvaluation(Evaluate<T>(input, this));

	#endregion
}