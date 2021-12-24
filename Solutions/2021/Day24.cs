﻿namespace AdventOfCode.Solutions.Year2021;

/// <summary>
/// Day 24: Arithmetic Logic Unit
/// https://adventofcode.com/2021/day/24
/// </summary>
[Description("Arithmetic Logic Unit")]
public class Day24 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static string Solution1(string[] input) {
		List<Instruction> instructions = input.Select(i => ParseLine(i)).ToList();
		int[] digits = new int[14];

		// Rules
		//  w0 = w13 + 8
		//  w1 = w12 - 7
		//  w2 =  w3 - 7
		//  w3 =  w2 - 7
		//  w4 =  w5 - 1
		//  w5 =  w4 + 1
		//  w6 = w11 - 8
		//  w7 =  w8 - 5
		//  w8 =  w7 + 5
		//  w9 = w10
		// w10 =  w9
		// w11 =  w6 + 8
		// w12 =  w1 + 7
		// w13 =  w0 - 8

		ALU submarineAlu = new();
		//                 01234567890123
		// Too low Guess = 92922311699991;
		// Too low Guess = 92922314999991;
		long modelNumber = 92928914999991;

		//Console.WriteLine();

		//Console.WriteLine($"{modelNumber}");
		digits = modelNumber.ToString().Select(d => int.Parse($"{d}")).ToArray();
		submarineAlu = new();
		submarineAlu.ExecuteInstructions(instructions, digits);
		//Console.WriteLine();
		if (submarineAlu.HasException) {
			//Console.WriteLine(submarineAlu.ExceptionMessage);
		} else {
			//Console.WriteLine($"{modelNumber} w: {submarineAlu.W} x: {submarineAlu.X} y: {submarineAlu.Y} z: {submarineAlu.Z} ");
		}

