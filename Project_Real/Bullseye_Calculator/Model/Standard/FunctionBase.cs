namespace Bullseye_Calculator.Model.Standard;

public abstract class FunctionBase(int parameters) : ValueHolder
{
	protected readonly ValueHolder[] parameters = new ValueHolder[parameters];

	public ValueHolder[] Parameters => parameters;

	internal abstract Priority Order();
	public abstract string Sign();
}