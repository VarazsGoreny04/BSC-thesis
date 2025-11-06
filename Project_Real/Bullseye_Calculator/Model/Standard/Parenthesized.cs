using Project_Real;

namespace Bullseye_Calculator.Model.Standard;

public sealed class Parenthesized(ValueHolder value) : ValueHolder
{
	private readonly ValueHolder value = value;

	public ValueHolder Content => value;

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder root, ref int step)
	{
		value.FullEvaluation(ref partialValues, root, ref step);
	}
	internal override void AcceptPostfix(ref Stack<Expression> functions, ref List<Expression> result) => throw new NotImplementedException();
	internal override void AcceptTree(ref Stack<Expression> result) => Calculator.VisitTree(ref result, this);
	public override Rational GetValue() => value.GetValue();
	public override string ToStringByStep(ref int step) => $"({value.ToStringByStep(ref step)})";
}