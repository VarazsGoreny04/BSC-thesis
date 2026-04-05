namespace Calculators;

/// <summary>
/// Describes the priority of a function.
/// </summary>
internal enum Priority
{
	None,
	FirstClass,
	SecondClass,
	ThirdClass,
	UnaryOperator,
	Function
}