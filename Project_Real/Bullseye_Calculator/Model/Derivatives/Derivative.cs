using System.Collections.Generic;

namespace Bullseye_Calculator.Model.Derivatives;

public class Derivative : Expression
{
	public override string ToStringByStep(ref int step)
	{
		throw new System.NotImplementedException();
	}

	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result)
	{
		throw new System.NotImplementedException();
	}

	internal override void ToTree(ref Stack<Expression> result)
	{
		throw new System.NotImplementedException();
	}
}