namespace Bullseye_Calculator.Model.Standard;

public abstract class FunctionBase(int parameters) : ValueHolder
{
	protected readonly ValueHolder[] parameters = new ValueHolder[parameters];

	public ValueHolder[] Parameters => parameters;

	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result)
	{
		if (functions.FirstOrDefault() is FunctionBase fB && fB.Order() >= Order())
		{
			functions.Pop();

			result.Add(fB);
		}

		functions.Push(this);
	}

	internal abstract Priority Order();
	public abstract string Sign();
}