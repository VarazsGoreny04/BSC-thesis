using Project_Real;
using System;
using System.Collections.Generic;

namespace Bullseye_Calculator.Model;

/// <summary>
/// Represents a value holding node in an expression.
/// </summary>
public abstract partial class ValueHolder<T> : Expression
{
	/// <summary>
	/// Parenthesizes the given <see cref="Rational"/> value if it is signed.
	/// </summary>
	/// <param name="value">The given <see cref="Rational"/> value.</param>
	/// <returns>The result as a <see cref="string"/>.</returns>
	protected static string ParenthesizeIfSigned(Rational value) => Rational.WriteSign || !value.Sign ? $"({value})" : value.ToString();

	/// <summary>
	/// Evaluates <see langword="this"/> instance and returns the partial and the final result in a  
	/// </summary>
	/// <param name="partialValues">The result.</param>
	/// <param name="root">The root node of the expression.</param>
	/// <param name="step">The number of calculations taken.</param>
	public abstract void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<T> root, ref int step);

	/// <summary>
	/// Calculates the value of <see langword="this"/> instance.
	/// </summary>
	/// <returns>The calculated value.</returns>
	public abstract T GetValue();
}