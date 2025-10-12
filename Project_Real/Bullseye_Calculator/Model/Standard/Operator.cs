namespace Bullseye_Calculator.Model.Standard;

public abstract class Operator(int parameter) : Function(parameter)
{
	internal override void AcceptPostfix(ref Stack<Expression> functions, ref List<Expression> result)
	{
		Calculator.VisitPostfix(ref functions, ref result, this);
	}
	internal override void AcceptTree(ref Stack<Expression> result) => Calculator.VisitTree(ref result, this);
}	