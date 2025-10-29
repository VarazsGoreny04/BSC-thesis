using Project_Real;

namespace Bullseye_Calculator.Model.Standard;

public sealed class Number(string token) : Term
{
	private readonly string token = token;
	public Rational? value = null;

	public string Token => token;

	public override Rational GetValue() => value ??= new(token);
	public override string ToStringByStep(ref int _) => ParenthesizeIfSigned(Value);
}