using System.Collections.Generic;
using System.Linq;

namespace Bullseye_Calculator.Model;

public abstract class Function<T>(int parameters) : FunctionBase<T>(parameters)
{
	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<T> root, ref int step)
	{
		++step;

		foreach (ValueHolder<T> parameter in parameters)
			parameter?.FullEvaluation(ref partialValues, root, ref step);

		int stepCopy = step;

		partialValues.Add(($"{Sign()}({string.Join(", ", parameters.Select(p => p.Value))}) = {Value}", root.ToStringByStep(ref stepCopy)));
	}
	public override string ToStringByStep(ref int step)
	{
		string[] arguments = new string[parameters.Length];

		for (int i = 0; i < parameters.Length; ++i)
			arguments[i] = parameters[i].ToStringByStep(ref step);

		return --step <= 0 ? $"{Sign()}({string.Join("; ", arguments)})" : $"({Value})";
	}

	internal override void ToTree(ref Stack<Expression> result)
	{
		for (int i = 1; i <= Parameters.Length && result.Peek() is Parenthesized<T> parenthesized; ++i)
		{
			result.Pop();

			Parameters[^i] = parenthesized.Content;
		}

		result.Push(this);
	}

	internal override Priority Order() => Priority.Function;
}