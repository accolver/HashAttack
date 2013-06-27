using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashAttack
{
	class Program
	{
		private const int MAX_HASH_LENGTH = 4;
		private const int MAX_TEXT_LENGTH = 500;
		private const int STARTING_LIST_SIZE = 10000000;

		static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Hash Attack (Max Hash Length: {0})", MAX_HASH_LENGTH);
			Console.WriteLine("----------------------------------");
			Console.ResetColor();

			var startTime = DateTime.Now;

			var hashGen = new HashGenerator(MAX_HASH_LENGTH, STARTING_LIST_SIZE);
			var ascii = new ASCIIEncoding();
			var collision = hashGen.GenerateUntilCollision(MAX_TEXT_LENGTH, ascii);

			var endTime = DateTime.Now;
			var totalTime = endTime.Subtract(startTime);

			PrintResults(collision, totalTime);
			Console.WriteLine();


			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Second PreImage (Max Hash Length: {0})", MAX_HASH_LENGTH);
			Console.WriteLine("--------------------------------------");
			Console.ResetColor();

			startTime = DateTime.Now;

			var hashGen2 = new HashGenerator(MAX_HASH_LENGTH, STARTING_LIST_SIZE);
			var secondPreImage = hashGen.SecondPreImage("AlanColver", MAX_TEXT_LENGTH);

			endTime = DateTime.Now;
			totalTime = endTime.Subtract(startTime);

			PrintResults(secondPreImage, totalTime);

			Console.ReadLine();
		}

		private static void PrintResults(Tuple<string, string, string, int> collision, TimeSpan time)
		{
			var text1 = collision.Item1;
			var text2 = collision.Item2;
			var tHash = collision.Item3;
			var times = collision.Item4;

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("Text 1: "); Console.ResetColor(); Console.WriteLine(text1);
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("Text 2: "); Console.ResetColor(); Console.WriteLine(text2);
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("Hash:   "); Console.ResetColor(); Console.WriteLine(tHash);
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("Cycles: "); Console.ResetColor(); Console.WriteLine(times);
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("Time:   "); Console.ResetColor(); Console.WriteLine("{0}h:{1}m:{2}s:{3}ms", time.Hours, time.Minutes, time.Seconds, time.Milliseconds);
		}

		List<Tuple<int, int>> SumToInt(int[] input, int sumTo)
		{
			if (input == null || input.Length < 2)
				return null;

			var sums = new List<Tuple<int, int>>();
			var scaledDown = new List<int>();
			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] <= sumTo)
				{
					scaledDown.Add(input[i]);
				}
			}

			for (int i = 0; i < scaledDown.Count; i++)
			{
				int firstNum = scaledDown[i];
				for (int j = i+1; j < scaledDown.Count; j++)
				{
					if (firstNum + scaledDown[j] == sumTo)
					{
						sums.Add(new Tuple<int, int>(firstNum, scaledDown[j]));
					}
				}
			}
			return sums;
		}


	}
}
