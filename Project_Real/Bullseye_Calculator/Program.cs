using Bullseye_Calculator.Model.Standard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bullseye_Calculator;

public class Program
{
	public static void Main()
	{
		/*Rational[,] a = new Rational[10, 10];

		for (int i = 0; i < 100; ++i)
			a[i / 10, i % 10] = Convert.ToString(i + 1);

		Console.WriteLine(Matrix.ToString(a));*/

		/*long iterations = 1;
		Stopwatch sw = Stopwatch.StartNew();
		long avg = 0;

		for (int i = 0; i < iterations; ++i)
		{
			Rational.FractionCalculatonLength = 3;

			Point2D[] myPoints = [
				new("1", "3"),
				new("2", "6"),
				new("3", "1"),
				new("4", "5"),
				new("5", "4"),
				new("6", "-47")
			];

			Rational[] res = Interpolation.Lagrange(myPoints);
			Console.WriteLine(Polynom.ToString(res));

			Rational.FractionFormat = false;

			Rational[] points = CoordinateSystem.LinSpace("1", "6", 100);
			//Console.WriteLine(Matrix.ToString(Polynom.EvaluateRange(res, points)));

			sw.Stop();
			avg += sw.ElapsedTicks;
			sw.Restart();
		}

		Console.WriteLine(avg / iterations);*/

		string a = "1", b = "3", c = "-4";
		string text = $"(-({b})+2|(({b})^2-4*({a})*({c})))/(2*({a}))";
		//string text = $"pi/pi";
		//string text = $"pi/pi";
		/*List<Expression> parsed = Calculator.Parse(text);

		Console.WriteLine(text);

		Console.WriteLine(Matrix.ToString(parsed.ToArray()));

		List<Expression> prefixed = Calculator.PostfixForm(parsed);

		Console.WriteLine(Matrix.ToString(prefixed.ToArray()));

		ValueHolder result = Calculator.TreeForm(prefixed);

		Console.WriteLine(text);
		Console.WriteLine(result.ToString());
		Console.WriteLine(result.Value().ToString());*/

		ValueHolder valueHolder = Calculator.Evaluate(text);
		List<(string Calculation, string State)> evaluation = Calculator.FullEvaluation(valueHolder);

		Console.WriteLine($"{valueHolder} == {valueHolder.Value}");
		int maxLength = evaluation.Max(step => step.Calculation.Length);
		evaluation.ForEach(step => Console.WriteLine($"{step.Calculation}{new string(' ', maxLength - step.Calculation.Length)}\t{step.State}"));

		/*for (int i = 1; i < 12; ++i)
		{
			int temp = i;
			Console.WriteLine(valueHolder.StepToString(ref temp));
		}*/
	}
}