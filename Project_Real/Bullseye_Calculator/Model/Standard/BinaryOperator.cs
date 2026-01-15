using Project_Real;
using System;
using System.Collections.Generic;

namespace Bullseye_Calculator.Model.Standard;

/// <summary>
/// Represents a binary operator in an expression.
/// </summary>
public abstract class BinaryOperator : Operator<Rational>
{
	#region Properties

	/// <summary>
	/// Gets or sets the value on the left hand side of the operator.
	/// </summary>
	public ValueHolder<Rational> Left
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	/// <summary>
	/// Gets or sets the value on the left hand side of the operator.
	/// </summary>
	public ValueHolder<Rational> Right
	{
		get => parameters[1];
		set => parameters[1] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="BinaryOperator"/> with the left and right values set to <see langword="null"/>.
	/// </summary>
	public BinaryOperator() : base(2) { }

	/// <summary>
	/// Constructs a <see cref="BinaryOperator"/> with the <paramref name="left"/> and <paramref name="right"/> values.
	/// </summary>
	/// <param name="left">The left hand side of the operator.</param>
	/// <param name="right">The right hand side of the operator.</param>
	public BinaryOperator(ValueHolder<Rational> left, ValueHolder<Rational> right) : base(2)
	{
		Left = left;
		Right = right;
	}

	#endregion

	#region Internal methods

	internal override Priority Order() => Priority.BinaryOperatorFirstClass;

	#endregion

	#region Public methods

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<Rational> root, ref int step)
	{
		Left?.FullEvaluation(ref partialValues, root, ref step);
		Right?.FullEvaluation(ref partialValues, root, ref step);

		int stepCopy = ++step;

		if (Left?.GetValue() is Rational left)
		{
			Rational right = Right?.GetValue() ?? throw new FormatException();
			partialValues.Add(($"{ParenthesizeIfSigned(left)}{Sign()}{ParenthesizeIfSigned(right)} = {GetValue()}", root.ToStringByStep(ref stepCopy)));
		}
	}

	public override string ToStringByStep(ref int step)
	{
		string left = Left?.ToStringByStep(ref step) ?? "";
		string right = Right?.ToStringByStep(ref step) ?? throw new FormatException();

		return --step <= 0 ? $"{left}{Sign()}{right}" : ParenthesizeIfSigned(GetValue());
	}

	#endregion
}

/// <summary>
/// Represents the add operator in an expression.
/// </summary>
public sealed class Add : BinaryOperator
{
	#region Constructors

	/// <summary>
	/// Constructs an <see cref="Add"/> operator with the left and right values set to <see langword="null"/>.
	/// </summary>
	public Add() : base() { }

	/// <summary>
	/// Constructs an <see cref="Add"/> operator with the <paramref name="left"/> and <paramref name="right"/> values.
	/// </summary>
	/// <param name="left">The left hand side of the operator.</param>
	/// <param name="right">The right hand side of the operator.</param>
	public Add(ValueHolder<Rational> left, ValueHolder<Rational> right) : base(left, right) { }

	#endregion

	#region Public methods

	public override Rational GetValue()
	{
		return Right is ValueHolder<Rational> r ? (Left is ValueHolder<Rational> l ? l.GetValue() + r.GetValue() : r.GetValue()) : throw new FormatException();
	}

	public override string Sign() => "+";

	#endregion
}

/// <summary>
/// 
/// </summary>
public sealed class Subtract : BinaryOperator
{
	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Subtract"/> operator with the left and right values set to <see langword="null"/>.
	/// </summary>
	public Subtract() : base() { }

	/// <summary>
	/// Constructs a <see cref="Subtract"/> operator with the <paramref name="left"/> and <paramref name="right"/> values.
	/// </summary>
	/// <param name="left">The left hand side of the operator.</param>
	/// <param name="right">The right hand side of the operator.</param>
	public Subtract(ValueHolder<Rational> left, ValueHolder<Rational> right) : base(left, right) { }

	#endregion

	#region Public methods

