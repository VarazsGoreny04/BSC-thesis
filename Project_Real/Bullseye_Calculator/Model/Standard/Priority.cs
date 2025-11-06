namespace Bullseye_Calculator.Model.Standard;

internal enum Priority
{
	None,
	BinaryOperatorFirstClass,
	BinaryOperatorSecondClass,
	BinaryOperatorThirdClass,
	UnaryOperator,
	Function
}