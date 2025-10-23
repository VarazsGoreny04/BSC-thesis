using Project_Real;

namespace Bullseye_Calculator.Model.Standard;

public abstract class Term : ValueHolder
{
	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder root, ref int step) { }
	internal override void AcceptPostfix(ref Stack<Expression> functions, ref List<Expression> result)
	{
		Calculator.VisitPostfix(ref functions, ref result, this);
	}
	internal override void AcceptTree(ref Stack<Expression> result) => Calculator.VisitTree(ref result, this);
}

public sealed class PI : Term
{
	public override Rational GetValue() => Rational.PI_Chudnovsky();
	public override string ToStringByStep(ref int _) => "pi";
}

public sealed class E : Term
{
	public override Rational GetValue() => Rational.E();
	public override string ToStringByStep(ref int _) => "e";
}