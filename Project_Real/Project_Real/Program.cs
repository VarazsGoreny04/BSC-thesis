using System;
using System.Diagnostics;

namespace Project_Real;

public class Program
{
	public static void Main()
	{
		Stopwatch timer = new();

		/*CultureInfo.CurrentCulture = new CultureInfo("en-us");*/

		/*string num = "+134.15";
		Real f = num;

		Console.WriteLine(Real.MAXWHOLEINT);
		Console.WriteLine(Real.MAXFRACTIONINT);
		Console.WriteLine(Real.FRACTIONLENGTH);
		Console.WriteLine(string.Join(',', Real.HUNDRED));

		Console.WriteLine(float.Parse(num));
		Console.WriteLine(f.ToBinaryString());
		Console.WriteLine(f.ToString());

		bool[] a = [true, false, true, false, false, false, false, true];
		bool[] b = [false, true, true, true, false, false, true, false];
		bool[] c = [false, false, true, false, false, true, false, false];

		Console.WriteLine(Real.ToUInt(c));
		Console.WriteLine(Real.ToUInt(b));
		Console.WriteLine("-" + Real.ToUInt(Real.BitAbs(Real.BitSubtract(c, b, false).Item2)));*/

		/*Real left = "-5.3";
		Real right = "+3.6";
		Console.WriteLine(right - left);
		Console.WriteLine();

		Real left = "+0.1";
		Real right = "+0.2";
		Real f3 = "+0.3";
		double f4 = 0.1;
		double f5 = 0.2;
		double f6 = 0.3;
		Console.WriteLine($" ({f4})  +  ({f5})  =  ({f6})  ->  ({f4})  +  ({f5})  ==  ({f4 + f5}) => {f4 + f5 == f6}");
		Console.WriteLine($"({left}) + ({right}) = ({f3}) -> ({left}) + ({right}) == ({left + right})            => {left + right == f3}");


		bool[] b = [false, true, false, true, false, false, false, false];
		bool[] c = [true, true, false, false, false, false, false, false];*/

		//Console.WriteLine(string.Join(',', Real.BitMultiply(b, c)));
		//Console.ReadKey();
		//Console.WriteLine();

		/*Natural natural = "6999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999";
		double @double = 6999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999d;
		Natural nat2 = "128";

		Console.WriteLine("Double:");
		Console.WriteLine(@double);
		Console.WriteLine("(<double> == <double> + 128)  \t" + (@double == @double + 128));
		Console.WriteLine(@double + 128);
		Console.WriteLine();
		Console.WriteLine("Natural:");
		Console.WriteLine(natural);
		Console.WriteLine("(<natural> == <natural> + 128)\t" + (natural == natural + nat2));
		Console.WriteLine(Natural.Add(natural, nat2));
		Console.WriteLine();

		Console.WriteLine("Trim:");
		Console.WriteLine(Natural.TrimEnd(nat2 - (nat2 + new Natural(Digit.ONE))));
		Console.WriteLine(Natural.TrimEnd(new Natural("0001000")));
		Console.WriteLine();*/

		/*Console.WriteLine("Integer:");
		Integer i1 = "-100";
		Integer i2 = Digit.TWO;
		Console.WriteLine(i1 + i2);
		Console.WriteLine();*/

		/*Digit a = '9';
		Digit b = '9';
		(bool f, Digit c) = Digit.Subtract(a, b);
		Console.WriteLine(c);
		Console.WriteLine(f);*/

		/*bool[] a = Digit.BitMultiply([false, true, true, true], [true, true, false, false]);
		BoolMatrixToString(a);*/

		/*(bool[] a, bool[] b) = Digit.BitDivide([false, true, true, true], [true, true, false, false]);
		BoolMatrixToString(a);
		BoolMatrixToString(b);*/

		/*Digit b = new();
		Digit c = b;
		b = Digit.Add(b, new Digit('1')).Digit;
		Console.WriteLine(b);
		Console.WriteLine(c);*/

		/*BoolMatrixToString(Digit.BitSubtract([false, true, true, true] *//*2+4+8=14*//*, [true, true, false, false] *//*1+2=3*//*));
		(bool a, Digit b) = Digit.Subtract(new Digit('9'), new Digit('6'), true);
		//(Digit a, Digit b) = Digit.Divide(new Digit('9'), new Digit('3'));
		Console.WriteLine(a);
		Console.WriteLine(b);*/

		/*Console.WriteLine(new Natural([new Digit('0'), new Digit('0')]));
		Console.WriteLine(new Natural("00"));
		Console.WriteLine(new Natural(""));*/

		/*Console.WriteLine("Multiplication:");
		Console.ReadKey();
		Console.WriteLine(Integer.Multiply(new Integer("-35419993"), new Integer("1524315243"))); // -53,991,235,236,853,299*/

		/*(Natural w, Natural value) = Natural.Divide(new Natural("132"), new Natural(Digit.FOUR));
		Console.WriteLine(w);
		Console.WriteLine(value);*/

		/*Console.WriteLine("Division:");
		Console.WriteLine(new Integer("10") / new Integer(Digit.THREE));
		Console.WriteLine(new Integer("10") % new Integer(Digit.THREE));
		Console.WriteLine(new Integer("10") / new Integer("-3"));
		Console.WriteLine(new Integer("10") % new Integer("-3"));
		Console.WriteLine(new Integer("-10") / new Integer(Digit.THREE));
		Console.WriteLine(new Integer("-10") % new Integer(Digit.THREE));
		Console.WriteLine(new Integer("-10") / new Integer("-3"));
		Console.WriteLine(new Integer("-10") % new Integer("-3"));
		Console.WriteLine();*/

		/*Console.WriteLine("Float formats:");
		//double f = 0.; // hiba
		//double f = 0.; // hiba
		//double f = 0.f; // hiba
		double left = .0;
		double right = 0.0;
		Console.WriteLine(left);
		Console.WriteLine(right);*/

		//Writable w = new();
		/*Digit a = new Digit();
		Digit b = a;
		a = '5';
		Console.WriteLine(b);*/

		/*Console.WriteLine("Positive formats:");
		Positive p0 = new();
		//Positive phiba = new(".0"); hiba
		Positive p1 = new(Digit.ZERO);
		Positive p2 = new("010.");
		Positive p3 = new("00.00");
		Console.WriteLine(p0);
		Console.WriteLine(p1);
		Console.WriteLine(p2);
		Console.WriteLine(p3);
		Console.WriteLine();*/

		//Natural.Divide("25", Digit.TWO);

		//var p8 = Positive.Divide(new Positive("927"), new Positive("0.000"));

		//Console.WriteLine(Positive.Add(new Positive("6."), new Positive("1.3")));
		//Console.WriteLine(Positive.Subtract(new Positive("6."), new Positive("1.3")));
		//Console.WriteLine(Positive.Multiply(new Positive("6."), new Positive("1.3")));
		/*Console.WriteLine(Positive.Divide(new Positive("6."), new Positive("1.6")));
		Console.WriteLine(Positive.Divide(new Positive("3."), new Positive("0.6")));
		Console.WriteLine(Positive.Divide(new Positive("68.5"), new Positive("1.6")));*/

		/*var p1 = Positive.GetWhole(Positive.Divide(new Positive("2368.5456"), new Positive("12.67")).Value);
		var p2 = Positive.Divide(new Positive("23.68"), new Positive("2368.0000"));
		var p3 = Positive.Divide(new Positive("1423.927"), new Positive("2368.456"));
		var p4 = Positive.Divide(new Positive("0.927"), new Positive("0.000023456"));
		var p5 = Positive.Divide(new Positive("0.0927"), new Positive("0.000023456"));
		var p6 = Positive.Divide(new Positive("0.00927"), new Positive("0.000023456"));
		var p7 = Positive.Divide(new Positive("927.000001"), new Positive("0.00092700"));
		Console.WriteLine(p1);  // 186.	941247040
		Console.WriteLine(p2);  // 0.	01
		Console.WriteLine(p3);	// 0.	601204751
		Console.WriteLine(p4);	// 0.	601204751
		Console.WriteLine(p5);  // 0.	601204751
		Console.WriteLine(p6);  // 0.	601204751
		Console.WriteLine(p7);  // 0.	601204751*/

		/*Console.Write(p1.Item2);
		MatrixToString(p1.Item1);
		Console.Write(p2.Item2);
		MatrixToString(p2.Item1);
		Console.Write(p3.Item2);
		MatrixToString(p3.Item1);*/

		//Console.WriteLine(Natural.Power(Digit.SIX, Digit.TWO));
		//Console.WriteLine(Natural.Power("10", "100"));
		//Console.WriteLine(Natural.Power("645", "219"));
		//Console.WriteLine(Math.Pow(645, 219));

		//Console.WriteLine(Natural.SquareRoot("1522756"));
		/*Console.WriteLine(Natural.SquareRoot("1522756"));
		Console.WriteLine(Natural.SquareRoot("23189675626"));
		Console.WriteLine(Positive.SquareRoot("152.2756"));
		Console.WriteLine(Positive.SquareRoot("1522.756"));
		Console.WriteLine(Positive.SquareRoot("2318785.835536"));*/
		//Console.WriteLine(Positive.SquareRoot("100.0"));              // 10
		//Console.WriteLine(Positive.SquareRoot("4192"));               // 64.745656224954581890369881235962
		/*Console.WriteLine(Positive.Root("1522.756", Digit.TWO));              // 39.0225063264778009568663463383
		Console.WriteLine(Natural.Root("1522756000000000", Digit.TWO));       // 39.0225063264778009568663463383
		Console.WriteLine(Positive.Root("2318785.835536", Digit.TWO));
		Console.WriteLine(Positive.Root("1522.756", Digit.FOUR));
		Console.WriteLine(Natural.Root("1522756000", Digit.THREE));
		Console.WriteLine(Positive.Root("3530945043.777457217", Digit.THREE));
		Console.WriteLine(Natural.Root("3530945043777457217", Digit.THREE));*/
		//Console.WriteLine(Natural.Root("4192000000000", Digit.THREE));
		//Console.WriteLine(Natural.Root("18446744073709551615", Digit.TWO));
		//Console.WriteLine(Positive.Root("4192000000000", Digit.THREE));
		//Console.WriteLine(Math.Pow(4192000000000.0, 1.0/3.0));
		//Console.WriteLine(4192000000000.0 - Math.Pow(Math.Pow(4192000000000.0, 1.0/3.0), 3.0));
		/*Positive.FractionCalculationLength = 11;
		(Positive whole, Positive remainder) = Positive.Root("124.067", Digit.SIX);
		Console.WriteLine((whole, remainder));
		Console.WriteLine(whole ^ Digit.SIX);*/

		/*Rational a = "+65/3";
		Rational b = "-3/2";*/

		/*timer.Restart();
		Console.WriteLine(Positive.SquareRoot("10005", 512));
		timer.Stop();
		Console.WriteLine(timer.ToString());*/

		//for (int i = 1; i < 8; ++i)
		//{
		//	timer.Restart();
		//	int a = (int)Math.Pow(2, i);
		//	Positive.FractionCalculationLength = (int)(a * 1.2);
		//	Console.WriteLine($"{Rational.ToWritableString(Rational.PI_Chudnovsky(a))} - {a}");
		//	timer.Stop();
		//	Console.WriteLine(timer.ToString());
		//}

		/*for (int i = 1; i < 8; ++i)
		{
			timer.Restart();
			int a = (int)Math.Pow(2, i);
			Positive.FractionCalculationLength = (int)(a * 1.2);
			Console.WriteLine($"{Rational.ToWritableString(Rational.PI(a))} - {a}");
			timer.Stop();
			Console.WriteLine(timer.ToString());
		}

		for (int i = 1; i < 8; ++i)
		{
			timer.Restart();
			int a = (int)Math.Pow(2, i);
			Positive.FractionCalculationLength = (int)(a * 1.2);
			Console.WriteLine($"{Rational.ToWritableString(Rational.E(a))} - {a}");
			timer.Stop();
			Console.WriteLine(timer.ToString());
		}*/

		/*Positive n = Digit.THREE;

		Positive res = Positive.Log(n);

		Console.WriteLine(res);*/
		//Console.WriteLine(Positive.Power("10", res));


		/*for (int i = 1; i <= 10; i++)
		{
			Rational.FractionCalculationLength = i * 10;

			timer.Restart();

			Rational a = Rational.E();
			Console.Write(Rational.ToWritableString(a * a));

			timer.Stop();
			Console.WriteLine($" - {timer}");

			timer.Restart();

			Rational b = Rational.Exp("2");
			Console.Write(Rational.ToWritableString(b));

			timer.Stop();
			Console.WriteLine($" - {timer}");
		}*/

		/*for (int i = 1; i <= 10; i++)
		{
			Rational.FractionCalculationLength = i * 10;

			*//*timer.Restart();

			Rational l1 = Rational.Ln("2");
			Console.Write(Rational.ToWritableString(l1));

			timer.Stop();
			Console.WriteLine($" - {timer}");*//*

			timer.Restart();

			Rational l2 = Rational.LnFast("16.2");
			Console.Write(Rational.ToWritableString(l2));

			timer.Stop();
			Console.WriteLine($" - {timer}");
		}*/

		Rational pi = "3.14";

		for (int i = 1; i <= 100; i++)
		{
			Rational.FractionCalculationLength = i;

			timer.Restart();

			Rational a1 = Rational.Sin(pi);
			Console.Write(Rational.ToWritableString(a1));

			timer.Stop();
			Console.WriteLine($" - {timer}");
		}

		for (int i = 1; i <= 100; i++)
		{
			Rational.FractionCalculationLength = i;

			timer.Restart();

			Rational b1 = Rational.Cos(pi);
			Console.Write(Rational.ToWritableString(b1));

			timer.Stop();
			Console.WriteLine($" - {timer}");
		}
	}
}