using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace MathGame
{
	internal class Program
	{
		private static readonly List<string> history = new List<string>();

		public static void Main()
		{
			try
			{
				Menu();
			}
			catch (Exception e)
			{
				Console.WriteLine("An error occurred: " + e.Message);
			}
		}

		private static void Menu()
		{
			while (true)
			{
				Console.WriteLine("Main Menu");
				Console.WriteLine("1. Addition");
				Console.WriteLine("2. Subtraction");
				Console.WriteLine("3. Multiplication");
				Console.WriteLine("4. Division");
				Console.WriteLine("5. Random Operation");
				Console.WriteLine("6. Show History");
				Console.WriteLine("7. Exit");

				int userInput = NumberConsoleInput();
				int methodToCall = userInput;
				// Gets a random operation if the user selects the random operation option
				if (userInput == 5)
				{
					methodToCall = GenerateRandomNumber(4);
				}

				switch (methodToCall)
				{
					case 1:
						PerformOperation('+');
						break;
					case 2:
						PerformOperation('-');
						break;
					case 3:
						PerformOperation('*');
						break;
					case 4:
						PerformOperation('/');
						break;
					case 6:
						ShowHistory();
						break;
					case 7:
						Environment.Exit(0);
						break;
					default:
						Console.WriteLine("Invalid operation");
						break;
				}
			}
		}

		private static void PerformOperation(char operation)
		{
			// New Stopwatch instance to track the time user took to calculate
			var stopWatch = new Stopwatch();
			// Sets difficulty level
			Console.WriteLine("Enter the maximum number for the operation");
			int maxNumber = NumberConsoleInput();
			int number1 = GenerateRandomNumber(maxNumber);
			int number2 = GenerateRandomNumber(maxNumber);
			int result = operation switch
			{
				'+' => number1 + number2,
				'-' => number1 - number2,
				'*' => number1 * number2,
				'/' => number1 / number2,
				_ => throw new InvalidOperationException("Invalid operation")
			};

			if (operation == '/')
			{
				while (number2 == 0 || number1 % number2 != 0)
				{
					number1 = GenerateRandomNumber(maxNumber);
					number2 = GenerateRandomNumber(maxNumber);
				}

				result = number1 / number2;
			}

			Console.WriteLine($"Guess the result of the following operation: {number1} {operation} {number2} = ?");
			stopWatch.Start();
			var userResult = NumberConsoleInput();
			stopWatch.Stop();
			var timeTook = stopWatch.Elapsed;
			CheckResult(result, userResult);
			SaveInHistory(number1, number2, operation, result, userResult, timeTook);
		}

		private static void CheckResult(int result, int userResult)
		{
			Console.WriteLine(userResult == result ? "Correct!" : $"Incorrect, the correct answer is {result}");
		}

		private static void SaveInHistory(int number1, int number2, char operation, int result, int userInput,
			TimeSpan timeTook)
		{
			history.Add($"{number1} {operation} {number2} = {result} (Your answer: {userInput}) Time took: {timeTook}");
		}

		private static void ShowHistory()
		{
			if (history.Count == 0)
			{
				Console.WriteLine("No history available.");
				return;
			}

			foreach (var entry in history)
			{
				Console.WriteLine(entry);
			}
		}

		private static int NumberConsoleInput()
		{
			while (true)
			{
				if (int.TryParse(Console.ReadLine(), out int number))
				{
					return number;
				}

				Console.WriteLine("Invalid Input, please enter a valid number");
			}
		}

		private static int GenerateRandomNumber(int maxNumber)
		{
			var random = new Random();
			return random.Next(1, maxNumber);
		}
	}
}