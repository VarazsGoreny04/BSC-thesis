using System.Collections.Generic;

namespace Bullseye_Calculator.Model;

public sealed class Coma<T>() : Expression
{
	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result) => result.Add(this);
	internal override void ToTree(ref Stack<Expression> result)
	{
		(new ClosingParenthesis<T>()).ToTree(ref result);
		(new OpeningParenthesis()).ToTree(ref result);
	}
	public override string ToStringByStep(ref int _) => ",";
}