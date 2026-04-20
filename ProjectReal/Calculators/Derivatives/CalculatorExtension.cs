/*using Calculators.Derivatives;
using ProjectReal.NumberSet;
using System;
using System.Linq;
using System.Numerics;

namespace Calculators;

public abstract partial class ValueHolder<T> : Expression
{
	public abstract FunctionBase<R> ToXBased<R>()
	where R :
		T,
		IEqualityOperators<R, R, bool>,
		IAdditiveIdentity<R, R>,
		IMultiplicativeIdentity<R, R>,
		IAdditionOperators<R, R, R>,
		ISubtractionOperators<R, R, R>,
		IMultiplyOperators<R, R, R>,
		IDivisionOperators<R, R, R>,
		IPowerOperations<R, R, R>,
		IRootOperations<R, R, R>,
		IParsable<R>
	;
}

public abstract partial class Term<T> : ValueHolder<T>
{
	public override FunctionBase<R> ToXBased<R>() => new X<R>(this is Term<R> term ? term : throw new ArgumentException(), null);
}

public abstract partial class FunctionBase<T> : ValueHolder<T>
{
	public abstract FunctionBase<R> Simplify<R>()
	where R :
		T,
		IEqualityOperators<R, R, bool>,
		IAdditiveIdentity<R, R>,
		IMultiplicativeIdentity<R, R>,
		IAdditionOperators<R, R, R>,
		ISubtractionOperators<R, R, R>,
		IMultiplyOperators<R, R, R>,
		IDivisionOperators<R, R, R>,
		IPowerOperations<R, R, R>,
		IRootOperations<R, R, R>,
		IParsable<R>
	;
}

public sealed partial class Parenthesized<T> : FunctionBase<T>
{
	public override FunctionBase<R> ToXBased<R>() => new Parenthesized<R>(Content.ToXBased<R>());
}

public partial class Add<T> where T : IAdditiveIdentity<T, T>, IAdditionOperators<T, T, T>
{
	public override FunctionBase<R> ToXBased<R>()
	{
		Add<R> xBasedAdd = new();

		if (!Left.Equals(initialLeft))
			xBasedAdd.Left = Left.ToXBased<R>();

		xBasedAdd.Right = Right.ToXBased<R>();

		return xBasedAdd;
	}

	public override FunctionBase<R> Simplify<R>()
	{
		return Left is X<R> leftX && Right is X<R> rightX && leftX.Exponent.GetValue() == rightX.Exponent.GetValue() ?
			new X<R>(
				new Number<R>(leftX.Coefficient.GetValue() + rightX.Coefficient.GetValue()),
				leftX.Exponent is Term<R> exponent ? exponent : throw new ArgumentException()
			) :
			new Add<R>(
	}
}*/