		if (submarineAlu.Z == 0) {
			return String.Join("", digits.Select(d => $"{d}"));
		} else {
			return "No idea";
		}
	}

	private static string Solution2(string[] input) {
		List<Instruction> instructions = input.Select(i => ParseLine(i)).ToList();
		int[] digits = new int[14];

		// Rules
		//  w0 = w13 + 8
		//  w1 = w12 - 7
		//  w2 =  w3 - 7
		//  w3 =  w2 - 7
		//  w4 =  w5 - 1
		//  w5 =  w4 + 1
		//  w6 = w11 - 8
		//  w7 =  w8 - 5
		//  w8 =  w7 + 5
		//  w9 = w10
		// w10 =  w9
		// w11 =  w6 + 8
		// w12 =  w1 + 7
		// w13 =  w0 - 8


		ALU submarineAlu = new();
		//                 01234567890123
		long modelNumber = 91811211611981;

		//Console.WriteLine();

		//Console.WriteLine($"{modelNumber}");
		digits = modelNumber.ToString().Select(d => int.Parse($"{d}")).ToArray();
		submarineAlu = new();
		submarineAlu.ExecuteInstructions(instructions, digits);
		//Console.WriteLine();
		if (submarineAlu.HasException) {
			//Console.WriteLine(submarineAlu.ExceptionMessage);
		} else {
			//Console.WriteLine($"{modelNumber} w: {submarineAlu.W} x: {submarineAlu.X} y: {submarineAlu.Y} z: {submarineAlu.Z} ");
		}

		if (submarineAlu.Z == 0) {
			return String.Join("", digits.Select(d => $"{d}"));
		} else {
			return "No idea";
		}
	}

	public record class ALU {
		public int W { get; set; } = 0;
		public int X { get; set; } = 0;
		public int Y { get; set; } = 0;
		public int Z { get; set; } = 0;
		public bool HasException { get; set; } = false;
		public string ExceptionMessage { get; set; } = "";

		public Queue<int> InputQueue { get; set; } = new();



		public bool ExecuteInstructions(List<Instruction> instructions, int[] inputs) {
			foreach (int input in inputs) {
				InputQueue.Enqueue(input);
			}
			int section = -1;
			//Console.WriteLine();
			foreach (var instruction in instructions) {
				switch (instruction.Operation) {
					case "inp":
						section++;
						//Console.Write($"Section: {section,-2}: ");
						//Console.WriteLine($"w: {W,-11} x: {X,-11} y: {Y,-11} z: {Z,-11} inst: {instruction.Operation} {instruction.VariableA} {instruction.VariableB}");
						Input(instruction.VariableA, InputQueue.Dequeue());
						break;
					case "add":
						Add(instruction.VariableA, instruction.VariableB);
						break;
					case "mul":
						Multiply(instruction.VariableA, instruction.VariableB);
						break;
					case "div":
						Divide(instruction.VariableA, instruction.VariableB);
						break;
					case "mod":
						Modulo(instruction.VariableA, instruction.VariableB);
						break;
					case "eql":
						Equal(instruction.VariableA, instruction.VariableB);
						break;
					default:
						break;
				}
				//if (section == 13) {
				//	Console.WriteLine($"w: {W,-11} x: {X,-11} y: {Y,-11} z: {Z,-11} inst: {instruction.Operation} {instruction.VariableA} {instruction.VariableB}");
				//}
				if (HasException) {
					ExceptionMessage = $"{String.Join("", inputs.Select(d => $"{d}"))} {ExceptionMessage}";
					break;
				}
			}
			return HasException;

		}

		private void Add(string variableA, string variableB) {
			int a = GetVariable(variableA);
			int b = GetVariable(variableB);
			SetVariable(variableA, a + b);
		}

		private void Multiply(string variableA, string variableB) {
			int a = GetVariable(variableA);
			int b = GetVariable(variableB);
			SetVariable(variableA, a * b);
		}

		private void Divide(string variableA, string variableB) {
			int a = GetVariable(variableA);
			int b = GetVariable(variableB);
			if (b == 0) {
				HasException = true;
				ExceptionMessage = $"Divide by Zero of {a}";
			}
			SetVariable(variableA, a / b);
		}

		private void Modulo(string variableA, string variableB) {
			int a = GetVariable(variableA);
			int b = GetVariable(variableB);
			if (a < 0 || b < 0) {
				HasException = true;
				ExceptionMessage = $"Negative Modulo {a} {b}";
			}
			SetVariable(variableA, a % b);
		}

		private void Equal(string variableA, string variableB) {
			int a = GetVariable(variableA);
			int b = GetVariable(variableB);
			SetVariable(variableA, a == b ? 1 : 0);
		}

		private void Input(string variableA, int value) => SetVariable(variableA, value);
		private int GetVariable(string variable) {
			return variable switch {
				"w" => W,
				"x" => X,
				"y" => Y,
				"z" => Z,
				_ => int.Parse(variable),
			};
		}

		private void SetVariable(string variable, int value) {
			switch (variable) {
				case "w":
					W = value;
					break;
				case "x":
					X = value;
					break;
				case "y":
					Y = value;
					break;
				case "z":
					Z = value;
					break;
				default:
					break;
			}
		}

		public static IEnumerable<Instruction> Parse(string[] inputs) {
			foreach (string input in inputs) {
				string[] i = input.Split(' ');
				yield return new(i[0], i[1], i.Length < 3 ? "" : i[2]);
			}
		}
	}






	public record Instruction(string Operation, string VariableA, string VariableB) {
		public int NumberB {
			get {
				if (string.IsNullOrWhiteSpace(VariableB) || char.IsLetter(VariableB[0])) {
					return 0;
				} else {
					return int.Parse(VariableB);
				}
			}
		}
	};

	private static Instruction ParseLine(string input) {
		string[] i = input.Split(' ');
		return new(i[0], i[1], i.Length < 3 ? "" : i[2]);
	}
}

