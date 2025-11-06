namespace Bullseye_Calculator.Model.Standard;

public abstract class Coma() : Expression
{
	internal override void AcceptPostfix(ref Stack<Expression> functions, ref List<Expression> result) => Calculator.VisitPostfix(ref functions, ref result, this);
	internal override void AcceptTree(ref Stack<Expression> result) => Calculator.VisitTree(ref result, this);
	public override string ToStringByStep(ref int _) => ",";
}