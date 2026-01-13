using System;
using System.Collections.Generic;

namespace Bullseye_Calculator.Model.Derivatives;

/*public abstract class BinaryOperator : Operator<Variable>
{
	public ValueHolder<Variable> Left
	{
		get => parameters[0];
		set => parameters[0] = value;
	}
	public ValueHolder<Variable> Right
	{
		get => parameters[1];
		set => parameters[1] = value;
	}

	public BinaryOperator() : base(2) { }

	public BinaryOperator(ValueHolder<Variable> left, ValueHolder<Variable> right) : base(2)
	{
		Left = left;
		Right = right;
	}

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<Variable> root, ref int step)
	{
		Left?.FullEvaluation(ref partialValues, root, ref step);
		Right?.FullEvaluation(ref partialValues, root, ref step);

		int stepCopy = ++step;

		if (Left?.GetValue() is Variable left)
		{
			Variable right = Right?.GetValue() ?? throw new FormatException();
			partialValues.Add(($"{ParenthesizeIfSigned(left)}{Sign()}{ParenthesizeIfSigned(right)} = {GetValue()}", root.ToStringByStep(ref stepCopy)));
		}
	}
	internal override Priority Order() => Priority.BinaryOperatorFirstClass;
	public override string ToStringByStep(ref int step)
	{
		string left = Left?.ToStringByStep(ref step) ?? "";
		string right = Right?.ToStringByStep(ref step) ?? throw new FormatException();

		return --step <= 0 ? $"{left}{Sign()}{right}" : ParenthesizeIfSigned(GetValue());
	}
}

public sealed class Add : BinaryOperator
{
	public Add() : base() { }
	public Add(ValueHolder<Variable> left, ValueHolder<Variable> right) : base(left, right) { }

	public override Variable GetValue()
	{
		return Right is ValueHolder<Variable> r ? (Left is ValueHolder<Variable> l ? l.GetValue() + r.GetValue() : r.GetValue()) : throw new FormatException();
	}
	public override string Sign() => "+";
}

public sealed class Subtract : BinaryOperator
{
	public Subtract() : base() { }
	public Subtract(ValueHolder<Variable> left, ValueHolder<Variable> right) : base(left, right) { }

	public override Variable GetValue()
	{
		return Right is ValueHolder<Variable> r ? (Left is ValueHolder<Variable> l ? l.GetValue() - r.GetValue() : -r.GetValue()) : throw new FormatException();
	}
	public override string Sign() => "-";
}

public sealed class Multiply : BinaryOperator
{
	public Multiply() : base() { }
	public Multiply(ValueHolder<Variable> left, ValueHolder<Variable> right) : base(left, right) { }

	public override Variable GetValue() => Left is ValueHolder<Variable> l && Right is ValueHolder<Variable> r ? l.GetValue() * r.GetValue() : throw new FormatException();
	internal override Priority Order() => Priority.BinaryOperatorSecondClass;
	public override string Sign() => "*";
}

public sealed class Divide : BinaryOperator
{
	public Divide() : base() { }
	public Divide(ValueHolder<Variable> left, ValueHolder<Variable> right) : base(left, right) { }

	public override Variable GetValue() => Left is ValueHolder<Variable> l && Right is ValueHolder<Variable> r ? l.GetValue() / r.GetValue() : throw new FormatException();
	internal override Priority Order() => Priority.BinaryOperatorSecondClass;
	public override string Sign() => "/";
}

public sealed class Power : BinaryOperator
{
	public Power() : base() { }
	public Power(ValueHolder<Variable> left, ValueHolder<Variable> right) : base(left, right) { }

	public override Variable GetValue() => Left is ValueHolder<Variable> l && Right is ValueHolder<Variable> r ? l.GetValue() ^ r.GetValue() : throw new FormatException();
	internal override Priority Order() => Priority.BinaryOperatorThirdClass;
	public override string Sign() => "^";
}

public sealed class Root : BinaryOperator
{
	public Root() : base() { }
	public Root(ValueHolder<Variable> left, ValueHolder<Variable> right) : base(left, right) { }

	public override Variable GetValue() => Left is ValueHolder<Variable> l && Right is ValueHolder<Variable> r ? l.GetValue() | r.GetValue() : throw new FormatException();
	internal override Priority Order() => Priority.BinaryOperatorThirdClass;
	public override string Sign() => "|";
}*/