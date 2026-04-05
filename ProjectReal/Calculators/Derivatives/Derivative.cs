using System;

namespace Calculators.Derivatives;

public class Derivative<T> : Function<T>
{
	public ValueHolder<T> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public Derivative() : base([null!]) { }

	public Derivative(ValueHolder<T> parameter) : base([parameter]) { }

	public override T GetValue() => throw new NotImplementedException();

	public override string Sign() => "D";
}