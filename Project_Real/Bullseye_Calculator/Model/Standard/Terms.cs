using Project_Real;

namespace Bullseye_Calculator.Model.Standard;

/// <summary>
/// Represents a number in an expression.
/// </summary>
public sealed class Number : Term<Rational>
{
	#region Fields

	private readonly string token;

	#endregion

	#region Properties

	/// <returns>The token used to represent <see langword="this"/> object.</returns>
	public string Token => token;

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Number"/> by a <see cref="string"/>.
	/// </summary>
	/// <param name="token">The value of the <see cref="Number"/> as a text.</param>
	public Number(string token) => this.token = token;

	/// <summary>
	/// Constructs a <see cref="Number"/> by a <see cref="Rational"/>.
	/// </summary>
	/// <param name="value">The <see cref="Rational"/> representation of the <see cref="Number"/>.</param>
	public Number(Rational value) => token = value.ToString();

	#endregion

	#region Public methods

	public override Rational GetValue() => new(token);

	public override string ToStringByStep(ref int _) => ParenthesizeIfSigned(GetValue());

	#endregion
}

/// <summary>
/// Represents a pi value in an expression.
/// </summary>
public sealed class PI : Term<Rational>
{
	#region Constructors

	/// <summary>
	/// Constructs a <see cref="PI"/> object.
	/// </summary>
	public PI() { }

	#endregion

	#region Public methods

	public override Rational GetValue() => Rational.Pi();

	public override string ToStringByStep(ref int _) => "pi";

	#endregion
}

/// <summary>
/// Represents an e number value in an expression.
/// </summary>
public sealed class E : Term<Rational>
{
	#region Constructors

	/// <summary>
	/// Constructs an <see cref="E"/> number object.
	/// </summary>
	public E() { }

	#endregion

	#region Public methods

	public override Rational GetValue() => Rational.E();

	public override string ToStringByStep(ref int _) => "e";

	#endregion
}