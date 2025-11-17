using System;
using System.Collections.Generic;

namespace Bullseye_Calculator.Model;

public sealed class Parenthesized<T>(ValueHolder<T> value) : ValueHolder<T>
{
	private readonly ValueHolder<T> value = value;

	public ValueHolder<T> Content => value;

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<T> root, ref int step)
	{
		value.FullEvaluation(ref partialValues, root, ref step);
	}
	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result) => throw new NotImplementedException();
	internal override void ToTree(ref Stack<Expression> result) => throw new NotImplementedException();
	public override T GetValue() => value.GetValue();
	public override string ToStringByStep(ref int step) => $"({value.ToStringByStep(ref step)})";
}