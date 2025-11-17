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

        partialValues.Add(($"{Sign()}({string.Join(", ", parameters.Select(p => p.Value))}) = {Value}", root.ToStringByStep(ref stepCopy)));
    }
    public override string ToStringByStep(ref int step)
    {
        string[] arguments = new string[parameters.Length];

        for (int i = 0; i < parameters.Length; ++i)
            arguments[i] = parameters[i].ToStringByStep(ref step);

        return --step <= 0 ? $"{Sign()}({string.Join("; ", arguments)})" : $"({Value})";
    }

    internal override void ToTree(ref Stack<Expression> result)
    {
		for (int i = 1; i <= Parameters.Length && result.Peek() is Parenthesized parenthesized; ++i)
		{
			result.Pop();

			Parameters[^i] = parenthesized.Content;
		}

		result.Push(this);
	}

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

    public override Rational GetValue() => Rational.Abs(Parameter?.Value ?? throw new FormatException());

    public override string Sign() => "abs";
}

internal sealed class Floor : Function
{
	public ValueHolder Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public Floor() : base(1) { }

	public Floor(ValueHolder parameter) : base(1) => Parameter = parameter;

	public override Rational GetValue() => Rational.RoundDown(Parameter?.Value ?? throw new FormatException());

	public override string Sign() => "floor";
}

public sealed class Round : Function
{
    public ValueHolder Parameter
    {
        get => parameters[0];
        set => parameters[0] = value;
    }

    public Round() : base(1) { }

    public Round(ValueHolder parameter) : base(1) => Parameter = parameter;

    public override Rational GetValue() => Rational.Round(Parameter?.Value ?? throw new FormatException());

    public override string Sign() => "round";
}

public sealed class Ceiling : Function
{
    public ValueHolder Parameter
    {
        get => parameters[0];
        set => parameters[0] = value;
    }

    public Ceiling() : base(1) { }

    public Ceiling(ValueHolder parameter) : base(1) => Parameter = parameter;

    public override Rational GetValue() => Rational.RoundUp(Parameter?.Value ?? throw new FormatException());

    public override string Sign() => "ceiling";
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

    public override Rational GetValue() => Rational.Abs(Parameter?.Value ?? throw new FormatException());

    public override string Sign() => "fact";
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