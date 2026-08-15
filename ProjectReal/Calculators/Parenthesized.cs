using System;
using System.Collections.Generic;

namespace Calculators;

/// <summary>
/// Represents a parenthesized section.
/// </summary>
/// <typeparam name="T">The type of the <see cref="ValueHolder{T}"/> inside.</typeparam>
public sealed partial class Parenthesized<T> : FunctionBase<T>
{
	#region Properties

	/// <summary>
	/// Gets or sets the value inside the parenthesized area.
	/// </summary>
	public ValueHolder<T> Content => parameters[0];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Parenthesized{T}"/> object.
	/// </summary>
	/// <param name="value">The value inside the parenthesized area.</param>
	public Parenthesized(ValueHolder<T> value) : base([value]) { }

	#endregion

	#region Protected methods

	protected override T CalculateValue() => Content.GetValue();

	#endregion

	#region Internal methods

	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result)
	{
		throw new NotImplementedException("This method should never be called!");
	}

	internal override void ToTree(ref Stack<Expression> result)
	{
		throw new NotImplementedException("This method should never be called!");
	}

	internal override Priority Order() => Priority.Function;

	#endregion

	#region Public methods

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<T> root, ref int step)
	{
		Content.FullEvaluation(ref partialValues, root, ref step);
	}

	public override string ToStringByStep(ref int step) => $"({Content.ToStringByStep(ref step)})";

	public override string Sign() => "()";

	#endregion
}