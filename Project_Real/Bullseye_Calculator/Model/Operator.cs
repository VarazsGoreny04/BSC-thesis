using System;
using System.Collections.Generic;

namespace Bullseye_Calculator.Model;

/// <summary>
/// Represents an operator in an expression.
/// </summary>
/// <typeparam name="T">The type of the <see cref="ValueHolder{T}"/> in the parameters.</typeparam>
public abstract class Operator<T> : FunctionBase<T>
{
	#region Constructors

	/// <summary>
	/// Constructs an <see cref="Operator{T}"/> object with a given number of parameters.
	/// </summary>
	/// <param name="parameters">The number of parameters used by the operator.</param>
	protected Operator(int parameters) : base(parameters) { }

	#endregion

	#region Internal methods

	internal override void ToTree(ref Stack<Expression> result)
	{
		int length = Math.Min(Parameters.Length, result.Count);

		for (int i = 1; i <= length && result.Peek() is ValueHolder<T> valueHolder; ++i)
		{
			result.Pop();

			Parameters[^i] = valueHolder;
		}

		result.Push(this);
	}

	#endregion
}