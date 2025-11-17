using Project_Real;
using System;

namespace Bullseye_Calculator.Model.Standard;

public sealed class Abs : Function<Rational>
{
	public ValueHolder<Rational> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public Abs() : base(1) { }

	public Abs(ValueHolder<Rational> parameter) : base(1) => Parameter = parameter;

	public override Rational GetValue() => Rational.Abs(Parameter?.Value ?? throw new FormatException());

	public override string Sign() => "abs";
}

internal sealed class Floor : Function<Rational>
{
	public ValueHolder<Rational> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public Floor() : base(1) { }

	public Floor(ValueHolder<Rational> parameter) : base(1) => Parameter = parameter;

	public override Rational GetValue() => Rational.RoundDown(Parameter?.Value ?? throw new FormatException());

	public override string Sign() => "floor";
}

public sealed class Round : Function<Rational>
{
	public ValueHolder<Rational> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public Round() : base(1) { }

	public Round(ValueHolder<Rational> parameter) : base(1) => Parameter = parameter;

	public override Rational GetValue() => Rational.Round(Parameter?.Value ?? throw new FormatException());

	public override string Sign() => "round";
}

public sealed class Ceiling : Function<Rational>
{
	public ValueHolder<Rational> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public Ceiling() : base(1) { }

	public Ceiling(ValueHolder<Rational> parameter) : base(1) => Parameter = parameter;

	public override Rational GetValue() => Rational.RoundUp(Parameter?.Value ?? throw new FormatException());

	public override string Sign() => "ceiling";
}

public sealed class Fact : Function<Rational>
{
	public ValueHolder<Rational> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public Fact() : base(1) { }

	public Fact(ValueHolder<Rational> parameter) : base(1) => Parameter = parameter;

	public override Rational GetValue() => Rational.Abs(Parameter?.Value ?? throw new FormatException());

	public override string Sign() => "fact";
}

public sealed class Max : Function<Rational>
{
	public ValueHolder<Rational> First
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public ValueHolder<Rational> Second
	{
		get => parameters[1];
		set => parameters[1] = value;
	}

	public Max() : base(2) { }

	public Max(ValueHolder<Rational> first, ValueHolder<Rational> second) : base(2)
	{
		First = first;
		Second = second;
	}

	public override Rational GetValue() => First.Value >= Second.Value ? First.Value : Second.Value;

	public override string Sign() => "max";
}

public sealed class Min : Function<Rational>
{
	public ValueHolder<Rational> First
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public ValueHolder<Rational> Second
	{
		get => parameters[1];
		set => parameters[1] = value;
	}

	public Min() : base(2) { }

	public Min(ValueHolder<Rational> first, ValueHolder<Rational> second) : base(2)
	{
		First = first;
		Second = second;
	}

	public override Rational GetValue() => First.Value >= Second.Value ? First.Value : Second.Value;

	public override string Sign() => "min";
}