using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bullseye_Calculator.Model;

/// <summary>
/// Represents an abstract calculator.
/// </summary>
public abstract partial class Calculator
{
	#region GeneratedRegex

	[GeneratedRegex(@"^\p{Nd}+\.?\p{Nd}*$")]
	protected static partial Regex NumberRegex();

	[GeneratedRegex(@"^\w+$")]
	protected static partial Regex FunctionNameRegex();

	[GeneratedRegex(@"^\+$")]
	protected static partial Regex AddRegex();

	[GeneratedRegex(@"^-$")]
	protected static partial Regex SubtractRegex();

	[GeneratedRegex(@"^\*$")]
	protected static partial Regex MultiplyRegex();

	[GeneratedRegex(@"^/$")]
	protected static partial Regex DivideRegex();

	[GeneratedRegex(@"^\^$")]
	protected static partial Regex PowerRegex();

	[GeneratedRegex(@"^\|$")]
	protected static partial Regex RootRegex();

	[GeneratedRegex(@"^\($")]
	protected static partial Regex OpeningParenthesisRegex();

	[GeneratedRegex(@"^\)$")]
	protected static partial Regex ClosingParenthesisRegex();

	[GeneratedRegex(@"^,$")]
	protected static partial Regex ComaRegex();

	#endregion

	#region Token types

	/// <summary>
	/// Basically a tuple with two components: a <see cref="Regex"/> pattern and a function.
	/// </summary>
	protected sealed class RegexToken
	{
		#region Fields

		private readonly Regex pattern;
		private readonly Func<string, Expression> function;

		#endregion

		#region Properties

		/// <returns>The <see cref="Regex"/> pattern of the token.</returns>
		public Regex Pattern => pattern;

		/// <returns>The function of the token.</returns>
		public Func<string, Expression> Function => function;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a <see cref="RegexToken"/> with a <paramref name="pattern"/> and a <paramref name="function"/>.
		/// </summary>
		/// <param name="pattern">The pattern of the token.</param>
		/// <param name="function">The function of the token.</param>
		public RegexToken(Regex pattern, Func<string, Expression> function)
		{
			this.pattern = pattern;
			this.function = function;
		}

		#endregion
	}

	/// <summary>
	/// Basically a tuple with two components: a <see cref="Regex"/> pattern and a function.
	/// </summary>
	protected sealed class FunctionToken
	{
		#region Fields

		private readonly string name;
		private readonly Func<Expression> function;

		#endregion

		#region Properties

		/// <returns>The name of the token.</returns>
		public string Name => name;

		/// <returns>The function of the token.</returns>
		public Func<Expression> Function => function;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a <see cref="FunctionToken"/> with a <paramref name="name"/> and a <paramref name="function"/>.
		/// </summary>
		/// <param name="name">The name of the token.</param>
		/// <param name="function">The function of the token.</param>
		public FunctionToken(string name, Func<Expression> function)
		{
			this.name = name;
			this.function = function;
		}

		#endregion
	}

	#endregion

	#region Fields

	protected readonly RegexToken[] regexTokens;

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a calculator with the given <paramref name="regexTokens"/>.
	/// </summary>
	/// <param name="regexTokens">The tokens recognizable by the calculator.</param>
	protected Calculator(RegexToken[] regexTokens) => this.regexTokens = regexTokens;

	#endregion

	#region Protected methods

	/// <summary>
	/// Removes every whitespace character from the given <paramref name="input"/>.
	/// </summary>
	/// <param name="input">The <see cref="string"/> to remove the characters from.</param>
	/// <returns>The <paramref name="input"/> without whitespaces.</returns>
	protected static string RemoveWhitespaces(string input) => new([.. input.Where(c => !char.IsWhiteSpace(c))]);

	/// <summary>
	/// Searches for the function in the <paramref name="functionTokens"/> by the given function <paramref name="name"/>.
	/// </summary>
	/// <param name="functionTokens">The functions.</param>
	/// <param name="name">The function name to search for.</param>
	/// <returns>The found function represented by an <see cref="Exception"/> object.</returns>
	/// <exception cref="FormatException">There must be a matching function name amongst the <paramref name="functionTokens"/>.</exception>
	protected static Expression GetFunctionByName(FunctionToken[] functionTokens, string name)
	{
		return functionTokens.FirstOrDefault(f => f.Name == name)?.Function.Invoke() ?? throw new FormatException("Unrecognizable function name found.");
	}

