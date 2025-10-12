using Project_Real;
using System;
using System.Collections.Generic;

namespace Bullseye_Calculator.Model.Standard;

public sealed class Parenthesized(ValueHolder e) : ValueHolder
{
	private readonly ValueHolder e = e;

	public ValueHolder E => e;

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder root, ref int step)
	{
		e.FullEvaluation(ref partialValues, root, ref step);
	}
	internal override void AcceptPostfix(ref Stack<Expression> functions, ref List<Expression> result) => throw new NotImplementedException();
	internal override void AcceptTree(ref Stack<Expression> result) => Calculator.VisitTree(ref result, this);
	public override Rational GetValue() => e.GetValue();
	public override string StepToString(ref int step) => $"({e.StepToString(ref step)})";
}