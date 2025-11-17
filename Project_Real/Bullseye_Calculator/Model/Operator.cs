using System;
using System.Collections.Generic;

namespace Bullseye_Calculator.Model;

public abstract class Operator<T>(int parameter) : FunctionBase<T>(parameter)
{
	internal override void ToTree(ref Stack<Expression> result)
	{
		int length = Math.Min(Parameters.Length, result.Count);

		for (int i = 1; i <= length && result.Peek() is ValueHolder<T> valueHolder; ++i)
		{
			result.Pop();

			Parameters[^i] = valueHolder;
		}

		result.Push(this);
	}
}