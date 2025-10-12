using Project_Real;

namespace Bullseye_Calculator.Model.Standard;

public abstract class BinaryOperator : Operator
{
	public ValueHolder Left
	{
		get => parameters[0];
		set => parameters[0] = value;
	}
	public ValueHolder Right
	{
		get => parameters[1];
		set => parameters[1] = value;
	}

	public BinaryOperator() : base(2) { }

	public BinaryOperator(ValueHolder left, ValueHolder right) : base(2)
	{
		Left = left;
		Right = right;
	}

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder root, ref int step)
	{
		Left?.FullEvaluation(ref partialValues, root, ref step);
		Right?.FullEvaluation(ref partialValues, root, ref step);
		++step;

		int depthCopy = step;

		partialValues.Add(($"{(Left is ValueHolder l ? $"({l.Value})" : "")}{Sign()}({Right?.Value}) == {Value}",
			root.StepToString(ref depthCopy)));
	}
	internal override Priority Order() => Priority.BinaryOperatorFirstClass;
	public override string StepToString(ref int step)
	{
		string left = $"{Left?.StepToString(ref step) ?? ""}{Sign()}";
		string rigth = Right.StepToString(ref step);

		return --step <= 0 ? left + rigth : $"({Value})";
	}
}

public sealed class Add : BinaryOperator
{
	public Add() : base() { }
	public Add(ValueHolder left, ValueHolder right) : base(left, right) { }

	public override Rational GetValue()
	{
		return Right is ValueHolder r ? (Left is ValueHolder l ? l.GetValue() + r.GetValue() : r.GetValue()) : throw new FormatException();
	}
	public override string Sign() => "+";
}

public sealed class Subtract : BinaryOperator
{
	public Subtract() : base() { }
	public Subtract(ValueHolder left, ValueHolder right) : base(left, right) { }

	public override Rational GetValue()
	{
		return Right is ValueHolder r ? (Left is ValueHolder l ? l.GetValue() - r.GetValue() : -r.GetValue()) : throw new FormatException();
	}
	public override string Sign() => "-";
}

public sealed class Multiply : BinaryOperator
{
	public Multiply() : base() { }
	public Multiply(ValueHolder left, ValueHolder right) : base(left, right) { }

	public override Rational GetValue() => Left is ValueHolder l && Right is ValueHolder r ? l.GetValue() * r.GetValue() : throw new FormatException();
	internal override Priority Order() => Priority.BinaryOperatorSecondClass;
	public override string Sign() => "*";
}

public sealed class Divide : BinaryOperator
{
	public Divide() : base() { }
	public Divide(ValueHolder left, ValueHolder right) : base(left, right) { }

	public override Rational GetValue() => Left is ValueHolder l && Right is ValueHolder r ? l.GetValue() / r.GetValue() : throw new FormatException();
	internal override Priority Order() => Priority.BinaryOperatorSecondClass;
	public override string Sign() => "/";
}

public sealed class Power : BinaryOperator
{
	public Power() : base() { }
	public Power(ValueHolder left, ValueHolder right) : base(left, right) { }

	public override Rational GetValue() => Left is ValueHolder l && Right is ValueHolder r ? l.GetValue() ^ r.GetValue() : throw new FormatException();
	internal override Priority Order() => Priority.BinaryOperatorThirdClass;
	public override string Sign() => "^";
}

public sealed class Root : BinaryOperator
{
	public Root() : base() { }
	public Root(ValueHolder left, ValueHolder right) : base(left, right) { }

	public override Rational GetValue() => Left is ValueHolder l && Right is ValueHolder r ? l.GetValue() | r.GetValue() : throw new FormatException();
	internal override Priority Order() => Priority.BinaryOperatorThirdClass;
	public override string Sign() => "|";
}