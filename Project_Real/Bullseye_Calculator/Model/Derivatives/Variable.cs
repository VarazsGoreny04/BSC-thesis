using Project_Real.NumberSet;
using System;
using System.Numerics;

namespace Bullseye_Calculator.Model.Derivatives;

public abstract class Variable<T> : Function<T>
where T : 
	IAdditiveIdentity<T, T>, 
	IMultiplicativeIdentity<T, T>, 
	IMultiplyOperators<T, T, T>, 
	IPowerOperations<T, T, T>, 
	IParsable<T>
{
	protected readonly char sign;

	public ValueHolder<T> Coefficient => parameters[0] ?? new Number<T>(T.AdditiveIdentity);
	public ValueHolder<T> Power => parameters[1] ?? new Number<T>(T.AdditiveIdentity);

	public Variable(char sign, Term<T>? coefficient, Term<T>? power) : base(
		[
			coefficient ?? new Number<T>(T.MultiplicativeIdentity), 
			power ?? new Number<T>(T.MultiplicativeIdentity)
		]
	)
	{
		this.sign = sign;
	}

	public override string ToStringByStep(ref int step) => $"{(Coefficient is Term<T> c ? $"{c}*" : "")}{sign}{(Power is Term<T> p ? $"^{p}" : "")}";

	public static T CalculateValue(Variable<T> variable, T value)
	{
		T withoutCoefficient = variable.Power is Term<T> power ? value ^ power.GetValue() : value;

		return variable.Coefficient is Term<T> coefficient ? coefficient.GetValue() * withoutCoefficient : withoutCoefficient;
	}
}

public sealed class X<T> : Variable<T>
where T : 
	IMultiplyOperators<T, T, T>, 
	IPowerOperations<T, T, T>,
	IAdditiveIdentity<T, T>,
	IMultiplicativeIdentity<T, T>,
	IParsable<T>
{
	private static T value = T.MultiplicativeIdentity;

	public X(Term<T>? coefficient, Term<T>? power) : base('x', coefficient, power) { }

	public override T GetValue() => CalculateValue(this, value);

	public override string Sign() => sign.ToString();
}