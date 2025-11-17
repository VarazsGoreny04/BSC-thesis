using Project_Real;

namespace Bullseye_Calculator.Model.Standard;

public abstract class Term : ValueHolder
{
    public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder root, ref int step) { }
    internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result) => result.Add(this);
    internal override void ToTree(ref Stack<Expression> result) => result.Push(this);
}

public sealed class PI : Term
{
	public override Rational GetValue() => Rational.Pi();
	public override string ToStringByStep(ref int _) => "pi";
}

public sealed class E : Term
{
	public override Rational GetValue() => Rational.E();
	public override string ToStringByStep(ref int _) => "e";
}