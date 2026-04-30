using ProjectReal.NumberSet;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Calculators;

/// <summary>
/// Represents the add operator.
/// </summary>
/// <typeparam name="T">The type of the <see cref="ValueHolder{T}"/> in the parameters.</typeparam>
public partial class Add<T> : BinaryOperator<T> where T : IAdditiveIdentity<T, T>, IAdditionOperators<T, T, T>
{
	#region Fields

	protected readonly ValueHolder<T>? initialLeft;

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs an <see cref="Add{T}"/> operator with the left value set to <see cref="T.AdditiveIdentity"/>
	/// and the right value set to <see langword="null"/>.
	/// </summary>
	public Add() : base(new Number<T>(T.AdditiveIdentity), null!) => initialLeft = Left;

	/// <summary>
	/// Constructs an <see cref="Add{T}"/> operator with the <paramref name="left"/> and <paramref name="right"/> values.
	/// </summary>
	/// <param name="left">The left hand side of the operator.</param>
	/// <param name="right">The right hand side of the operator.</param>
	public Add(ValueHolder<T> left, ValueHolder<T> right) : base(left, right) => initialLeft = null;

	#endregion

	#region Public methods

	public override T GetValue() => Left.GetValue() + Right.GetValue();

	public override string Sign() => "+";

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<T> root, ref int step)
	{
		if (Left.Equals(initialLeft))
		{
			Right.FullEvaluation(ref partialValues, root, ref step);

			int stepCopy = ++step;

			partialValues.Add(($"{Sign()}{ParenthesizeIfSigned(Right.GetValue())} = {GetValue()}", root.ToStringByStep(ref stepCopy)));
		}
		else
			base.FullEvaluation(ref partialValues, root, ref step);
	}

	public override string ToStringByStep(ref int step)
	{
		string left = Left.Equals(initialLeft) ? "" : Left.ToStringByStep(ref step);
		string right = Right.ToStringByStep(ref step);

		return --step <= 0 ? $"{left}{Sign()}{right}" : ParenthesizeIfSigned(GetValue());
	}

	#endregion
}

/// <summary>
/// Represents the subtract operator.
/// </summary>
/// <typeparam name="T">The type of the <see cref="ValueHolder{T}"/> in the parameters.</typeparam>
public partial class Subtract<T> : BinaryOperator<T> where T : IAdditiveIdentity<T, T>, ISubtractionOperators<T, T, T>, IUnaryNegationOperators<T, T>
{
	#region Fields

	protected readonly ValueHolder<T>? initialLeft;

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Subtract{T}"/> operator with the left value set to <see cref="T.AdditiveIdentity"/>
	/// and the right value set to <see langword="null"/>.
	/// </summary>
	public Subtract() : base(new Number<T>(T.AdditiveIdentity), null!) => initialLeft = Left;

	/// <summary>
	/// Constructs a <see cref="Subtract{T}"/> operator with the <paramref name="left"/> and <paramref name="right"/> values.
	/// </summary>
	/// <param name="left">The left hand side of the operator.</param>
	/// <param name="right">The right hand side of the operator.</param>
	public Subtract(ValueHolder<T> left, ValueHolder<T> right) : base(left, right) => initialLeft = null;

	#endregion

	#region Public methods

	public override T GetValue() => Left.GetValue() - Right.GetValue();

	public override string Sign() => "-";

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<T> root, ref int step)
	{
		if (Left.Equals(initialLeft))
		{
			Right.FullEvaluation(ref partialValues, root, ref step);

			int stepCopy = ++step;

			partialValues.Add(($"{Sign()}{ParenthesizeIfSigned(Right.GetValue())} = {GetValue()}", root.ToStringByStep(ref stepCopy)));
		}
		else
			base.FullEvaluation(ref partialValues, root, ref step);
	}

	public override string ToStringByStep(ref int step)
	{
		string left = Left.Equals(initialLeft) ? "" : Left.ToStringByStep(ref step);
		string right = Right.ToStringByStep(ref step);

		return --step <= 0 ? $"{left}{Sign()}{right}" : ParenthesizeIfSigned(GetValue());
	}

	#endregion
}

/// <summary>
/// Represents the multiply operator.
/// </summary>
/// <typeparam name="T">The type of the <see cref="ValueHolder{T}"/> in the parameters.</typeparam>
public partial class Multiply<T> : BinaryOperator<T> where T : IMultiplyOperators<T, T, T>
{
	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Multiply{T}"/> operator with the left and right values set to <see langword="null"/>.
	/// </summary>
	public Multiply() : base(null!, null!) { }

	/// <summary>
	/// Constructs a <see cref="Multiply{T}"/> operator with the <paramref name="left"/> and <paramref name="right"/> values.
	/// </summary>
	/// <param name="left">The left hand side of the operator.</param>
	/// <param name="right">The right hand side of the operator.</param>
	public Multiply(ValueHolder<T> left, ValueHolder<T> right) : base(left, right) { }

