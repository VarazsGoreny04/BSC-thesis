using Calculators;
using ProjectReal.Number;
using System;

namespace BullseyeCalculator.Model;

/// <summary>
/// Represents an absolute value function in an expression.
/// </summary>
public sealed class Abs : Function<Rational>
{
	#region Properties

	/// <summary>
	/// Gets or sets the parameter of the function.
	/// </summary>
	public ValueHolder<Rational> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs an <see cref="Abs"/> function with the parameter set to <see langword="null"/>.
	/// </summary>
	public Abs() : base([null!]) { }

	/// <summary>
	/// Constructs an <see cref="Abs"/> function with the <paramref name="parameter"/> value.
	/// </summary>
	/// <param name="parameter">The parameter of the function.</param>
	public Abs(ValueHolder<Rational> parameter) : base([parameter]) { }

	#endregion

	#region Public methods

	public override Rational GetValue() => Rational.Abs(Parameter?.GetValue() ?? throw new FormatException());

	public override string Sign() => "abs";

	#endregion
}

/// <summary>
/// Represents a floor function in an expression.
/// </summary>
public sealed class Floor : Function<Rational>
{
	#region Properties

	/// <summary>
	/// Gets or sets the parameter of the function.
	/// </summary>
	public ValueHolder<Rational> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Floor"/> function with the parameter set to <see langword="null"/>.
	/// </summary>
	public Floor() : base([null!]) { }

	/// <summary>
	/// Constructs a <see cref="Floor"/> function with the <paramref name="parameter"/> value.
	/// </summary>
	/// <param name="parameter">The parameter of the function.</param>
	public Floor(ValueHolder<Rational> parameter) : base([parameter]) { }

	#endregion

	#region Public methods

	public override Rational GetValue() => Rational.RoundDown(Parameter?.GetValue() ?? throw new FormatException());

	public override string Sign() => "floor";

	#endregion
}

/// <summary>
/// Represents a round function in an expression.
/// </summary>
public sealed class Round : Function<Rational>
{
	#region Properties

	/// <summary>
	/// Gets or sets the parameter of the function.
	/// </summary>
	public ValueHolder<Rational> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Round"/> function with the parameter set to <see langword="null"/>.
	/// </summary>
	public Round() : base([null!]) { }

	/// <summary>
	/// Constructs a <see cref="Round"/> function with the <paramref name="parameter"/> value.
	/// </summary>
	/// <param name="parameter">The parameter of the function.</param>
	public Round(ValueHolder<Rational> parameter) : base([parameter]) { }

	#endregion

	#region Public methods

	public override Rational GetValue() => Rational.Round(Parameter?.GetValue() ?? throw new FormatException());

	public override string Sign() => "round";

	#endregion
}

/// <summary>
/// Represents a ceiling function in an expression.
/// </summary>
public sealed class Ceiling : Function<Rational>
{
	#region Properties

	/// <summary>
	/// Gets or sets the parameter of the function.
	/// </summary>
	public ValueHolder<Rational> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Ceiling"/> function with the parameter set to <see langword="null"/>.
	/// </summary>
	public Ceiling() : base([null!]) { }

	/// <summary>
	/// Constructs a <see cref="Ceiling"/> function with the <paramref name="parameter"/> value.
	/// </summary>
	/// <param name="parameter">The parameter of the function.</param>
	public Ceiling(ValueHolder<Rational> parameter) : base([parameter]) { }

	#endregion

	#region Public methods

	public override Rational GetValue() => Rational.RoundUp(Parameter?.GetValue() ?? throw new FormatException());

	public override string Sign() => "ceiling";

	#endregion
}

/// <summary>
/// Represents a factorial function in an expression.
/// </summary>
public sealed class Fact : Function<Rational>
{
	#region Properties

	/// <summary>
	/// Gets or sets the parameter of the function.
	/// </summary>
	public ValueHolder<Rational> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Fact"/> function with the parameter set to <see langword="null"/>.
	/// </summary>
	public Fact() : base([null!]) { }

