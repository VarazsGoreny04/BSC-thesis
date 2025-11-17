using System;
using System.Collections.Generic;
using System.Linq;

namespace Bullseye_Calculator.Model;

public abstract class Parenthesis() : Expression { }

public sealed class OpeningParenthesis : Parenthesis
{
	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result)
	{
		functions.Push(this);
		result.Add(this);
	}
	internal override void ToTree(ref Stack<Expression> result) => result.Push(this);
	public override string ToStringByStep(ref int _) => "(";
}

public sealed class ClosingParenthesis<T> : Parenthesis
{
	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result)
	{
		if (!functions.Any(f => f is OpeningParenthesis))
			throw new FormatException();

		Expression e;

		while ((e = functions.Pop()) is not OpeningParenthesis)
			result.Add(e);

		result.Add(this);
	}
	internal override void ToTree(ref Stack<Expression> result)
	{
		Stack<ValueHolder<T>> temp = new();

		while (result.TryPop(out Expression? expression) && expression is not OpeningParenthesis)
		{
			if (expression is ValueHolder<T> valueHolder)
				temp.Push(valueHolder);
			else
				throw new FormatException();
		}

		if (temp.Count == 1)
			result.Push(new Parenthesized<T>(temp.Pop()));
		else
			throw new FormatException();
	}
	public override string ToStringByStep(ref int _) => ")";
}