	#endregion

	#region Internal methods

	internal override Priority Order() => Priority.SecondClass;

	#endregion

	#region Public methods

	public override T GetValue() => Left.GetValue() * Right.GetValue();

	public override string Sign() => "*";

	#endregion
}

/// <summary>
/// Represents the divide operator.
/// </summary>
/// <typeparam name="T">The type of the <see cref="ValueHolder{T}"/> in the parameters.</typeparam>
public partial class Divide<T> : BinaryOperator<T> where T : IDivisionOperators<T, T, T>
{
	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Divide{T}"/> operator with the left and right values set to <see langword="null"/>.
	/// </summary>
	public Divide() : base(null!, null!) { }

	/// <summary>
	/// Constructs a <see cref="Divide{T}"/> operator with the <paramref name="left"/> and <paramref name="right"/> values.
	/// </summary>
	/// <param name="left">The left hand side of the operator.</param>
	/// <param name="right">The right hand side of the operator.</param>
	public Divide(ValueHolder<T> left, ValueHolder<T> right) : base(left, right) { }

	#endregion

	#region Internal methods

	internal override Priority Order() => Priority.SecondClass;

	#endregion

	#region Public methods

	public override T GetValue() => Left.GetValue() / Right.GetValue();

	public override string Sign() => "/";

	#endregion
}

/// <summary>
/// Represents the modulo operator.
/// </summary>
/// <typeparam name="T">The type of the <see cref="ValueHolder{T}"/> in the parameters.</typeparam>
public partial class Modulo<T> : BinaryOperator<T> where T : IModulusOperators<T, T, T>
{
	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Modulo{T}"/> operator with the left and right values set to <see langword="null"/>.
	/// </summary>
	public Modulo() : base(null!, null!) { }

	/// <summary>
	/// Constructs a <see cref="Modulo{T}"/> operator with the <paramref name="left"/> and <paramref name="right"/> values.
	/// </summary>
	/// <param name="left">The left hand side of the operator.</param>
	/// <param name="right">The right hand side of the operator.</param>
	public Modulo(ValueHolder<T> left, ValueHolder<T> right) : base(left, right) { }

	#endregion

	#region Internal methods

	internal override Priority Order() => Priority.SecondClass;

	#endregion

	#region Public methods

	public override T GetValue() => Left.GetValue() % Right.GetValue();

	public override string Sign() => "%";

	#endregion
}

/// <summary>
/// Represents the power operator.
/// </summary>
/// <typeparam name="T">The type of the <see cref="ValueHolder{T}"/> in the parameters.</typeparam>
public partial class Power<T> : BinaryOperator<T> where T : IPowerOperations<T, T, T>
{
	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Power{T}"/> operator with the left and right values set to <see langword="null"/>.
	/// </summary>
	public Power() : base(null!, null!) { }

	/// <summary>
	/// Constructs a <see cref="Power{T}"/> operator with the <paramref name="left"/> and <paramref name="right"/> values.
	/// </summary>
	/// <param name="left">The left hand side of the operator.</param>
	/// <param name="right">The right hand side of the operator.</param>
	public Power(ValueHolder<T> left, ValueHolder<T> right) : base(left, right) { }

	#endregion

	#region Internal methods

	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result)
	{
		while (functions.FirstOrDefault() is FunctionBase<T> fB && fB.Order() > Order())
		{
			functions.Pop();

			result.Add(fB);
		}

		functions.Push(this);
	}

	internal override Priority Order() => Priority.ThirdClass;

	#endregion

	#region Public methods

	public override T GetValue() => Left.GetValue() ^ Right.GetValue();

	public override string Sign() => "^";

	#endregion
}

/// <summary>
/// Represents the root operator.
/// </summary>
/// <typeparam name="T">The type of the <see cref="ValueHolder{T}"/> in the parameters.</typeparam>
public partial class Root<T> : BinaryOperator<T> where T : IRootOperations<T, T, T>
{
	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Root{T}"/> operator with the left and right values set to <see langword="null"/>.
	/// </summary>
	public Root() : base(null!, null!) { }

	/// <summary>
	/// Constructs a <see cref="Root{T}"/> operator with the <paramref name="left"/> and <paramref name="right"/> values.
	/// </summary>
	/// <param name="left">The left hand side of the operator.</param>
	/// <param name="right">The right hand side of the operator.</param>
	public Root(ValueHolder<T> left, ValueHolder<T> right) : base(left, right) { }

	#endregion

	#region Internal methods

	internal override Priority Order() => Priority.ThirdClass;

	#endregion

	#region Public methods

	public override T GetValue() => Left.GetValue() | Right.GetValue();

	public override string Sign() => "|";

	#endregion
}