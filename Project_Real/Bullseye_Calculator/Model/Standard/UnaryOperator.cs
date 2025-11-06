namespace Bullseye_Calculator.Model.Standard;

public abstract class UnaryOperator : Operator
{
	public ValueHolder Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public UnaryOperator() : base(1) { }

	public UnaryOperator(ValueHolder parameter) : base(1)
	{
		Parameter = parameter;
	}
	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder root, ref int step)
	{
		++step;
		Parameter?.FullEvaluation(ref partialValues, root, ref step);

		int stepCopy = step;

		partialValues.Add(($"({Parameter?.Value}){Sign()} = {Value}", root.ToStringByStep(ref stepCopy)));
	}
	internal override Priority Order() => Priority.UnaryOperator;
	public override string ToStringByStep(ref int step)
	{
		string parameterToString = Parameter.ToStringByStep(ref step);

		return --step <= 0 ? $"{parameterToString}{Sign()}" : $"({Value})";
	}
}