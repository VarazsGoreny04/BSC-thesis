using Project_Real;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bullseye_Calculator.Model.Standard;

public static partial class Calculator
{
	private sealed class RegexToken(Regex pattern, Func<string, Expression> function)
	{
		private readonly Regex pattern = pattern;
		private readonly Func<string, Expression> function = function;

		public Regex Pattern => pattern;
		public Func<string, Expression> Function => function;
	}

	private static readonly Regex sWhitespace = new(@"\s+");
	private static readonly RegexToken[] regexTokens =
	[
		// Rational number
		new(null!, s => new Number(s)),
		// Function name
		new(new(@"^[\p{Ll}\p{Lu}]+$"), _ => new PI()),
		// Operators
		new(new(@"^\+$"), _ => new Add()),
		new(new(@"^\-$"), _ => new Subtract()),
		new(new(@"^\*$"), _ => new Multiply()),
		new(new(@"^\/$"), _ => new Divide()),
		new(new(@"^\^$"), _ => new Power()),
		new(new(@"^\|$"), _ => new Root()),
		new(new(@"^\($"), _ => new OpeningParenthesis()),
		new(new(@"^\)$"), _ => new ClosingParenthesis()),
		/*new(new("^==$"), Function.GetTree), 
		new(new("^<=$"), Function.GetTree), 
		new(new("^>=$"), Function.GetTree), 
		new(new("^<$"), Function.GetTree), 
		new(new("^>$"), Function.GetTree),
		new(new("^=$"), Function.GetTree)*/
	];

	private static RegexToken[] Tokens
	{
		get
		{
			regexTokens[0] = new(new($"^\\p{{Nd}}+[{(Rational.Separator is '.' ? @"\." : Rational.Separator)}]?\\p{{Nd}}*$"), s => new Number(s));
			return regexTokens;
		}
	}

	public static ValueHolder Evaluate(string input)
	{
		ValueHolder result = TreeForm(PostfixForm(Parse(RemoveWhitespaces(input))));

		return result.ToString() == input ? result : throw new FormatException("Could not understand input.");
	}

	public static string RemoveWhitespaces(string input) => sWhitespace.Replace(input, "");

	public static List<Expression> Parse(string whitespacelessInput)
	{
		RegexToken[] tokens = Tokens;
		List<Expression> result = [];
		string lastSequence = string.Empty;
		string currentSequence = string.Empty;
		RegexToken? lastToken = null;
		RegexToken? currentToken;

		foreach (char letter in whitespacelessInput)
		{
			currentSequence = lastSequence + letter;

			currentToken = tokens.FirstOrDefault(token => token.Pattern.IsMatch(currentSequence));

			if (currentToken is not null)
			{
				lastSequence = currentSequence;
				lastToken = currentToken;
			}
			else
			{
				if (lastToken is null)
					throw new FormatException("Invalid token found.");

				result.Add(lastToken.Function.Invoke(lastSequence));

				lastSequence = letter.ToString();
				lastToken = tokens.FirstOrDefault(token => token.Pattern.IsMatch(lastSequence));
			}
		}

		if (lastToken is not null)
			result.Add(lastToken.Function.Invoke(lastSequence));
		else
			throw new FormatException("Invalid token found.");

		return result;
	}

	public static List<Expression> PostfixForm(List<Expression> unordered)
	{
		List<Expression> result = [];
		Stack<Expression> functions = new();

		unordered.ForEach(expression => expression.AcceptPostfix(ref functions, ref result));

		result.AddRange(functions);

		return result;
	}

	internal static void VisitPostfix(ref Stack<Expression> _, ref List<Expression> result, Term t) => result.Add(t);

	internal static void VisitPostfix(ref Stack<Expression> functions, ref List<Expression> result, OpeningParenthesis op)
	{
		functions.Push(op);
		result.Add(op);
	}

	internal static void VisitPostfix(ref Stack<Expression> functions, ref List<Expression> result, ClosingParenthesis cp)
	{
		if (!functions.Any(f => f is OpeningParenthesis))
			throw new FormatException();

		Expression e;

		while ((e = functions.Pop()) is not OpeningParenthesis)
			result.Add(e);

		result.Add(cp);
	}

	internal static void VisitPostfix(ref Stack<Expression> functions, ref List<Expression> result, Operator f)
	{
		if (functions.FirstOrDefault() is Operator o && o.Order() >= f.Order())
		{
			functions.Pop();

			result.Add(o);
		}

		functions.Push(f);
	}

	public static ValueHolder TreeForm(List<Expression> ordered)
	{
		Stack<Expression> result = new();
		
		ordered.ForEach(expression => expression.AcceptTree(ref result));

		return result.FirstOrDefault() as ValueHolder ?? throw new FormatException();
	}

	internal static void VisitTree(ref Stack<Expression> result, Term t) => result.Push(t);

	internal static void VisitTree(ref Stack<Expression> result, OpeningParenthesis op) => result.Push(op);

	internal static void VisitTree(ref Stack<Expression> result, ClosingParenthesis _)
	{
		Stack<ValueHolder> temp = new();

		while (result.TryPop(out Expression? expression) && expression is not OpeningParenthesis)
		{
			if (expression is ValueHolder valueHolder)
				temp.Push(valueHolder);
			else
				throw new FormatException();
		}

		if (temp.Count == 1)
			result.Push(new Parenthesized(temp.Pop()));
		else
			throw new FormatException();
	}

	internal static void VisitTree(ref Stack<Expression> result, Operator f)
	{
		int length = Math.Min(f.Parameters.Length, result.Count);

		for (int i = 1; i <= length && result.Peek() is ValueHolder valueHolder; ++i)
		{
			result.Pop();
			f.Parameters[^i] = valueHolder;
		}

		result.Push(f);
	}

	internal static void VisitTree(ref Stack<Expression> result, Parenthesized p) => throw new NotImplementedException();

	public static List<(string Calculation, string State)> FullEvaluation(ValueHolder root)
	{
		List<(string, string)> result = [];
		int step = 1;

		root.FullEvaluation(ref result, root, ref step);

		return result;
	}
}