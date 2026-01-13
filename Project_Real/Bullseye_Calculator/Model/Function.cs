using System;
using System.Collections.Generic;
using System.Linq;

namespace Bullseye_Calculator.Model;

/// <summary>
/// Represents a function in an expression.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Function<T> : FunctionBase<T>
{
	#region Constructors

	/// <summary>
	/// Constructs a <see cref="FunctionBase{T}"/> object with a given number of parameters.
	/// </summary>
	/// <param name="parameters">The number of parameters used by the function.</param>
	protected Function(int parameters) : base(parameters) { }

	#endregion

	#region Internal methods

	internal override void ToTree(ref Stack<Expression> result)
	{
		for (int i = 1; i <= parameters.Length && result.Peek() is Parenthesized<T> parenthesized; ++i)
		{
			result.Pop();

			parameters[^i] = parenthesized.Content;
		}

		result.Push(this);
	}

	internal override Priority Order() => Priority.Function;

	#endregion

	#region Public methods

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<T> root, ref int step)
	{
		++step;

		foreach (ValueHolder<T> parameter in parameters)
			parameter?.FullEvaluation(ref partialValues, root, ref step)/* ?? throw new FormatException()*/;

		int stepCopy = step;

		partialValues.Add(($"{Sign()}({string.Join(", ", parameters.Select(p => p.GetValue()))}) = {GetValue()}", root.ToStringByStep(ref stepCopy)));
	}
	public override string ToStringByStep(ref int step)
	{
		string[] arguments = new string[parameters.Length];

		for (int i = 0; i < parameters.Length; ++i)
			arguments[i] = parameters[i].ToStringByStep(ref step) ?? throw new FormatException();

		return --step <= 0 ? $"{Sign()}({string.Join("; ", arguments)})" : $"({GetValue()})";
	}

	#endregion
}