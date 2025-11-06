using Project_Real;
using Bullseye_Calculator.Model.Standard;
using System.Text.RegularExpressions;

namespace Bullseye_Calculator.Model;

public abstract partial class Calculator
{
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

	protected readonly RegexToken[] regexTokens;

	protected RegexToken[] Tokens
	{
		get
		{
			regexTokens[0] = new(new($"^\\p{{Nd}}+[{(Rational.Separator is '.' ? @"\." : Rational.Separator)}]?\\p{{Nd}}*$"), value => new Number(value));
			return regexTokens;
		}
	}

	protected Calculator(RegexToken[] regexTokens) => this.regexTokens = regexTokens;

	public static ValueHolder Evaluate(string input, Calculator calculator)
	{
		ValueHolder result = TreeForm(PostfixForm(Parse(RemoveWhitespaces(input), calculator)));

		return result.ToString() == input ? result : throw new FormatException("Could not understand input.");
	}

	protected static string RemoveWhitespaces(string input) => WhitespaceRegex().Replace(input, "");

	protected static Expression GetFunctionByName(FunctionToken[] functionTokens, string name) => functionTokens.First(f => f.Name == name).Function.Invoke();

	protected static List<Expression> Parse(string whitespacelessInput, Calculator calculator)
	{
		RegexToken[] tokens = calculator.Tokens;
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

	protected static List<Expression> PostfixForm(List<Expression> unordered)
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

	internal static void VisitPostfix(ref Stack<Expression> functions, ref List<Expression> result, Operator o)
	{
		if (functions.FirstOrDefault() is FunctionBase fB && fB.Order() >= o.Order())
		{
			functions.Pop();

			result.Add(fB);
		}

		functions.Push(o);
	}

	internal static void VisitPostfix(ref Stack<Expression> functions, ref List<Expression> result, Function f)
	{
		if (functions.FirstOrDefault() is FunctionBase fB && fB.Order() >= f.Order())
		{
			functions.Pop();

			result.Add(fB);
		}

		functions.Push(f);
	}

	internal static void VisitPostfix(ref Stack<Expression> _, ref List<Expression> result, Coma c) => result.Add(c);

	protected static ValueHolder TreeForm(List<Expression> ordered)
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

	internal static void VisitTree(ref Stack<Expression> result, Operator o)
	{
		int length = Math.Min(o.Parameters.Length, result.Count);

		for (int i = 1; i <= length && result.Peek() is ValueHolder valueHolder; ++i)
		{
			result.Pop();

			o.Parameters[^i] = valueHolder;
		}

		result.Push(o);
	}

	internal static void VisitTree(ref Stack<Expression> result, Function f)
	{
		for (int i = 1; i <= f.Parameters.Length && result.Peek() is Parenthesized parenthesized; ++i)
		{
			result.Pop();

			f.Parameters[^i] = parenthesized.Content;
		}

		result.Push(f);
	}

	internal static void VisitTree(ref Stack<Expression> result, Coma _)
	{
		VisitTree(ref result, new ClosingParenthesis());
		VisitTree(ref result, new OpeningParenthesis());
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