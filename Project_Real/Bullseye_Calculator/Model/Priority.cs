namespace Bullseye_Calculator.Model;

internal enum Priority
{
	None,
	BinaryOperatorFirstClass,
	BinaryOperatorSecondClass,
	BinaryOperatorThirdClass,
	UnaryOperator,
	Function
}