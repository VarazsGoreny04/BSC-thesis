using System;
using System.Collections.Generic;

namespace Calculators;

/// <summary>
/// Represents an operator.
/// </summary>
/// <typeparam name="T">The type of the <see cref="ValueHolder{T}"/> in the parameters.</typeparam>
public abstract partial class Operator<T> : FunctionBase<T>
{
	#region Constructors

	/// <summary>
	/// Constructs an <see cref="Operator{T}"/> with a given parameters.
	/// </summary>
	/// <param name="parameters">The parameters of the operator.</param>
	protected Operator(ValueHolder<T>[] parameters) : base(parameters) { }

	#endregion

	#region Internal methods

	internal override void ToTree(ref Stack<Expression> result)
	{
		int length = Math.Min(parameters.Length, result.Count);

		for (int i = 1; i <= length && result.Peek() is ValueHolder<T> valueHolder; ++i)
		{
			result.Pop();

			parameters[^i] = valueHolder;
		}

		result.Push(this);
	}

	#endregion
}