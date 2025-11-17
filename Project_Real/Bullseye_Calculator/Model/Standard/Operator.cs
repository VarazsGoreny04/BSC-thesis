namespace Bullseye_Calculator.Model.Standard;

public abstract class Operator(int parameter) : FunctionBase(parameter)
{
    internal override void ToTree(ref Stack<Expression> result)
    {
		int length = Math.Min(Parameters.Length, result.Count);

		for (int i = 1; i <= length && result.Peek() is ValueHolder valueHolder; ++i)
		{
			result.Pop();

			Parameters[^i] = valueHolder;
		}

		result.Push(this);
	}
}