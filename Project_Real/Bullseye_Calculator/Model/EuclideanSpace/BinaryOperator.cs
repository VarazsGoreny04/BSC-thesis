using System;
using System.Collections.Generic;

namespace Bullseye_Calculator.Model.EuclideanSpace;

public abstract class BinaryOperator : Operator<Matrix>
{
	public ValueHolder<Matrix> Left
	{
		get => parameters[0];
		set => parameters[0] = value;
	}
	public ValueHolder<Matrix> Right
	{
		get => parameters[1];
		set => parameters[1] = value;
	}

	public BinaryOperator() : base(2) { }

	public BinaryOperator(ValueHolder<Matrix> left, ValueHolder<Matrix> right) : base(2)
	{
		Left = left;
		Right = right;
	}

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<Matrix> root, ref int step)
	{
		Left?.FullEvaluation(ref partialValues, root, ref step);
		Right?.FullEvaluation(ref partialValues, root, ref step);

		int stepCopy = ++step;

		if (Left?.Value is Matrix left)
		{
			Matrix right = Right?.Value ?? throw new FormatException();
			partialValues.Add(($"{left}{Sign()}{right} = {Value}", root.ToStringByStep(ref stepCopy)));
		}
	}
	internal override Priority Order() => Priority.BinaryOperatorFirstClass;
	public override string ToStringByStep(ref int step)
	{
		string left = Left?.ToStringByStep(ref step) ?? "";
		string right = Right.ToStringByStep(ref step);

		return --step <= 0 ? $"{left}{Sign()}{right}" : Value.ToString();
	}
}

public sealed class Add : BinaryOperator
{
	public Add() : base() { }
	public Add(ValueHolder<Matrix> left, ValueHolder<Matrix> right) : base(left, right) { }

	public override Matrix GetValue() => Right is ValueHolder<Matrix> r && Left is ValueHolder<Matrix> l ? l.GetValue() + r.GetValue() : throw new FormatException();
	public override string Sign() => "+";
}

public sealed class Subtract : BinaryOperator
{
	public Subtract() : base() { }
	public Subtract(ValueHolder<Matrix> left, ValueHolder<Matrix> right) : base(left, right) { }

	public override Matrix GetValue() => Right is ValueHolder<Matrix> r && Left is ValueHolder<Matrix> l ? l.GetValue() - r.GetValue() : throw new FormatException();
	public override string Sign() => "-";
}

public sealed class Multiply : BinaryOperator
{
	public Multiply() : base() { }
	public Multiply(ValueHolder<Matrix> left, ValueHolder<Matrix> right) : base(left, right) { }

	public override Matrix GetValue() => Left is ValueHolder<Matrix> l && Right is ValueHolder<Matrix> r ? l.GetValue() * r.GetValue() : throw new FormatException();
	internal override Priority Order() => Priority.BinaryOperatorSecondClass;
	public override string Sign() => "*";
}