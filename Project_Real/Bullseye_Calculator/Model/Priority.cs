namespace Bullseye_Calculator.Model;

/// <summary>
/// Describes the priority of a function.
/// </summary>
internal enum Priority
{
	None,
	BinaryOperatorFirstClass,
	BinaryOperatorSecondClass,
	BinaryOperatorThirdClass,
	UnaryOperator,
	Function
}