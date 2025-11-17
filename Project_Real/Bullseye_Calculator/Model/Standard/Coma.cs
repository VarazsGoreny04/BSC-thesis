namespace Bullseye_Calculator.Model.Standard;

public sealed class Coma() : Expression
{
	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result) => result.Add(this);
	internal override void ToTree(ref Stack<Expression> result)
	{
		(new ClosingParenthesis()).ToTree(ref result);
		(new OpeningParenthesis()).ToTree(ref result);
	}
	public override string ToStringByStep(ref int _) => ",";
}