	/// <summary>
	/// Constructs a <see cref="Fact"/> function with the <paramref name="parameter"/> value.
	/// </summary>
	/// <param name="parameter">The parameter of the function.</param>
	public Fact(ValueHolder<Rational> parameter) : base([parameter]) { }

	#endregion

	#region Public methods

	public override Rational GetValue() => Rational.Abs(Parameter?.GetValue() ?? throw new FormatException());

	public override string Sign() => "fact";

	#endregion
}

/// <summary>
/// Represents the sine function in an expression.
/// </summary>
public sealed class Sin : Function<Rational>
{
	#region Properties

	/// <summary>
	/// Gets or sets the parameter of the function.
	/// </summary>
	public ValueHolder<Rational> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Sin"/> function with the parameters set to <see langword="null"/>.
	/// </summary>
	public Sin() : base([null!]) { }

	/// <summary>
	/// Constructs a <see cref="Sin"/> function with the <paramref name="parameter"/> value.
	/// </summary>
	/// <param name="parameter">The parameter of the function.</param>
	public Sin(ValueHolder<Rational> parameter) : base([parameter]) { }

	#endregion

	#region Public methods

	public override Rational GetValue() => Rational.Sin(Parameter?.GetValue() ?? throw new FormatException());

	public override string Sign() => "sin";

	#endregion
}

/// <summary>
/// Represents the cosine function in an expression.
/// </summary>
public sealed class Cos : Function<Rational>
{
	#region Properties

	/// <summary>
	/// Gets or sets the parameter of the function.
	/// </summary>
	public ValueHolder<Rational> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Cos"/> function with the parameters set to <see langword="null"/>.
	/// </summary>
	public Cos() : base([null!]) { }

	/// <summary>
	/// Constructs a <see cref="Cos"/> function with the <paramref name="parameter"/> value.
	/// </summary>
	/// <param name="parameter">The parameter of the function.</param>
	public Cos(ValueHolder<Rational> parameter) : base([parameter]) { }

	#endregion

	#region Public methods

	public override Rational GetValue() => Rational.Cos(Parameter?.GetValue() ?? throw new FormatException());

	public override string Sign() => "cos";

	#endregion
}


/// <summary>
/// Represents the exponential function in an expression.
/// </summary>
public sealed class Exp : Function<Rational>
{
	#region Properties

	/// <summary>
	/// Gets or sets the parameter of the function.
	/// </summary>
	public ValueHolder<Rational> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Exp"/> function with the parameters set to <see langword="null"/>.
	/// </summary>
	public Exp() : base([null!]) { }

	/// <summary>
	/// Constructs a <see cref="Exp"/> function with the <paramref name="parameter"/> value.
	/// </summary>
	/// <param name="parameter">The parameter of the function.</param>
	public Exp(ValueHolder<Rational> parameter) : base([parameter]) { }

	#endregion

	#region Public methods

	public override Rational GetValue() => Rational.Exp(Parameter?.GetValue() ?? throw new FormatException());

	public override string Sign() => "exp";

	#endregion
}

/// <summary>
/// Represents the natural logarithm function in an expression.
/// </summary>
public sealed class Ln : Function<Rational>
{
	#region Properties

	/// <summary>
	/// Gets or sets the parameter of the function.
	/// </summary>
	public ValueHolder<Rational> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Ln"/> function with the parameters set to <see langword="null"/>.
	/// </summary>
	public Ln() : base([null!]) { }

	/// <summary>
	/// Constructs a <see cref="Ln"/> function with the <paramref name="parameter"/> value.
	/// </summary>
	/// <param name="parameter">The parameter of the function.</param>
	public Ln(ValueHolder<Rational> parameter) : base([parameter]) { }

	#endregion

	#region Public methods

	public override Rational GetValue() => Rational.Ln(Parameter?.GetValue() ?? throw new FormatException());

	public override string Sign() => "ln";

	#endregion
}