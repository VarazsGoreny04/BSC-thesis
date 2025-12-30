using Project_Real;
using System;
using System.Collections.Generic;

namespace Bullseye_Calculator.Model.Standard;

public abstract class BinaryOperator : Operator<Rational>
{
	public ValueHolder<Rational> Left
	{
		get => parameters[0];
		set => parameters[0] = value;
	}
	public ValueHolder<Rational> Right
	{
		get => parameters[1];
		set => parameters[1] = value;
	}

	public BinaryOperator() : base(2) { }

	public BinaryOperator(ValueHolder<Rational> left, ValueHolder<Rational> right) : base(2)
	{
		Left = left;
		Right = right;
	}

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<Rational> root, ref int step)
	{
		Left?.FullEvaluation(ref partialValues, root, ref step);
		Right?.FullEvaluation(ref partialValues, root, ref step);

		int stepCopy = ++step;

		if (Left?.Value is Rational left)
		{
			Rational right = Right?.Value ?? throw new FormatException();
			partialValues.Add(($"{ParenthesizeIfSigned(left)}{Sign()}{ParenthesizeIfSigned(right)} = {Value}", root.ToStringByStep(ref stepCopy)));
		}
	}
	internal override Priority Order() => Priority.BinaryOperatorFirstClass;
	public override string ToStringByStep(ref int step)
	{
		string left = Left?.ToStringByStep(ref step) ?? "";
		string right = Right?.ToStringByStep(ref step) ?? throw new FormatException();

		return --step <= 0 ? $"{left}{Sign()}{right}" : ParenthesizeIfSigned(Value);
	}
}

public sealed class Add : BinaryOperator
{
	public Add() : base() { }
	public Add(ValueHolder<Rational> left, ValueHolder<Rational> right) : base(left, right) { }

	public override Rational GetValue()
	{
		return Right is ValueHolder<Rational> r ? (Left is ValueHolder<Rational> l ? l.GetValue() + r.GetValue() : r.GetValue()) : throw new FormatException();
	}
	public override string Sign() => "+";
}

public sealed class Subtract : BinaryOperator
{
	public Subtract() : base() { }
	public Subtract(ValueHolder<Rational> left, ValueHolder<Rational> right) : base(left, right) { }

	public override Rational GetValue()
	{
		return Right is ValueHolder<Rational> r ? (Left is ValueHolder<Rational> l ? l.GetValue() - r.GetValue() : -r.GetValue()) : throw new FormatException();
	}
	public override string Sign() => "-";
}

public sealed class Multiply : BinaryOperator
{
	public Multiply() : base() { }
	public Multiply(ValueHolder<Rational> left, ValueHolder<Rational> right) : base(left, right) { }

	public override Rational GetValue() => Left is ValueHolder<Rational> l && Right is ValueHolder<Rational> r ? l.GetValue() * r.GetValue() : throw new FormatException();
	internal override Priority Order() => Priority.BinaryOperatorSecondClass;
	public override string Sign() => "*";
}

public sealed class Divide : BinaryOperator
{
	public Divide() : base() { }
	public Divide(ValueHolder<Rational> left, ValueHolder<Rational> right) : base(left, right) { }

	public override Rational GetValue() => Left is ValueHolder<Rational> l && Right is ValueHolder<Rational> r ? l.GetValue() / r.GetValue() : throw new FormatException();
	internal override Priority Order() => Priority.BinaryOperatorSecondClass;
	public override string Sign() => "/";
}

public sealed class Power : BinaryOperator
{
	public Power() : base() { }
	public Power(ValueHolder<Rational> left, ValueHolder<Rational> right) : base(left, right) { }

	public override Rational GetValue() => Left is ValueHolder<Rational> l && Right is ValueHolder<Rational> r ? l.GetValue() ^ r.GetValue() : throw new FormatException();
	internal override Priority Order() => Priority.BinaryOperatorThirdClass;
	public override string Sign() => "^";
}

public sealed class Root : BinaryOperator
{
	public Root() : base() { }
	public Root(ValueHolder<Rational> left, ValueHolder<Rational> right) : base(left, right) { }

	public override Rational GetValue() => Left is ValueHolder<Rational> l && Right is ValueHolder<Rational> r ? l.GetValue() | r.GetValue() : throw new FormatException();
	internal override Priority Order() => Priority.BinaryOperatorThirdClass;
	public override string Sign() => "|";
}