using System.Collections.Generic;

namespace Calculators;

/// <summary>
/// Represents a term node in an expression.
/// </summary>
/// <typeparam name="T">The type of the term.</typeparam>
public abstract partial class Term<T> : ValueHolder<T>
{
	#region Internal methods

	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result) => result.Add(this);

	internal override void ToTree(ref Stack<Expression> result) => result.Push(this);

	#endregion

	#region Public methods
	
	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<T> root, ref int step) { }

	#endregion
}