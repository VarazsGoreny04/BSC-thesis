using Project_Real;
using System.Collections.Generic;

namespace Bullseye_Calculator.Model.Standard;

/// <summary>
/// Represents a unary operator in an expression.
/// </summary>
public abstract class UnaryOperator : Operator<Rational>
{
	#region Properties

	/// <summary>
	/// Gets or sets the parameter of the operator.
	/// </summary>
	public ValueHolder<Rational> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="UnaryOperator"/> with the parameter set to <see langword="null"/>.
	/// </summary>
	public UnaryOperator() : base(1) { }

	/// <summary>
	/// Constructs a <see cref="UnaryOperator"/> with the <paramref name="parameter"/> value.
	/// </summary>
	/// <param name="parameter">The parameter of the operator.</param>
	public UnaryOperator(ValueHolder<Rational> parameter) : base(1) => Parameter = parameter;

	#endregion

	#region Internal methods

	internal override Priority Order() => Priority.UnaryOperator;

	#endregion

	#region Public methods

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<Rational> root, ref int step)
	{
		++step;
		Parameter?.FullEvaluation(ref partialValues, root, ref step);

		int stepCopy = step;

		partialValues.Add(($"({Parameter?.GetValue()}){Sign()} = {GetValue()}", root.ToStringByStep(ref stepCopy)));
	}

	public override string ToStringByStep(ref int step)
	{
		string parameterToString = Parameter.ToStringByStep(ref step);

		return --step <= 0 ? $"{parameterToString}{Sign()}" : $"({GetValue()})";
	}

	#endregion
}