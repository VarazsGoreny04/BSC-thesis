using Project_Real;
using System.Collections.Generic;

namespace Bullseye_Calculator.Model.Standard;

public abstract class UnaryOperator : Operator<Rational>
{
	public ValueHolder<Rational> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public UnaryOperator() : base(1) { }

	public UnaryOperator(ValueHolder<Rational> parameter) : base(1)
	{
		Parameter = parameter;
	}
	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<Rational> root, ref int step)
	{
		++step;
		Parameter?.FullEvaluation(ref partialValues, root, ref step);

		int stepCopy = step;

		partialValues.Add(($"({Parameter?.GetValue()}){Sign()} = {GetValue()}", root.ToStringByStep(ref stepCopy)));
	}
	internal override Priority Order() => Priority.UnaryOperator;
	public override string ToStringByStep(ref int step)
	{
		string parameterToString = Parameter.ToStringByStep(ref step);

		return --step <= 0 ? $"{parameterToString}{Sign()}" : $"({GetValue()})";
	}
}