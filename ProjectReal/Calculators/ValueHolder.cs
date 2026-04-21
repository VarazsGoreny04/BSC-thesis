using System;
using System.Collections.Generic;

namespace Calculators;

/// <summary>
/// Represents a value holding node.
/// </summary>
/// <typeparam name="T">The type to hold.</typeparam>
public abstract partial class ValueHolder<T> : Expression
{
	#region Protected methods

	/// <summary>
	/// Parenthesizes the given <see cref="T"/> value if it is signed.
	/// </summary>
	/// <param name="value">The given <see cref="T"/> value.</param>
	/// <returns>The result as a <see cref="string"/>.</returns>
	protected static string ParenthesizeIfSigned(T value)
	{
		string result = value?.ToString() ?? throw new FormatException();

		return result.Length > 0 && (result[0] == '+' || result[0] == '-') ? $"({result})" : result;
	}

	#endregion

	#region Public methods

	/// <summary>
	/// Evaluates <see langword="this"/> instance and returns the partial results in a list. The final result is in the last node of the list.
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

	#endregion
}