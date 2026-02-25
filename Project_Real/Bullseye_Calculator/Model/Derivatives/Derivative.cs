using Bullseye_Calculator.Model.Standard;
using Project_Real;
using System;

namespace Bullseye_Calculator.Model.Derivatives;

public class Derivative : Function<ValueHolder<Rational>>
{
	public ValueHolder<ValueHolder<Rational>> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public Derivative() : base(1) { }

	public Derivative(ValueHolder<ValueHolder<Rational>> parameter) : base(1) => Parameter = parameter;

	public override ValueHolder<Rational> GetValue() => null!;

	public override string Sign() => "D";


	public static ValueHolder<Rational> Simplify(ValueHolder<Rational> v) => v;
	public static ValueHolder<Rational> Simplify(Term<Rational> v) => new X(v, null);
	public static ValueHolder<Rational> Simplify(Parenthesized<Rational> v)
	{
		v.Content = Simplify(v.Content);

		return v.Content is X content ? content : v;
	}

	public static ValueHolder<Rational> Simplify(FunctionBase<Rational> v) => throw new NotImplementedException();
	public static ValueHolder<Rational> Simplify(BinaryOperator v)
	{
		v.Left = Simplify(v.Left);
		v.Right = Simplify(v.Right);

		return v;
	}
	public static ValueHolder<Rational> Simplify(Multiply v)
	{
		v.Left = Simplify(v.Left);
		v.Right = Simplify(v.Right);

		return (v.Left is X left && v.Right is X right) ?
			new X(new Number(left.Coefficient.GetValue() * right.Coefficient.GetValue()), new Number(left.Power.GetValue() + right.Power.GetValue())) :
			v;
	}
	public static ValueHolder<Rational> Simplify(Divide v)
	{
		v.Left = Simplify(v.Left);
		v.Right = Simplify(v.Right);

		return (v.Left is X left && v.Right is X right) ? 
			new X(new Number(left.Coefficient.GetValue() / right.Coefficient.GetValue()), new Number(left.Power.GetValue() - right.GetValue())) :
			v;
	}
	public static ValueHolder<Rational> Simplify(Power v)
	{
		v.Left = Simplify(v.Left);
		v.Right = Simplify(v.Right);

		if (v.Left is X left && v.Right is X right)
		{
			left.Power = new Number(left.Power.GetValue() * right.Coefficient.GetValue());
			right.Coefficient = new Number(Digit.ONE);

			if (right.Power.GetValue().IsZero)
				return left;
		}

		return v;
	}
	public static ValueHolder<Rational> Simplify(Root v)
	{
		v.Left = Simplify(v.Left);
		v.Right = Simplify(v.Right);

		if (v.Left is X left && v.Right is X right)
		{
			right.Power = new Number(right.Power.GetValue() / left.Coefficient.GetValue());
			left.Coefficient = new Number(Digit.ONE);

			if (left.Power.GetValue().IsZero)
				return right;
		}

		return v;
	}
}