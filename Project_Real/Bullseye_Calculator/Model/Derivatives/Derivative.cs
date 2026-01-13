namespace Bullseye_Calculator.Model.Derivatives;

public class Derivative : Function<Variable>
{
	public ValueHolder<Variable> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public Derivative() : base(1) { }

	public Derivative(ValueHolder<Variable> parameter) : base(1) => Parameter = parameter;

	public override Variable GetValue() => null!;

	public override string Sign() => "D";
}