/*
 * TYPICAL Program Segment
 * 
	inp w
	mul x 0
	add x z
	mod x 26
	div z 1
	add x 12
	eql x w
	eql x 0
	mul y 0
	add y 25
	mul y x
	add y 1
	mul z y
	mul y 0
	add y w
	add y 4
	mul y x
	add z y

	condition is w == remainder (z + 12)
	x => 0        if {condition} else 1
	y => 0        if {condition} else w + 12
	z => (z / 26) if {condition} else (z * 26) + w + 12

 * 
 * Debug run of lowest successful number
 * 

Section: 0 : w: 0           x: 0           y: 0           z: 0           inst: inp w
w: 9           x: 0           y: 0           z: 0           inst: inp w
w: 9           x: 0           y: 0           z: 0           inst: mul x 0
w: 9           x: 0           y: 0           z: 0           inst: add x z
w: 9           x: 0           y: 0           z: 0           inst: mod x 26
w: 9           x: 0           y: 0           z: 0           inst: div z 1
w: 9           x: 12          y: 0           z: 0           inst: add x 12
w: 9           x: 0           y: 0           z: 0           inst: eql x w
w: 9           x: 1           y: 0           z: 0           inst: eql x 0
w: 9           x: 1           y: 0           z: 0           inst: mul y 0
w: 9           x: 1           y: 25          z: 0           inst: add y 25
w: 9           x: 1           y: 25          z: 0           inst: mul y x
w: 9           x: 1           y: 26          z: 0           inst: add y 1
w: 9           x: 1           y: 26          z: 0           inst: mul z y
w: 9           x: 1           y: 0           z: 0           inst: mul y 0
w: 9           x: 1           y: 9           z: 0           inst: add y w
w: 9           x: 1           y: 13          z: 0           inst: add y 4
w: 9           x: 1           y: 13          z: 0           inst: mul y x
w: 9           x: 1           y: 13          z: 13          inst: add z y
Section: 1 : w: 9           x: 1           y: 13          z: 13          inst: inp w
w: 1           x: 1           y: 13          z: 13          inst: inp w
w: 1           x: 0           y: 13          z: 13          inst: mul x 0
w: 1           x: 13          y: 13          z: 13          inst: add x z
w: 1           x: 13          y: 13          z: 13          inst: mod x 26
w: 1           x: 13          y: 13          z: 13          inst: div z 1
w: 1           x: 28          y: 13          z: 13          inst: add x 15
w: 1           x: 0           y: 13          z: 13          inst: eql x w
w: 1           x: 1           y: 13          z: 13          inst: eql x 0
w: 1           x: 1           y: 0           z: 13          inst: mul y 0
w: 1           x: 1           y: 25          z: 13          inst: add y 25
w: 1           x: 1           y: 25          z: 13          inst: mul y x
w: 1           x: 1           y: 26          z: 13          inst: add y 1
w: 1           x: 1           y: 26          z: 338         inst: mul z y
w: 1           x: 1           y: 0           z: 338         inst: mul y 0
w: 1           x: 1           y: 1           z: 338         inst: add y w
w: 1           x: 1           y: 12          z: 338         inst: add y 11
w: 1           x: 1           y: 12          z: 338         inst: mul y x
w: 1           x: 1           y: 12          z: 350         inst: add z y
Section: 2 : w: 1           x: 1           y: 12          z: 350         inst: inp w
w: 8           x: 1           y: 12          z: 350         inst: inp w
w: 8           x: 0           y: 12          z: 350         inst: mul x 0
w: 8           x: 350         y: 12          z: 350         inst: add x z
w: 8           x: 12          y: 12          z: 350         inst: mod x 26
w: 8           x: 12          y: 12          z: 350         inst: div z 1
w: 8           x: 23          y: 12          z: 350         inst: add x 11
w: 8           x: 0           y: 12          z: 350         inst: eql x w
w: 8           x: 1           y: 12          z: 350         inst: eql x 0
w: 8           x: 1           y: 0           z: 350         inst: mul y 0
w: 8           x: 1           y: 25          z: 350         inst: add y 25
w: 8           x: 1           y: 25          z: 350         inst: mul y x
w: 8           x: 1           y: 26          z: 350         inst: add y 1
w: 8           x: 1           y: 26          z: 9100        inst: mul z y
w: 8           x: 1           y: 0           z: 9100        inst: mul y 0
w: 8           x: 1           y: 8           z: 9100        inst: add y w
w: 8           x: 1           y: 15          z: 9100        inst: add y 7
w: 8           x: 1           y: 15          z: 9100        inst: mul y x
w: 8           x: 1           y: 15          z: 9115        inst: add z y
Section: 3 : w: 8           x: 1           y: 15          z: 9115        inst: inp w
w: 1           x: 1           y: 15          z: 9115        inst: inp w
w: 1           x: 0           y: 15          z: 9115        inst: mul x 0
w: 1           x: 9115        y: 15          z: 9115        inst: add x z
w: 1           x: 15          y: 15          z: 9115        inst: mod x 26
w: 1           x: 15          y: 15          z: 350         inst: div z 26
w: 1           x: 1           y: 15          z: 350         inst: add x -14
w: 1           x: 1           y: 15          z: 350         inst: eql x w
w: 1           x: 0           y: 15          z: 350         inst: eql x 0
w: 1           x: 0           y: 0           z: 350         inst: mul y 0
w: 1           x: 0           y: 25          z: 350         inst: add y 25
w: 1           x: 0           y: 0           z: 350         inst: mul y x
w: 1           x: 0           y: 1           z: 350         inst: add y 1
w: 1           x: 0           y: 1           z: 350         inst: mul z y
w: 1           x: 0           y: 0           z: 350         inst: mul y 0
w: 1           x: 0           y: 1           z: 350         inst: add y w
w: 1           x: 0           y: 3           z: 350         inst: add y 2
w: 1           x: 0           y: 0           z: 350         inst: mul y x
w: 1           x: 0           y: 0           z: 350         inst: add z y
Section: 4 : w: 1           x: 0           y: 0           z: 350         inst: inp w
w: 1           x: 0           y: 0           z: 350         inst: inp w
w: 1           x: 0           y: 0           z: 350         inst: mul x 0
w: 1           x: 350         y: 0           z: 350         inst: add x z
w: 1           x: 12          y: 0           z: 350         inst: mod x 26
w: 1           x: 12          y: 0           z: 350         inst: div z 1
w: 1           x: 24          y: 0           z: 350         inst: add x 12
w: 1           x: 0           y: 0           z: 350         inst: eql x w
w: 1           x: 1           y: 0           z: 350         inst: eql x 0
w: 1           x: 1           y: 0           z: 350         inst: mul y 0
w: 1           x: 1           y: 25          z: 350         inst: add y 25
w: 1           x: 1           y: 25          z: 350         inst: mul y x
w: 1           x: 1           y: 26          z: 350         inst: add y 1
w: 1           x: 1           y: 26          z: 9100        inst: mul z y
w: 1           x: 1           y: 0           z: 9100        inst: mul y 0
w: 1           x: 1           y: 1           z: 9100        inst: add y w
w: 1           x: 1           y: 12          z: 9100        inst: add y 11
w: 1           x: 1           y: 12          z: 9100        inst: mul y x
w: 1           x: 1           y: 12          z: 9112        inst: add z y
Section: 5 : w: 1           x: 1           y: 12          z: 9112        inst: inp w
w: 2           x: 1           y: 12          z: 9112        inst: inp w
w: 2           x: 0           y: 12          z: 9112        inst: mul x 0
w: 2           x: 9112        y: 12          z: 9112        inst: add x z
w: 2           x: 12          y: 12          z: 9112        inst: mod x 26
w: 2           x: 12          y: 12          z: 350         inst: div z 26
w: 2           x: 2           y: 12          z: 350         inst: add x -10
w: 2           x: 1           y: 12          z: 350         inst: eql x w
w: 2           x: 0           y: 12          z: 350         inst: eql x 0
w: 2           x: 0           y: 0           z: 350         inst: mul y 0
w: 2           x: 0           y: 25          z: 350         inst: add y 25
w: 2           x: 0           y: 0           z: 350         inst: mul y x
w: 2           x: 0           y: 1           z: 350         inst: add y 1
w: 2           x: 0           y: 1           z: 350         inst: mul z y
w: 2           x: 0           y: 0           z: 350         inst: mul y 0
w: 2           x: 0           y: 2           z: 350         inst: add y w
w: 2           x: 0           y: 15          z: 350         inst: add y 13
w: 2           x: 0           y: 0           z: 350         inst: mul y x
w: 2           x: 0           y: 0           z: 350         inst: add z y
Section: 6 : w: 2           x: 0           y: 0           z: 350         inst: inp w
w: 1           x: 0           y: 0           z: 350         inst: inp w
w: 1           x: 0           y: 0           z: 350         inst: mul x 0
w: 1           x: 350         y: 0           z: 350         inst: add x z
w: 1           x: 12          y: 0           z: 350         inst: mod x 26
w: 1           x: 12          y: 0           z: 350         inst: div z 1
w: 1           x: 23          y: 0           z: 350         inst: add x 11
w: 1           x: 0           y: 0           z: 350         inst: eql x w
w: 1           x: 1           y: 0           z: 350         inst: eql x 0
w: 1           x: 1           y: 0           z: 350         inst: mul y 0
w: 1           x: 1           y: 25          z: 350         inst: add y 25
w: 1           x: 1           y: 25          z: 350         inst: mul y x
w: 1           x: 1           y: 26          z: 350         inst: add y 1
w: 1           x: 1           y: 26          z: 9100        inst: mul z y
w: 1           x: 1           y: 0           z: 9100        inst: mul y 0
w: 1           x: 1           y: 1           z: 9100        inst: add y w
w: 1           x: 1           y: 10          z: 9100        inst: add y 9
w: 1           x: 1           y: 10          z: 9100        inst: mul y x
w: 1           x: 1           y: 10          z: 9110        inst: add z y
Section: 7 : w: 1           x: 1           y: 10          z: 9110        inst: inp w
w: 1           x: 1           y: 10          z: 9110        inst: inp w
w: 1           x: 0           y: 10          z: 9110        inst: mul x 0
w: 1           x: 9110        y: 10          z: 9110        inst: add x z
w: 1           x: 10          y: 10          z: 9110        inst: mod x 26
w: 1           x: 10          y: 10          z: 9110        inst: div z 1
w: 1           x: 23          y: 10          z: 9110        inst: add x 13
w: 1           x: 0           y: 10          z: 9110        inst: eql x w
w: 1           x: 1           y: 10          z: 9110        inst: eql x 0
w: 1           x: 1           y: 0           z: 9110        inst: mul y 0
w: 1           x: 1           y: 25          z: 9110        inst: add y 25
w: 1           x: 1           y: 25          z: 9110        inst: mul y x
w: 1           x: 1           y: 26          z: 9110        inst: add y 1
w: 1           x: 1           y: 26          z: 236860      inst: mul z y
w: 1           x: 1           y: 0           z: 236860      inst: mul y 0
w: 1           x: 1           y: 1           z: 236860      inst: add y w
w: 1           x: 1           y: 13          z: 236860      inst: add y 12
w: 1           x: 1           y: 13          z: 236860      inst: mul y x
w: 1           x: 1           y: 13          z: 236873      inst: add z y
Section: 8 : w: 1           x: 1           y: 13          z: 236873      inst: inp w
w: 6           x: 1           y: 13          z: 236873      inst: inp w
w: 6           x: 0           y: 13          z: 236873      inst: mul x 0
w: 6           x: 236873      y: 13          z: 236873      inst: add x z
w: 6           x: 13          y: 13          z: 236873      inst: mod x 26
w: 6           x: 13          y: 13          z: 9110        inst: div z 26
w: 6           x: 6           y: 13          z: 9110        inst: add x -7
w: 6           x: 1           y: 13          z: 9110        inst: eql x w
w: 6           x: 0           y: 13          z: 9110        inst: eql x 0
w: 6           x: 0           y: 0           z: 9110        inst: mul y 0
w: 6           x: 0           y: 25          z: 9110        inst: add y 25
w: 6           x: 0           y: 0           z: 9110        inst: mul y x
w: 6           x: 0           y: 1           z: 9110        inst: add y 1
w: 6           x: 0           y: 1           z: 9110        inst: mul z y
w: 6           x: 0           y: 0           z: 9110        inst: mul y 0
w: 6           x: 0           y: 6           z: 9110        inst: add y w
w: 6           x: 0           y: 12          z: 9110        inst: add y 6
w: 6           x: 0           y: 0           z: 9110        inst: mul y x
w: 6           x: 0           y: 0           z: 9110        inst: add z y
Section: 9 : w: 6           x: 0           y: 0           z: 9110        inst: inp w
w: 1           x: 0           y: 0           z: 9110        inst: inp w
w: 1           x: 0           y: 0           z: 9110        inst: mul x 0
w: 1           x: 9110        y: 0           z: 9110        inst: add x z
w: 1           x: 10          y: 0           z: 9110        inst: mod x 26
w: 1           x: 10          y: 0           z: 9110        inst: div z 1
w: 1           x: 20          y: 0           z: 9110        inst: add x 10
w: 1           x: 0           y: 0           z: 9110        inst: eql x w
w: 1           x: 1           y: 0           z: 9110        inst: eql x 0
w: 1           x: 1           y: 0           z: 9110        inst: mul y 0
w: 1           x: 1           y: 25          z: 9110        inst: add y 25
w: 1           x: 1           y: 25          z: 9110        inst: mul y x
w: 1           x: 1           y: 26          z: 9110        inst: add y 1
w: 1           x: 1           y: 26          z: 236860      inst: mul z y
w: 1           x: 1           y: 0           z: 236860      inst: mul y 0
w: 1           x: 1           y: 1           z: 236860      inst: add y w
w: 1           x: 1           y: 3           z: 236860      inst: add y 2
w: 1           x: 1           y: 3           z: 236860      inst: mul y x
w: 1           x: 1           y: 3           z: 236863      inst: add z y
Section: 10: w: 1           x: 1           y: 3           z: 236863      inst: inp w
w: 1           x: 1           y: 3           z: 236863      inst: inp w
w: 1           x: 0           y: 3           z: 236863      inst: mul x 0
w: 1           x: 236863      y: 3           z: 236863      inst: add x z
w: 1           x: 3           y: 3           z: 236863      inst: mod x 26
w: 1           x: 3           y: 3           z: 9110        inst: div z 26
w: 1           x: 1           y: 3           z: 9110        inst: add x -2
w: 1           x: 1           y: 3           z: 9110        inst: eql x w
w: 1           x: 0           y: 3           z: 9110        inst: eql x 0
w: 1           x: 0           y: 0           z: 9110        inst: mul y 0
w: 1           x: 0           y: 25          z: 9110        inst: add y 25
w: 1           x: 0           y: 0           z: 9110        inst: mul y x
w: 1           x: 0           y: 1           z: 9110        inst: add y 1
w: 1           x: 0           y: 1           z: 9110        inst: mul z y
w: 1           x: 0           y: 0           z: 9110        inst: mul y 0
w: 1           x: 0           y: 1           z: 9110        inst: add y w
w: 1           x: 0           y: 12          z: 9110        inst: add y 11
w: 1           x: 0           y: 0           z: 9110        inst: mul y x
w: 1           x: 0           y: 0           z: 9110        inst: add z y
Section: 11: w: 1           x: 0           y: 0           z: 9110        inst: inp w
w: 9           x: 0           y: 0           z: 9110        inst: inp w
w: 9           x: 0           y: 0           z: 9110        inst: mul x 0
w: 9           x: 9110        y: 0           z: 9110        inst: add x z
w: 9           x: 10          y: 0           z: 9110        inst: mod x 26
w: 9           x: 10          y: 0           z: 350         inst: div z 26
w: 9           x: 9           y: 0           z: 350         inst: add x -1
w: 9           x: 1           y: 0           z: 350         inst: eql x w
w: 9           x: 0           y: 0           z: 350         inst: eql x 0
w: 9           x: 0           y: 0           z: 350         inst: mul y 0
w: 9           x: 0           y: 25          z: 350         inst: add y 25
w: 9           x: 0           y: 0           z: 350         inst: mul y x
w: 9           x: 0           y: 1           z: 350         inst: add y 1
w: 9           x: 0           y: 1           z: 350         inst: mul z y
w: 9           x: 0           y: 0           z: 350         inst: mul y 0
w: 9           x: 0           y: 9           z: 350         inst: add y w
w: 9           x: 0           y: 21          z: 350         inst: add y 12
w: 9           x: 0           y: 0           z: 350         inst: mul y x
w: 9           x: 0           y: 0           z: 350         inst: add z y
Section: 12: w: 9           x: 0           y: 0           z: 350         inst: inp w
w: 8           x: 0           y: 0           z: 350         inst: inp w
w: 8           x: 0           y: 0           z: 350         inst: mul x 0
w: 8           x: 350         y: 0           z: 350         inst: add x z
w: 8           x: 12          y: 0           z: 350         inst: mod x 26
w: 8           x: 12          y: 0           z: 13          inst: div z 26
w: 8           x: 8           y: 0           z: 13          inst: add x -4
w: 8           x: 1           y: 0           z: 13          inst: eql x w
w: 8           x: 0           y: 0           z: 13          inst: eql x 0
w: 8           x: 0           y: 0           z: 13          inst: mul y 0
w: 8           x: 0           y: 25          z: 13          inst: add y 25
w: 8           x: 0           y: 0           z: 13          inst: mul y x
w: 8           x: 0           y: 1           z: 13          inst: add y 1
w: 8           x: 0           y: 1           z: 13          inst: mul z y
w: 8           x: 0           y: 0           z: 13          inst: mul y 0
w: 8           x: 0           y: 8           z: 13          inst: add y w
w: 8           x: 0           y: 11          z: 13          inst: add y 3
w: 8           x: 0           y: 0           z: 13          inst: mul y x
w: 8           x: 0           y: 0           z: 13          inst: add z y
Section: 13: w: 8           x: 0           y: 0           z: 13          inst: inp w
w: 1           x: 0           y: 0           z: 13          inst: inp w
w: 1           x: 0           y: 0           z: 13          inst: mul x 0
w: 1           x: 13          y: 0           z: 13          inst: add x z
w: 1           x: 13          y: 0           z: 13          inst: mod x 26
w: 1           x: 13          y: 0           z: 0           inst: div z 26
w: 1           x: 1           y: 0           z: 0           inst: add x -12
w: 1           x: 1           y: 0           z: 0           inst: eql x w
w: 1           x: 0           y: 0           z: 0           inst: eql x 0
w: 1           x: 0           y: 0           z: 0           inst: mul y 0
w: 1           x: 0           y: 25          z: 0           inst: add y 25
w: 1           x: 0           y: 0           z: 0           inst: mul y x
w: 1           x: 0           y: 1           z: 0           inst: add y 1
w: 1           x: 0           y: 1           z: 0           inst: mul z y
w: 1           x: 0           y: 0           z: 0           inst: mul y 0
w: 1           x: 0           y: 1           z: 0           inst: add y w
w: 1           x: 0           y: 14          z: 0           inst: add y 13
w: 1           x: 0           y: 0           z: 0           inst: mul y x
w: 1           x: 0           y: 0           z: 0           inst: add z y

 91811211611981 w: 1 x: 0 y: 0 z: 0
 Pt2:   27ms  91811211611981
 * 
 * 
 * 
*/
