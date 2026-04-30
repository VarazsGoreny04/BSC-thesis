using System;
using System.Collections.Generic;

namespace Calculators;

/// <summary>
/// Represents a separator character in a function call.
/// </summary>
/// <typeparam name="T">The type to separate.</typeparam>
public sealed class Coma<T> : Expression
{
	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Coma{T}"/>.
	/// </summary>
	public Coma() { }

	#endregion

	#region Internal methods

	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result)
	{
		new ClosingParenthesis<T>().ToPostfix(ref functions, ref result);
		new OpeningParenthesis().ToPostfix(ref functions, ref result);
	}

	/// <summary>
	/// Throws a <see cref="NotImplementedException"/> because a coma cannot be parsed into tree form.
	/// </summary>
	internal override void ToTree(ref Stack<Expression> _)
	{
		throw new NotImplementedException("This function should never be called because a coma cannot be parsed into tree form!");
	}

	#endregion

	#region Public methods

	public override string ToStringByStep(ref int _) => ",";

	#endregion
}