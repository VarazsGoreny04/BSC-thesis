using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculators;

/// <summary>
/// Represents a parenthesis in an expression.
/// </summary>
public abstract class Parenthesis : Expression { }

/// <summary>
/// Represents an opening parenthesis in an expression.
/// </summary>
public sealed class OpeningParenthesis : Parenthesis
{
	#region Constructors
	
	/// <summary>
	/// Constructs an <see cref="OpeningParenthesis"/>.
	/// </summary>
	public OpeningParenthesis() { }

	#endregion

	#region Internal methods

	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result)
	{
		functions.Push(this);
		result.Add(this);
	}
	internal override void ToTree(ref Stack<Expression> result) => result.Push(this);

	#endregion

	#region Public methods

	public override string ToStringByStep(ref int _) => "(";

	#endregion
}

/// <summary>
/// Represents a closing parenthesis in an expression.
/// </summary>
public sealed class ClosingParenthesis<T> : Parenthesis
{
	#region Constructors

	/// <summary>
	/// Constructs a <see cref="ClosingParenthesis{T}"/>.
	/// </summary>
	public ClosingParenthesis() { }

	#endregion

	#region Internal methods

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

	#endregion

	#region Internal methods

	public override string ToStringByStep(ref int _) => ")";

	#endregion
}