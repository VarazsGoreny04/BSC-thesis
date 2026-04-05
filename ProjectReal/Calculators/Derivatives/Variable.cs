using ProjectReal.NumberSet;
using System;
using System.Numerics;

namespace Calculators.Derivatives;

public sealed class X<T> : Function<T>
where T :
	IEqualityOperators<T, T, bool>,
	IAdditiveIdentity<T, T>,
	IMultiplicativeIdentity<T, T>,
	IAdditionOperators<T, T, T>,
	ISubtractionOperators<T, T, T>,
	IMultiplyOperators<T, T, T>,
	IDivisionOperators<T, T, T>,
	IPowerOperations<T, T, T>,
	IRootOperations<T, T, T>,
	IParsable<T>
{
	#region Fields

	private static T value = T.MultiplicativeIdentity;

	#endregion

	#region Properties

	public static T Value { get => value; set => X<T>.value = value; }

	public ValueHolder<T> Coefficient => parameters[0];
	public ValueHolder<T> Power => parameters[1];

	#endregion

	#region Constructor

	public X(Term<T>? coefficient, Term<T>? power) : base([coefficient ?? new Number<T>(T.AdditiveIdentity), power ?? new Number<T>(T.AdditiveIdentity)]) { }

	#endregion

	#region Public methods

	public override T GetValue() => Coefficient.GetValue() * (value ^ Power.GetValue());

	public override string Sign() => "x";

	public override string ToStringByStep(ref int step) => $"{(Coefficient is Term<T> c ? $"{c}*" : "")}{Sign()}{(Power is Term<T> p ? $"^{p}" : "")}";

	//public override FunctionBase<R> Simplify<R>() => this as FunctionBase<R>;

	public override bool Equals(object? obj) => obj is X<T> x && this == x;

	public override int GetHashCode() => throw new NotImplementedException();

	#endregion

	#region Operators

	public static bool operator ==(X<T> left, X<T> right)
	{
		return left.Coefficient.GetValue() == right.Coefficient.GetValue() && left.Power.GetValue() == right.Power.GetValue();
	}
	public static bool operator !=(X<T> left, X<T> right) => !(left == right);

	#endregion
}