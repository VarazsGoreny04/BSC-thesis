namespace Bullseye_Calculator.Model.Standard;

public abstract class Expression
{
	internal abstract void AcceptPostfix(ref Stack<Expression> functions, ref List<Expression> result);
	internal abstract void AcceptTree(ref Stack<Expression> result);
	public abstract string StepToString(ref int step);

	public override string ToString()
	{
		int step = 1;
		return StepToString(ref step);
	}
}