	public override Rational GetValue()
	{
		return Right is ValueHolder<Rational> r ? (Left is ValueHolder<Rational> l ? l.GetValue() - r.GetValue() : -r.GetValue()) : throw new FormatException();
	}

	public override string Sign() => "-";

	#endregion
}

/// <summary>
/// 
/// </summary>
public sealed class Multiply : BinaryOperator
{
	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Multiply"/> operator with the left and right values set to <see langword="null"/>.
	/// </summary>
	public Multiply() : base() { }

	/// <summary>
	/// Constructs a <see cref="Multiply"/> operator with the <paramref name="left"/> and <paramref name="right"/> values.
	/// </summary>
	/// <param name="left">The left hand side of the operator.</param>
	/// <param name="right">The right hand side of the operator.</param>
	public Multiply(ValueHolder<Rational> left, ValueHolder<Rational> right) : base(left, right) { }

	#endregion

	#region Internal methods

	internal override Priority Order() => Priority.BinaryOperatorSecondClass;

	#endregion

	#region Public methods

	public override Rational GetValue() => Left is ValueHolder<Rational> l && Right is ValueHolder<Rational> r ? l.GetValue() * r.GetValue() : throw new FormatException();

	public override string Sign() => "*";

	#endregion
}

/// <summary>
/// 
/// </summary>
public sealed class Divide : BinaryOperator
{
	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Divide"/> operator with the left and right values set to <see langword="null"/>.
	/// </summary>
	public Divide() : base() { }

	/// <summary>
	/// Constructs a <see cref="Divide"/> operator with the <paramref name="left"/> and <paramref name="right"/> values.
	/// </summary>
	/// <param name="left">The left hand side of the operator.</param>
	/// <param name="right">The right hand side of the operator.</param>
	public Divide(ValueHolder<Rational> left, ValueHolder<Rational> right) : base(left, right) { }

	#endregion

	#region Internal methods

	internal override Priority Order() => Priority.BinaryOperatorSecondClass;

	#endregion

	#region Public methods

	public override Rational GetValue() => Left is ValueHolder<Rational> l && Right is ValueHolder<Rational> r ? l.GetValue() / r.GetValue() : throw new FormatException();

	public override string Sign() => "/";

	#endregion
}

/// <summary>
/// 
/// </summary>
public sealed class Power : BinaryOperator
{
	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Power"/> operator with the left and right values set to <see langword="null"/>.
	/// </summary>
	public Power() : base() { }

	/// <summary>
	/// Constructs a <see cref="Power"/> operator with the <paramref name="left"/> and <paramref name="right"/> values.
	/// </summary>
	/// <param name="left">The left hand side of the operator.</param>
	/// <param name="right">The right hand side of the operator.</param>
	public Power(ValueHolder<Rational> left, ValueHolder<Rational> right) : base(left, right) { }

	#endregion

	#region Internal methods

	internal override Priority Order() => Priority.BinaryOperatorThirdClass;

	#endregion

	#region Public methods

	public override Rational GetValue() => Left is ValueHolder<Rational> l && Right is ValueHolder<Rational> r ? l.GetValue() ^ r.GetValue() : throw new FormatException();
	public override string Sign() => "^";

	#endregion
}

/// <summary>
/// 
/// </summary>
public sealed class Root : BinaryOperator
{
	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Root"/> operator with the left and right values set to <see langword="null"/>.
	/// </summary>
	public Root() : base() { }

	/// <summary>
	/// Constructs a <see cref="Root"/> operator with the <paramref name="left"/> and <paramref name="right"/> values.
	/// </summary>
	/// <param name="left">The left hand side of the operator.</param>
	/// <param name="right">The right hand side of the operator.</param>
	public Root(ValueHolder<Rational> left, ValueHolder<Rational> right) : base(left, right) { }

	#endregion

	#region Internal methods

	internal override Priority Order() => Priority.BinaryOperatorThirdClass;

	#endregion

	#region Public methods

	public override Rational GetValue() => Left is ValueHolder<Rational> l && Right is ValueHolder<Rational> r ? l.GetValue() | r.GetValue() : throw new FormatException();
	public override string Sign() => "|";

	#endregion
}