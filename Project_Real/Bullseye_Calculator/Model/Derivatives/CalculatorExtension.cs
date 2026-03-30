using Bullseye_Calculator.Model.Derivatives;
using Project_Real.NumberSet;
using System;
using System.Linq;
using System.Numerics;

namespace Bullseye_Calculator.Model;

public abstract partial class ValueHolder<T> : Expression
{
	public abstract FunctionBase<R> ToFunctionBase<R>()
	where R :
		T,
		IMultiplyOperators<R, R, R>,
		IDivisionOperators<R, R, R>,
		IPowerOperations<R, R, R>,
		IRootOperations<R, R, R>,
		IAdditiveIdentity<R, R>,
		IMultiplicativeIdentity<R, R>,
		IParsable<R>
	;
}

public abstract partial class Term<T> : ValueHolder<T>
{
	public override FunctionBase<R> ToFunctionBase<R>() => new X<R>(this is Term<R> term ? term : throw new ArgumentException(), null);
}

public abstract partial class FunctionBase<T> : ValueHolder<T>
{
	public override FunctionBase<R> ToFunctionBase<R>() // This function is weird :(
	{
		return Activator.CreateInstance(GetType(), parameters.Select(static p => p.ToFunctionBase<R>())) as FunctionBase<R> ?? throw new ArgumentException();
	}

	/*public abstract FunctionBase<R> Simplify<R>()
	where R :
		T,
		IMultiplyOperators<R, R, R>,
		IDivisionOperators<R, R, R>,
		IPowerOperations<R, R, R>,
		IRootOperations<R, R, R>,
		IAdditiveIdentity<R, R>,
		IMultiplicativeIdentity<R, R>,
		IParsable<R>
	;*/
}

public sealed partial class Parenthesized<T> : FunctionBase<T>
{
	public override FunctionBase<R> ToFunctionBase<R>() => new Parenthesized<R>(Content.ToFunctionBase<R>());
}