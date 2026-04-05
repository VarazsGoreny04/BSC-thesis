using Calculators;
using Calculators.EuclideanSpace;
using Calculators.Standard;
using ProjectReal.Number;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculators;

public class Program
{
	public static void Main()
	{
		Calculator.FunctionToken<Matrix<Rational>>[] euclideanSpaceFunctionTokens = [
			new("diag", () => new Diagonalize<Rational>()),
			new("inv", () => new Inverse<Rational>())
		];

		Calculator calculator = new StandardCalculator<Rational>([]);

		/*Rational[,] a = new Rational[10, 10];

		for (int i = 0; i < 100; ++i)
			a[i / 10, i % 10] = Convert.ToString(i + 1);

		Console.WriteLine(Matrix.ToString(a));*/

		/*long iterations = 1;
		Stopwatch sw = Stopwatch.StartNew();
		long avg = 0;

		for (int i = 0; i < iterations; ++i)
		{
			Rational.fractionCalculationLength = 3;

			Point2D[] myPoints = [
				new("1", "3"),
				new("2", "6"),
				new("3", "1"),
				new("4", "5"),
				new("5", "4"),
				new("6", "-47")
			];

			Rational[] res = Interpolation.Lagrange(myPoints);
			Console.WriteLine(Polynomial.ToString(res));

			Rational.FractionFormat = false;

			Rational[] points = CoordinateSystem.LinSpace("1", "6", 100);
			//Console.WriteLine(Matrix.ToString(Polynomial.EvaluateRange(res, points)));

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

		/*ValueHolder valueHolder = Calculator.Evaluate(text, calculator);
		List<(string Calculation, string State)> evaluation = Calculator.FullEvaluation(valueHolder);

		Console.WriteLine($"{valueHolder} == {valueHolder.Value}");
		int maxLength = evaluation.Max(step => step.Calculation.Length);
		evaluation.ForEach(step => Console.WriteLine($"{step.Calculation}{new string(' ', maxLength - step.Calculation.Length)}\t{step.State}"));*/

		/*for (int i = 1; i < 12; ++i)
		{
			int temp = i;
			Console.WriteLine(valueHolder.StepToString(ref temp));
		}*/

		/*Rational.FractionCalculationLength = 3;
		Rational.FractionalFormat = true;

		Point2D[] myPoints = [
			new("1", "8"),
			new("2", "5"),
			new("3", "3"),
			new("4", "9"),
			//new(Rational.Pi(), "1")
		];

		Rational[] res = Interpolation.Lagrange(myPoints);
		Console.WriteLine(Polynomial.ToString(res));*/

		/*Rational[] points = CoordinateSystem.LinSpace("1", "6", 100);
		Console.WriteLine(Matrix.ToString(Polynomial.EvaluateRange(res, points)));*/

		/*Rational.WriteSign = false;

		Matrix l0 = new(
			"1;0;0&" +
			"3;1;0&" +
			"9;-4;1"
			);
		Matrix u0 = new(
			"5;2;4&" +
			"0;5;-2&" +
			"0;0;2"
			);*/
		/*Matrix m = Matrix.Product(Matrix.ToRationalMatrix(l0.GetValue()), Matrix.ToRationalMatrix(u0.GetValue()));
		//Matrix m = new Matrix(
		//	"1;2;3&" +
		//	"3;6;9&" +
		//	"2;4;6"
		//	);

		Console.WriteLine(Matrix.ToString(m.GetValue()));

		Rational[,] lu = Matrix.GaussianElimination(Matrix.ToRationalMatrix(m.GetValue())).EliminatedMatrix;
		(Rational[,] l, Rational[,] u) = Matrix.LUDecomposition(Matrix.ToRationalMatrix(m.GetValue()));

		Console.WriteLine($"{Matrix.ToString(lu)} = {Matrix.ToString(l)} * {Matrix.ToString(u)} = {Matrix.ToString(Matrix.Product(l, u))}");

		Console.WriteLine(m.ToString());

		Console.WriteLine(Matrix.Determinant(Matrix.ToRationalMatrix(m.GetValue())));*/

		/*ValueHolder<Rational> v = new Derivative(new Model.Standard.Add(new Multiply(new Number("5"), new X()), new Number("3")));
		Calculator calc = new DerivativeCalculator();*/

		/*List<ValueHolder<Rational>> valueHolders =
		[
			new Parenthesized<Rational>(new Number(new Rational("3"))),
			new Add(new Number(new Rational("7")), new Number(new Rational("4")))
		];

		valueHolders.ForEach(v => Console.WriteLine(v.Simplify()));*/

		/*StandardCalculator<int> calculator1 = new();

		Console.WriteLine(calculator1.FullEvaluation("3+1").LastOrDefault().State);*/
	}
}