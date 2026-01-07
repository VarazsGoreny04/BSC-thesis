using Project_Real;

namespace Bullseye_Calculator.Model.Standard;

public sealed class Number : Term<Rational>
{
	private readonly string token;
	public Rational? value = null;

	public string Token => token;

	public Number(string token) => this.token = token;

	public Number(Rational value)
	{
		token = value.ToString();
		this.value = value;
	}

	public override Rational GetValue() => value ??= new(token);
	public override string ToStringByStep(ref int _) => ParenthesizeIfSigned(GetValue());
}

public sealed class PI : Term<Rational>
{
	public override Rational GetValue() => Rational.Pi();
	public override string ToStringByStep(ref int _) => "pi";
}

public sealed class E : Term<Rational>
{
	public override Rational GetValue() => Rational.E();
	public override string ToStringByStep(ref int _) => "e";
}