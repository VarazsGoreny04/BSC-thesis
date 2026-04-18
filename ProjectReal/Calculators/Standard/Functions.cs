using System.Numerics;

namespace Calculators.Standard;

/// <summary>
/// Represents a maximum value function in an expression.
/// </summary>
/// <typeparam name="T"></typeparam> // TODO: Mindenhol meg kell csinálni (BinaryOperator, UnaryOperator, stb...)
public sealed class Max<T> : Function<T> where T : IComparisonOperators<T, T, bool>
{
	#region Properties

	/// <summary>
	/// Gets or sets the first parameter of the function.
	/// </summary>
	public ValueHolder<T> First
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	/// <summary>
	/// Gets or sets the second parameter of the function.
	/// </summary>
	public ValueHolder<T> Second
	{
		get => parameters[1];
		set => parameters[1] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Max"/> function with the parameters set to <see langword="null"/>.
	/// </summary>
	public Max() : base([null!, null!]) { }

	/// <summary>
	/// Constructs a <see cref="Max"/> function with the <paramref name="first"/> and <paramref name="second"/> values.
	/// </summary>
	/// <param name="first">The first parameter of the function.</param>
	/// <param name="second">The second parameter of the function.</param>
	public Max(ValueHolder<T> first, ValueHolder<T> second) : base([first, second]) { }

	#endregion

	#region Public methods

	public override T GetValue() => First.GetValue() > Second.GetValue() ? First.GetValue() : Second.GetValue();

	public override string Sign() => "max";

	#endregion
}

/// <summary>
/// Represents a minimum value function in an expression.
/// </summary>
public sealed class Min<T> : Function<T> where T : IComparisonOperators<T, T, bool>
{
	#region Properties

	/// <summary>
	/// Gets or sets the first parameter of the function.
	/// </summary>
	public ValueHolder<T> First
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	/// <summary>
	/// Gets or sets the second parameter of the function.
	/// </summary>
	public ValueHolder<T> Second
	{
		get => parameters[1];
		set => parameters[1] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Min"/> function with the parameters set to <see langword="null"/>.
	/// </summary>
	public Min() : base([null!, null!]) { }

	/// <summary>
	/// Constructs a <see cref="Min"/> function with the <paramref name="first"/> and <paramref name="second"/> values.
	/// </summary>
	/// <param name="first">The first parameter of the function.</param>
	/// <param name="second">The second parameter of the function.</param>
	public Min(ValueHolder<T> first, ValueHolder<T> second) : base([first!, second!]) { }

	#endregion

	#region Public methods

	public override T GetValue() => First.GetValue() < Second.GetValue() ? First.GetValue() : Second.GetValue();

	public override string Sign() => "min";

	#endregion
}