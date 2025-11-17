using Project_Real;

namespace Bullseye_Calculator.Model.Standard;

public sealed class Number(string token) : Term<Rational>
{
	private readonly string token = token;
	public Rational? value = null;

	public string Token => token;

	public override Rational GetValue() => value ??= new(token);
	public override string ToStringByStep(ref int _) => ParenthesizeIfSigned(Value);
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