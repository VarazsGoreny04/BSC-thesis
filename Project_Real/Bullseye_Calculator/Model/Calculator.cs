using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Bullseye_Calculator.Model;

public abstract partial class Calculator
{
	#region GeneratedRegex

	[GeneratedRegex(@"\s+")]
	protected static partial Regex WhitespaceRegex();

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

	protected sealed class RegexToken(Regex pattern, Func<string, Expression> function)
	{
		private readonly Regex pattern = pattern;
		private readonly Func<string, Expression> function = function;

		public Regex Pattern => pattern;
		public Func<string, Expression> Function => function;
	}

	protected sealed class FunctionToken(string name, Func<Expression> function)
	{
		private readonly string name = name;
		private readonly Func<Expression> function = function;

		public string Name => name;
		public Func<Expression> Function => function;
	}

	#endregion

	protected readonly RegexToken[] regexTokens;

	protected Calculator(RegexToken[] regexTokens) => this.regexTokens = regexTokens;

	public abstract List<(string Calculation, string State)> FullEvaluation(string input);

	public static ValueHolder<T> Evaluate<T>(string input, Calculator calculator)
	{
		ValueHolder<T> result = TreeForm<T>(PostfixForm(Parse(RemoveWhitespaces(input), calculator)));

		return result.ToString() == input ? result : throw new FormatException("Could not understand input.");
	}

	protected static string RemoveWhitespaces(string input) => WhitespaceRegex().Replace(input, "");

	protected static Expression GetFunctionByName(FunctionToken[] functionTokens, string name) => functionTokens.First(f => f.Name == name).Function.Invoke();

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

		if (lastToken is not null)
			result.Add(lastToken.Function.Invoke(lastSequence));
		else
			throw new FormatException("Invalid token found.");

		return result;
	}

	protected static List<Expression> PostfixForm(List<Expression> unordered)
	{
		List<Expression> result = [];
		Stack<Expression> functions = new();

		unordered.ForEach(expression => expression.ToPostfix(ref functions, ref result));

		result.AddRange(functions);

		return result;
	}
	protected static ValueHolder<T> TreeForm<T>(List<Expression> ordered)
	{
		Stack<Expression> result = new();

		ordered.ForEach(expression => expression.ToTree(ref result));

		Debug.WriteLine(result.FirstOrDefault()?.GetType());

		return result.FirstOrDefault() as ValueHolder<T> ?? throw new FormatException();
	}

	public static List<(string Calculation, string State)> FullEvaluation<T>(ValueHolder<T> root)
	{
		List<(string, string)> result = [];
		int step = 1;

		root.FullEvaluation(ref result, root, ref step);

		return result;
	}
}