	/// <summary>
	/// Parses the input and creates an <see cref="Expression"/> list from it by the given <see cref="Calculator"/>.
	/// </summary>
	/// <param name="whitespacelessInput">The input without any whitespace characters.</param>
	/// <param name="calculator">The used <see cref="Calculator"/>.</param>
	/// <returns>The list of <see cref="Expression"/>s representing the input.</returns>
	/// <exception cref="FormatException">The input cannot be parsed properly.</exception>
	protected static List<Expression> Parse(string whitespacelessInput, Calculator calculator)
	{
		RegexToken[] tokens = calculator.regexTokens;
		List<Expression> result = [];
		string lastSequence = string.Empty;
		string currentSequence = string.Empty;
		RegexToken? lastToken = null;
		RegexToken? currentToken;

		foreach (char letter in whitespacelessInput)
		{
			currentSequence = lastSequence + letter;

			currentToken = tokens.FirstOrDefault(token => token.Pattern.IsMatch(currentSequence));

			if (currentToken is RegexToken)
			{
				lastSequence = currentSequence;
				lastToken = currentToken;
			}
			else if (lastToken is RegexToken)
			{
				result.Add(lastToken.Function.Invoke(lastSequence));

				lastSequence = letter.ToString();
				lastToken = tokens.FirstOrDefault(token => token.Pattern.IsMatch(lastSequence));
			}
			else
				lastSequence = currentSequence;
		}

		if (lastToken is RegexToken)
			result.Add(lastToken.Function.Invoke(lastSequence));
		else
			throw new FormatException("Invalid token found.");

		return result;
	}

	/// <summary>
	/// Transforms the given <paramref name="unordered"/> <see cref="Expression"/> list into postfix form.
	/// </summary>
	/// <param name="unordered">The input in parsed form.</param>
	/// <returns>The postfix form of the list.</returns>
	protected static List<Expression> PostfixForm(List<Expression> unordered)
	{
		List<Expression> result = [];
		Stack<Expression> functions = new();

		unordered.ForEach(expression => expression.ToPostfix(ref functions, ref result));

		result.AddRange(functions);

		return result;
	}

	/// <summary>
	/// Transforms the given <paramref name="ordered"/> <see cref="Expression"/> list into tree form.
	/// </summary>
	/// <typeparam name="T">The type of value in the <see cref="ValueHolder{T}"/>.</typeparam>
	/// <param name="ordered">The input in postfix form.</param>
	/// <returns>The tree form of the list.</returns>
	/// <exception cref="FormatException">The input cannot be turned into an <see cref="Expression"/> tree properly.</exception>
	protected static ValueHolder<T> TreeForm<T>(List<Expression> ordered)
	{
		Stack<Expression> result = new();

		ordered.ForEach(expression => expression.ToTree(ref result));

		return result.FirstOrDefault() as ValueHolder<T> ?? throw new FormatException("Could not understand input.");
	}

	/// <summary>
	/// Evaluates the given <see cref="Expression"/> tree and returns the partial results in a list. The final result is the last node of the list.
	/// </summary>
	/// <typeparam name="T">The type of value in the <see cref="ValueHolder{T}"/>.</typeparam>
	/// <param name="root">The root node of the <see cref="Expression"/> tree.</param>
	/// <returns>The list of partial values.</returns>
	protected static List<(string Calculation, string State)> FullEvaluation<T>(ValueHolder<T> root)
	{
		List<(string, string)> result = [];
		int step = 1;

		root.FullEvaluation(ref result, root, ref step);

		return result;
	}

	#endregion

	#region Internal methods

	/// <summary>
	/// Evaluates the given input and creates an <see cref="Expression"/> tree from it.
	/// </summary>
	/// <typeparam name="T">The type of value in the <see cref="ValueHolder{T}"/>.</typeparam>
	/// <param name="input">The given input.</param>
	/// <param name="calculator">The used <see cref="Calculator"/>.</param>
	/// <returns>The root node of the <see cref="Expression"/> tree.</returns>
	/// <exception cref="FormatException">The result and the input must match to confirm that the calculator understood the input properly.</exception>
	internal static ValueHolder<T> Evaluate<T>(string input, Calculator calculator)
	{
		input = RemoveWhitespaces(input);

		ValueHolder<T> result = TreeForm<T>(PostfixForm(Parse(input, calculator)));

		return result.ToString() == input ? result : throw new FormatException("Could not understand input.");
	}

	#endregion

	#region Public methods

	/// <summary>
	/// Evaluates the given <paramref name="input"/> and returns the partial results in a list. The final result is the last node of the list.
	/// </summary>
	/// <param name="input">The given input.</param>
	/// <returns>The list of partial values.</returns>
	public abstract List<(string Calculation, string State)> FullEvaluation(string input);

	#endregion
}