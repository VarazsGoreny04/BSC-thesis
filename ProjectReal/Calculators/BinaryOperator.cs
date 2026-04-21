using System.Collections.Generic;

namespace Calculators;

/// <summary>
/// Represents a binary operator.
/// </summary>
/// <typeparam name="T">The type of the <see cref="ValueHolder{T}"/> in the parameters.</typeparam>
public abstract partial class BinaryOperator<T> : Operator<T>
{
	#region Properties

	/// <summary>
	/// Gets or sets the value on the left hand side of the operator.
	/// </summary>
	public ValueHolder<T> Left
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	/// <summary>
	/// Gets or sets the value on the left hand side of the operator.
	/// </summary>
	public ValueHolder<T> Right
	{
		get => parameters[1];
		set => parameters[1] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="BinaryOperator<T>"/> with the <paramref name="left"/> and <paramref name="right"/> values.
	/// </summary>
	/// <param name="left">The left hand side of the operator.</param>
	/// <param name="right">The right hand side of the operator.</param>
	public BinaryOperator(ValueHolder<T> left, ValueHolder<T> right) : base([left, right]) { }

	#endregion

	#region Internal methods

	internal override Priority Order() => Priority.FirstClass;

	#endregion

	#region Public methods

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<T> root, ref int step)
	{
		Left.FullEvaluation(ref partialValues, root, ref step);
		Right.FullEvaluation(ref partialValues, root, ref step);

		int stepCopy = ++step;

		partialValues.Add(($"{ParenthesizeIfSigned(Left.GetValue())}{Sign()}{ParenthesizeIfSigned(Right.GetValue())} = {GetValue()}",
			root.ToStringByStep(ref stepCopy)));
	}

	public override string ToStringByStep(ref int step)
	{
		string left = Left.ToStringByStep(ref step);
		string right = Right.ToStringByStep(ref step);

		return --step <= 0 ? $"{left}{Sign()}{right}" : ParenthesizeIfSigned(GetValue());
	}

	#endregion
}