using System;
using System.Collections.Generic;

namespace Calculators;

/// <summary>
/// Represents a unary operator.
/// </summary>
/// <typeparam name="T">The type of the <see cref="ValueHolder{T}"/> in the parameter.</typeparam>
public abstract class UnaryOperator<T> : Operator<T>
{
	#region Properties

	/// <summary>
	/// Gets or sets the parameter of the operator.
	/// </summary>
	public ValueHolder<T> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="UnaryOperator"/> with the parameter set to <see langword="null"/>.
	/// </summary>
	public UnaryOperator() : base([null!]) { }

	/// <summary>
	/// Constructs a <see cref="UnaryOperator"/> with the <paramref name="parameter"/> value.
	/// </summary>
	/// <param name="parameter">The parameter of the operator.</param>
	public UnaryOperator(ValueHolder<T> parameter) : base([parameter]) { }

	#endregion

	#region Internal methods

	internal override Priority Order() => Priority.UnaryOperator;

	#endregion

	#region Public methods

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<T> root, ref int step)
	{
		++step;
		Parameter?.FullEvaluation(ref partialValues, root, ref step);

		int stepCopy = step;

		ValueHolder<T> parameter = Parameter ?? throw new FormatException();

		partialValues.Add(($"{Sign()}{ParenthesizeIfSigned(parameter.GetValue())} = {GetValue()}",
			root.ToStringByStep(ref stepCopy)));
	}

	public override string ToStringByStep(ref int step)
	{
		string parameterToString = Parameter.ToStringByStep(ref step);

		return --step <= 0 ? $"{parameterToString}{Sign()}" : $"({GetValue()})";
	}

	#endregion
}