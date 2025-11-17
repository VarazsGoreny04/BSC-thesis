using System.Collections.Generic;

namespace Bullseye_Calculator.Model;

public abstract class Term<T> : ValueHolder<T>
{
	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<T> root, ref int step) { }
	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result) => result.Add(this);
	internal override void ToTree(ref Stack<Expression> result) => result.Push(this);
}