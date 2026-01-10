using System.Collections.Generic;
using System.Linq;

namespace Bullseye_Calculator.Model;

public abstract class FunctionBase<T> : ValueHolder<T>
{
	protected readonly ValueHolder<T>[] parameters;

	public ValueHolder<T>[] Parameters => parameters;

	public FunctionBase(int parameters) => this.parameters = new ValueHolder<T>[parameters];

	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result)
	{
		if (functions.FirstOrDefault() is FunctionBase<T> fB && fB.Order() >= Order())
		{
			functions.Pop();

			result.Add(fB);
		}

		functions.Push(this);
	}

	internal abstract Priority Order();
	public abstract string Sign();
}