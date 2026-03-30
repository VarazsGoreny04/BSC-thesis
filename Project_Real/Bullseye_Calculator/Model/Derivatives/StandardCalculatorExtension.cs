using System;

namespace Bullseye_Calculator.Model;

public partial class Multiply<T>
{
	public override FunctionBase<R> ToFunctionBase<R>()
	{
		Term<R> left = Left?.ToFunctionBase<R>() is Term<R> term1 ? term1 : throw new ArgumentException();
		Term<R> right = Right?.ToFunctionBase<R>() is Term<R> term2 ? term2 : throw new ArgumentException();

		return new Multiply<R>(left, right);
	}
}

public partial class Divide<T>
{
	public override FunctionBase<R> ToFunctionBase<R>()
	{
		Term<R> left = Left?.ToFunctionBase<R>() is Term<R> term1 ? term1 : throw new ArgumentException();
		Term<R> right = Right?.ToFunctionBase<R>() is Term<R> term2 ? term2 : throw new ArgumentException();

		return new Divide<R>(left, right);

		/*return (Left is X<T> left && Right is X<T> right) ?
			new X<T>(
				new Number<T>(left.Coefficient.GetValue() / right.Coefficient.GetValue()), 
				new Number<T>(left.Power.GetValue() - right.Power.GetValue())
			) :
			this;*/
	}
}

public partial class Power<T>
{
	public override FunctionBase<R> ToFunctionBase<R>()
	{
		Term<R> left = Left?.ToFunctionBase<R>() is Term<R> term1 ? term1 : throw new ArgumentException();
		Term<R> right = Right?.ToFunctionBase<R>() is Term<R> term2 ? term2 : throw new ArgumentException();

		/*if (Left is X<T> left && Right is X<T> right)
		{
			left.Power = new Number<T>(left.Power.GetValue() * right.Coefficient.GetValue());
			right.Coefficient = new Number<T>(T.MultiplicativeIdentity);

			if (right.Power.GetValue() == T.AdditiveIdentity)
				return left;
		}*/

		return new Power<R>(left, right);
	}
}

public partial class Root<T>
{
	public override FunctionBase<R> ToFunctionBase<R>()
	{
		Term<R> left = Left?.ToFunctionBase<R>() is Term<R> term1 ? term1 : throw new ArgumentException();
		Term<R> right = Right?.ToFunctionBase<R>() is Term<R> term2 ? term2 : throw new ArgumentException();

		/*if (Left is X<T> left && Right is X<T> right)
		{
			right.Power = new Number<T>(right.Power.GetValue() / left.Coefficient.GetValue());
			left.Coefficient = new Number<T>(T.MultiplicativeIdentity);

			if (left.Power.GetValue() == T.AdditiveIdentity)
				return right;
		}*/

		return new Root<R>(left, right);
	}
}