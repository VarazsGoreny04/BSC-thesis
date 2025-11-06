using Project_Real;

namespace Bullseye_Calculator.Model.Standard;

public abstract class Function(int parameters) : FunctionBase(parameters)
{
	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder root, ref int step)
	{
		++step;

		foreach (ValueHolder parameter in parameters)
			parameter?.FullEvaluation(ref partialValues, root, ref step);

		int stepCopy = step;

		partialValues.Add(($"{Sign()}({string.Join(", ", parameters.Select(p => p.Value))} = {Value}", root.ToStringByStep(ref stepCopy)));
	}
	public override string ToStringByStep(ref int step)
	{
		string[] arguments = new string[parameters.Length];

		for (int i = 0; i < parameters.Length; ++i)
			arguments[i] = parameters[i].ToStringByStep(ref step);

		return --step <= 0 ? $"{Sign()}({string.Join("; ", arguments)})" : $"({Value})";
	}

	internal override void AcceptPostfix(ref Stack<Expression> functions, ref List<Expression> result) => Calculator.VisitPostfix(ref functions, ref result, this);
	internal override void AcceptTree(ref Stack<Expression> result) => Calculator.VisitTree(ref result, this);

	internal override Priority Order() => Priority.Function;
}

public sealed class Abs : Function
{
	public ValueHolder Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public Abs() : base(1) { }

	public Abs(ValueHolder parameter) : base(1) => Parameter = parameter;

	public override Rational GetValue() => new(true, Parameter.Value.Numerator, Parameter.Value.Denominator);

	public override string Sign() => "abs";
}

public sealed class Fact : Function
{
	public ValueHolder Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public Fact() : base(1) { }

	public Fact(ValueHolder parameter) : base(1) => Parameter = parameter;

	public override Rational GetValue() => new(true, Parameter.Value.Numerator, Parameter.Value.Denominator);

	public override string Sign() => "abs";
}

public sealed class Max : Function
{
	public ValueHolder First
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public ValueHolder Second
	{
		get => parameters[1];
		set => parameters[1] = value;
	}

	public Max() : base(2) { }

	public Max(ValueHolder first, ValueHolder second) : base(2)
	{
		First = first;
		Second = second;
	}

	public override Rational GetValue() => First.Value >= Second.Value ? First.Value : Second.Value;

	public override string Sign() => "max";
}

public sealed class Min : Function
{
	public ValueHolder First
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public ValueHolder Second
	{
		get => parameters[1];
		set => parameters[1] = value;
	}

	public Min() : base(2) { }

	public Min(ValueHolder first, ValueHolder second) : base(2)
	{
		First = first;
		Second = second;
	}

	public override Rational GetValue() => First.Value >= Second.Value ? First.Value : Second.Value;

	public override string Sign() => "min";
}