namespace ProjectReal.NumberSet;

/// <summary>Defines a mechanism for computing the power of two values.</summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
/// <typeparam name="TOther">The type that represents the exponent to <typeparamref name="TSelf" />.</typeparam>
/// <typeparam name="TResult">
/// The type that contains the result of <typeparamref name="TSelf" /> raised to <typeparamref name="TOther" />.
/// </typeparam>
public interface IPowerOperations<TSelf, TOther, TResult> where TSelf : IPowerOperations<TSelf, TOther, TResult>?
{
	/// <summary>Computes the power of the two values. The operator is left associative.</summary>
	/// <param name="left">The base value.</param>
	/// <param name="right">The exponent value.</param>
	/// <returns>The value of <paramref name="left" /> raised to <paramref name="right" />.</returns>
	static abstract TResult operator ^(TSelf left, TOther right);
}