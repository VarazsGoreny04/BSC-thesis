using System.Collections.Generic;

namespace Bullseye_Calculator.Model;

/// <summary>
/// Represents a node of an expression.
/// </summary>
public abstract class Expression
{
	#region Internal methods

	/// <summary>
	/// Turns the given <see cref="Expression"/> stack into a list containing the postfix form of the expressions.
	/// </summary>
	/// <param name="functions">The given <see cref="Expression"/> stack.</param>
	/// <param name="result">The result list.</param>
	internal abstract void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result);

	/// <summary>
	/// Turns the given <see cref="Expression"/> stack into trees of expressions.
	/// </summary>
	/// <param name="result">The given <see cref="Expression"/> stack.</param>
	internal abstract void ToTree(ref Stack<Expression> result);

	#endregion

	#region Public methods

	/// <summary>
	/// Solves <see langword="this"/> instance until the given <paramref name="step"/> count and 
	/// gives back a <see cref="string"/> representing the partially solved expression.
	/// </summary>
	/// <param name="step"></param>
	/// <returns>The result of the calculation converted into a <see cref="string"/>.</returns>
	public abstract string ToStringByStep(ref int step);

	/// <summary>
	/// Solves <see langword="this"/> instance and gives back a <see cref="string"/> representing the solved expression.
	/// </summary>
	/// <param name="step"></param>
	/// <returns>The result of the calculation converted into a <see cref="string"/>.</returns>
	public override string ToString()
	{
		int step = 1;
		return ToStringByStep(ref step);
	}

	#endregion
}