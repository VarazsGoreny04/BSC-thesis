using System.Collections.Generic;
using System.Linq;

namespace Bullseye_Calculator.Model;

/// <summary>
/// Represents a function in an expression.
/// </summary>
/// <typeparam name="T">The type of the <see cref="ValueHolder{T}"/> in the parameters.</typeparam>
public abstract partial class FunctionBase<T> : ValueHolder<T>
{
	#region Fields

	protected readonly ValueHolder<T>[] parameters;

	#endregion

	#region Properties methods

	/// <returns>The array containing the parameters of the function.</returns>
	public ValueHolder<T>[] Parameters => parameters;

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="FunctionBase{T}"/> object with a given parameters.
	/// </summary>
	/// <param name="parameters">The parameters of the function.</param>
	protected FunctionBase(ValueHolder<T>[] parameters) => this.parameters = parameters;

	#endregion

	#region Internal methods

	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result)
	{
		if (functions.FirstOrDefault() is FunctionBase<T> fB && fB.Order() >= Order())
		{
			functions.Pop();

			result.Add(fB);
		}

		functions.Push(this);
	}

	/// <summary>
	/// Returns the <see cref="Priority"/> of <see langword="this"/> function.
	/// </summary>
	/// <returns>The <see cref="Priority"/>.</returns>
	internal abstract Priority Order();

	#endregion

	#region Public methods

	/// <summary>
	/// Returns the sign of <see langword="this"/> function.
	/// </summary>
	/// <returns>The sign represented by a <see cref="string"/>.</returns>
	public abstract string Sign();

	#endregion
}