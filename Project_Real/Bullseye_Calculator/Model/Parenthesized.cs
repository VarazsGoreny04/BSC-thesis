using System;
using System.Collections.Generic;

namespace Bullseye_Calculator.Model;

/// <summary>
/// Represents a parenthesized section in an expression.
/// </summary>
/// <typeparam name="T">The type of the <see cref="ValueHolder{T}"/> inside.</typeparam>
public sealed class Parenthesized<T> : ValueHolder<T>
{
	#region Fields

	private ValueHolder<T> value;

	#endregion

	#region Properties

	/// <summary>
	/// Gets or sets the value inside the parenthesized area.
	/// </summary>
	public ValueHolder<T> Content
	{
		get => value;
		set => this.value = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Parenthesized{T}"/> object.
	/// </summary>
	/// <param name="value">The value inside the parenthesized area.</param>
	public Parenthesized(ValueHolder<T> value) => this.value = value;

	#endregion

	#region Internal methods

	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result) => throw new NotImplementedException();

	internal override void ToTree(ref Stack<Expression> result) => throw new NotImplementedException();

	#endregion

	#region Public methods

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<T> root, ref int step)
	{
		value.FullEvaluation(ref partialValues, root, ref step);
	}

	public override T GetValue() => value.GetValue();

	public override string ToStringByStep(ref int step) => $"({value.ToStringByStep(ref step)})";

	#endregion
}