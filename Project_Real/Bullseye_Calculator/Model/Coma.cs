using System.Collections.Generic;

namespace Bullseye_Calculator.Model;

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

	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result) => result.Add(this);

	internal override void ToTree(ref Stack<Expression> result)
	{
		(new ClosingParenthesis<T>()).ToTree(ref result);
		(new OpeningParenthesis()).ToTree(ref result);
	}

	#endregion

	#region Public methods

	public override string ToStringByStep(ref int _) => ",";

	#endregion
}