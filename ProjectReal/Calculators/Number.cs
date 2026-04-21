namespace Calculators;

/// <summary>
/// Represents a number.
/// </summary>
/// <typeparam name="T">The type of the term.</typeparam>
public sealed class Number<T> : Term<T>
{
	#region Fields

	private readonly T value;

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Number{T}"/> by a <paramref name="value"/>.
	/// </summary>
	/// <param name="value">The representation of the <see cref="Number{T}"/>.</param>
	public Number(T value) => this.value = value;

	#endregion

	#region Public methods

	public override T GetValue() => value;

	public override string ToStringByStep(ref int _) => ParenthesizeIfSigned(value);

	#endregion
}