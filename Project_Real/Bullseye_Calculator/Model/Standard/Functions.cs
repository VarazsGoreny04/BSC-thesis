using Project_Real;
using System;

namespace Bullseye_Calculator.Model.Standard;

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
	public Abs() : base(1) { }

	/// <summary>
	/// Constructs an <see cref="Abs"/> function with the <paramref name="parameter"/> value.
	/// </summary>
	/// <param name="parameter">The parameter of the function.</param>
	public Abs(ValueHolder<Rational> parameter) : base(1) => Parameter = parameter;

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
	public Floor() : base(1) { }

	/// <summary>
	/// Constructs a <see cref="Floor"/> function with the <paramref name="parameter"/> value.
	/// </summary>
	/// <param name="parameter">The parameter of the function.</param>
	public Floor(ValueHolder<Rational> parameter) : base(1) => Parameter = parameter;

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
	public Round() : base(1) { }

	/// <summary>
	/// Constructs a <see cref="Round"/> function with the <paramref name="parameter"/> value.
	/// </summary>
	/// <param name="parameter">The parameter of the function.</param>
	public Round(ValueHolder<Rational> parameter) : base(1) => Parameter = parameter;

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
	public Ceiling() : base(1) { }

	/// <summary>
	/// Constructs a <see cref="Ceiling"/> function with the <paramref name="parameter"/> value.
	/// </summary>
	/// <param name="parameter">The parameter of the function.</param>
	public Ceiling(ValueHolder<Rational> parameter) : base(1) => Parameter = parameter;

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
	public Fact() : base(1) { }

	/// <summary>
	/// Constructs a <see cref="Fact"/> function with the <paramref name="parameter"/> value.
	/// </summary>
	/// <param name="parameter">The parameter of the function.</param>
	public Fact(ValueHolder<Rational> parameter) : base(1) => Parameter = parameter;

	#endregion

	#region Public methods

	public override Rational GetValue() => Rational.Abs(Parameter?.GetValue() ?? throw new FormatException());

	public override string Sign() => "fact";

	#endregion
}

/// <summary>
/// Represents a maximum value function in an expression.
/// </summary>
public sealed class Max : Function<Rational>
{
	#region Properties

	/// <summary>
	/// Gets or sets the first parameter of the function.
	/// </summary>
	public ValueHolder<Rational> First
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	/// <summary>
	/// Gets or sets the second parameter of the function.
	/// </summary>
	public ValueHolder<Rational> Second
	{
		get => parameters[1];
		set => parameters[1] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Max"/> function with the parameters set to <see langword="null"/>.
	/// </summary>
	public Max() : base(2) { }

	/// <summary>
	/// Constructs a <see cref="Max"/> function with the <paramref name="first"/> and <paramref name="second"/> values.
	/// </summary>
	/// <param name="first">The first parameter of the function.</param>
	/// <param name="second">The second parameter of the function.</param>
	public Max(ValueHolder<Rational> first, ValueHolder<Rational> second) : base(2)
	{
		First = first;
		Second = second;
	}

	#endregion

	#region Public methods

	public override Rational GetValue() => First.GetValue() >= Second.GetValue() ? First.GetValue() : Second.GetValue();

	public override string Sign() => "max";

	#endregion
}

/// <summary>
/// Represents a minimum value function in an expression.
/// </summary>
public sealed class Min : Function<Rational>
{
	#region Properties

	/// <summary>
	/// Gets or sets the first parameter of the function.
	/// </summary>
	public ValueHolder<Rational> First
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	/// <summary>
	/// Gets or sets the second parameter of the function.
	/// </summary>
	public ValueHolder<Rational> Second
	{
		get => parameters[1];
		set => parameters[1] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Min"/> function with the parameters set to <see langword="null"/>.
	/// </summary>
	public Min() : base(2) { }

	/// <summary>
	/// Constructs a <see cref="Min"/> function with the <paramref name="first"/> and <paramref name="second"/> values.
	/// </summary>
	/// <param name="first">The first parameter of the function.</param>
	/// <param name="second">The second parameter of the function.</param>
	public Min(ValueHolder<Rational> first, ValueHolder<Rational> second) : base(2)
	{
		First = first;
		Second = second;
	}

	#endregion

	#region Public methods

	public override Rational GetValue() => First.GetValue() >= Second.GetValue() ? First.GetValue() : Second.GetValue();

	public override string Sign() => "min";

	#endregion
}