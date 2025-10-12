namespace Bullseye_Calculator.Model.Standard;

public abstract class Function(int parameters) : ValueHolder
{
	protected readonly ValueHolder[] parameters = new ValueHolder[parameters];

	public ValueHolder[] Parameters => parameters;

	internal virtual Priority Order() => Priority.None;

	public abstract string Sign();
}