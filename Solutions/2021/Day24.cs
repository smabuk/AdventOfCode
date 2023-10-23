using static AdventOfCode.Solutions._2021.Day24.InstructionType;

namespace AdventOfCode.Solutions._2021;

/// <summary>
/// Day 24: Arithmetic Logic Unit
/// https://adventofcode.com/2021/day/24
/// </summary>
[Description("Arithmetic Logic Unit")]
public class Day24 {

	public static string Part1(string[] input, params object[]? args) {
		bool debug = GetArgument<bool>(args, argumentNumber: 1, false);
		return Solution1(input, debug).ToString();
	}
	public static string Part2(string[] input, params object[]? args) {
		bool debug = GetArgument<bool>(args, argumentNumber: 1, false);
		return Solution2(input, debug).ToString();
	}

	private static string Solution1(string[] input, bool debug = false) {
		List<Instruction> instructions = ALU.Parse(input).ToList();

		long modelNumber = 11111111111111;

		ALU submarineAlu = new();

		if (debug) {
			submarineAlu.Debug = debug;
			Console.WriteLine();
			Console.WriteLine($"{modelNumber}");
		}

		int[] digits = modelNumber.ToString().Select(d => int.Parse($"{d}")).ToArray();
		_ = submarineAlu.ExecuteInstructions(instructions, digits);

		if (debug) {
			Console.WriteLine();
			if (submarineAlu.HasException) {
				Console.WriteLine(submarineAlu.ExceptionMessage);
			} else {
				Console.WriteLine($"{modelNumber} w: {submarineAlu.W} x: {submarineAlu.X} y: {submarineAlu.Y} z: {submarineAlu.Z} ");
			}
		}

		if (submarineAlu.Z == 0) {
			return string.Join("", digits.Select(d => $"{d}"));
		} else {
			foreach ((int w1, int w2, int offset) in submarineAlu.Conditions) {
				if (offset >= 0) {
					digits[w1] = 9;
					digits[w2] = 9 - offset;

				} else {
					digits[w2] = 9;
					digits[w1] = 9 + offset;
				}
			}
			return string.Join("", digits.Select(d => $"{d}"));
		}
	}

	private static string Solution2(string[] input, bool debug = false) {
		List<Instruction> instructions = ALU.Parse(input).ToList();

		long modelNumber = 11111111111111;

		ALU submarineAlu = new();

		if (debug) {
			submarineAlu.Debug = debug;
			Console.WriteLine();
			Console.WriteLine($"{modelNumber}");
		}

		int[] digits = modelNumber.ToString().Select(d => int.Parse($"{d}")).ToArray();
		_ = submarineAlu.ExecuteInstructions(instructions, digits);

		if (debug) {
			Console.WriteLine();
			if (submarineAlu.HasException) {
				Console.WriteLine(submarineAlu.ExceptionMessage);
			} else {
				Console.WriteLine($"{modelNumber} w: {submarineAlu.W} x: {submarineAlu.X} y: {submarineAlu.Y} z: {submarineAlu.Z} ");
			}
		}

		if (submarineAlu.Z == 0) {
			return string.Join("", digits.Select(d => $"{d}"));
		} else {
			foreach ((int w1, int w2, int offset) in submarineAlu.Conditions) {
				if (offset >= 0) {
					digits[w1] = 1 + offset;
					digits[w2] = 1;

				} else {
					digits[w2] = 1 - offset;
					digits[w1] = 1;
				}
			}
			return string.Join("", digits.Select(d => $"{d}"));
		}
	}

	public record class ALU {
		public bool Debug { get; set; } = false;
		private readonly Dictionary<Register, int> Registers = new () {
			{ new Register("w"), 0 },
			{ new Register("x"), 0 },
			{ new Register("y"), 0 },
			{ new Register("z"), 0 },
		};
		public List<(int w1, int w2, int offset)> Conditions = [];

		public int W => Registers[new Register("w")];
		public int X => Registers[new Register("x")];
		public int Y => Registers[new Register("y")];
		public int Z => Registers[new Register("z")];
		public bool HasException { get; set; } = false;
		public string ExceptionMessage { get; set; } = "";

		public Queue<int> InputQueue { get; set; } = new();

		public bool ExecuteInstructions(List<Instruction> instructions, int[] inputs) {
			foreach (int input in inputs) {
				InputQueue.Enqueue(input);
			}
			Conditions = [];
			int lastAddX = 0;
			int lastAddY = 0;
			bool divZBy26 = false;
			int zDepth = -1;
			Dictionary<int, (int Section, int Value)> lastZs = [];
			List<string> Rules = [];
			int section = -1;
			if (Debug) { Console.WriteLine(); };
			for (int index = 0; index < instructions.Count; index++) {
				Instruction instruction = instructions[index];
				switch (instruction.OpCode) {
					case Inp:
						if (Debug) {
							Console.WriteLine();
						}
						section++;
						if (Debug) {
							Console.WriteLine($"Section {section,-2}: ");
						};
						zDepth++;
						Input(instruction.VariableA, InputQueue.Dequeue());
						break;
					case InstructionType.Add:
						Add(instruction.VariableA, instruction.VariableB);
						if (instruction.VariableB is IntValue v) {
							if (instruction.VariableA.Name == "x") {
								lastAddX = v.Value;
							} else if (instruction.VariableA.Name == "y" && instructions[index - 1].VariableB is Register r && r.Name == "w") {
								lastAddY = v.Value;
							}
						}
						break;
					case Mul:
						Multiply(instruction.VariableA, instruction.VariableB);
						break;
					case Div:
						if (instruction.VariableB is IntValue varz) {
							divZBy26 = varz.Value == 26;
						}
						Divide(instruction.VariableA, instruction.VariableB);
						break;
					case Mod:
						Modulo(instruction.VariableA, instruction.VariableB);
						break;
					case Eql:
						Equal(instruction.VariableA, instruction.VariableB);
						break;
					default:
						break;
				}
				if (Debug) {
					Console.WriteLine($"i:{index,3}   w: {W,-2} x: {X,-11} y: {Y,2}   z: {Z,-11}  {instruction.OpCode} {instruction.VariableA}  {instruction.VariableB}");
				};
				if (HasException) {
					ExceptionMessage = $"{string.Join("", inputs.Select(d => $"{d}"))} {ExceptionMessage}";
					break;
				}
				if (index == instructions.Count - 1 || instructions[index + 1].OpCode is Inp) {
					if (Debug) {
						Console.Write($"Summary {section,2}:");
					}
					if (divZBy26) {
						zDepth -= 2;
						(int sec, int val) = lastZs[section - 2];
						string condition = $"w{section} == {FormatSum(lastZs[section - 1].Section, lastZs[section - 1].Value + lastAddX)}";
						if (zDepth < 1) {
							(sec, val) = lastZs[zDepth + 1];
							condition = $"w{section} == {FormatSum(sec, val + lastAddX)}";
							Conditions.Add((section, sec, val + lastAddX));
						} else {
							Conditions.Add((section, lastZs[section - 1].Section, lastZs[section - 1].Value + lastAddX));
						}
						lastZs.Add(section, (sec, val));
						lastAddY = 0;
						if (Debug) {
							Console.ForegroundColor = ConsoleColor.Green;
							Console.Write($"   x: {X,-4}  {condition,-13}   z: {FormatSum(sec, val)}       depth: {zDepth}");
						}
					} else {
						lastZs.Add(section, (section, lastAddY));
						if (Debug) {
							Console.Write($"   x: {X,-11}         z: {FormatSum(section, lastAddY)}       depth: {zDepth}");
						}
					}
					if (Debug) {
						Console.ResetColor();
						Console.WriteLine();
					}
				};

			}
			foreach ((int w1, int w2, int offset) in Conditions) {
				Rules.Add($"  {$"w{w1}",3} == {FormatSum(w2, offset)}");
				Rules.Add($"  {$"w{w2}",3} == {FormatSum(w1, -offset)}");
			}
			if (Debug) {
				Console.WriteLine("Rules");
				foreach (string rule in Rules.OrderBy(r => r)) {
					Console.WriteLine(rule);
				}
				Console.WriteLine();
			}
			return HasException;
		}

		private static string FormatSum(int section, int value) {
			string w1 = $"w{section}";
			return value switch {
				0 => $"{w1,3}",
				< 0 => $"{w1,3} - {Math.Abs(value)}",
				> 0 => $"{w1,3} + {Math.Abs(value)}",
			};
		}

		private void Input(Register variableA, int value) => SetVariable(variableA, value);

		private void Add(Register registerA, RegisterOrIntValue variableB) {
			int a = GetVariable(registerA);
			int b = GetVariable(variableB);
			SetVariable(registerA, a + b);
		}

		private void Multiply(Register registerA, RegisterOrIntValue variableB) {
			int a = GetVariable(registerA);
			int b = GetVariable(variableB);
			SetVariable(registerA, a * b);
		}

		private void Divide(Register registerA, RegisterOrIntValue variableB) {
			int a = GetVariable(registerA);
			int b = GetVariable(variableB);
			if (b == 0) {
				HasException = true;
				ExceptionMessage = $"Divide by Zero of {a}";
			}
			SetVariable(registerA, a / b);
		}

		private void Modulo(Register registerA, RegisterOrIntValue variableB) {
			int a = GetVariable(registerA);
			int b = GetVariable(variableB);
			if (a < 0 || b < 0) {
				HasException = true;
				ExceptionMessage = $"Negative Modulo {a} {b}";
			}
			SetVariable(registerA, a % b);
		}

		private void Equal(Register registerA, RegisterOrIntValue variableB) {
			int a = GetVariable(registerA);
			int b = GetVariable(variableB);
			SetVariable(registerA, a == b ? 1 : 0);
		}

		private int GetVariable(RegisterOrIntValue variable) {
			if (variable is Register r) {
				return Registers[r];
			} else if (variable is IntValue i) {
				return i.Value;
			}
			throw new ArgumentOutOfRangeException();
		}

		private void SetVariable(Register r, int value) {
			Registers[r] = value;
		}

		public static IEnumerable<Instruction> Parse(string[] inputs) {
			foreach (string input in inputs) {
				yield return Instruction.Parse(input);
			}
		}
	}

	public record Instruction(InstructionType OpCode, Register VariableA, RegisterOrIntValue VariableB) {
		public static Instruction Parse(string input) {
			string[] i = input.Split(' ');
			InstructionType opCode = i[0] switch {
				"inp" => Inp,
				"add" => Add,
				"mul" => Mul,
				"div" => Div,
				"mod" => Mod,
				"eql" => Eql,
				_ => throw new NotImplementedException(),
			};

			RegisterOrIntValue riA = char.IsLetter(i[1][0]) switch {
				true => new Register(i[1]),
				false => new IntValue(int.Parse(i[1])),
			};

			if (opCode is Inp) {
				return new Instruction(opCode, new Register("w"), riA);
			}

			RegisterOrIntValue riB = char.IsLetter(i[2][0]) switch {
				true => new Register(i[2]),
				false => new IntValue(int.Parse(i[2])),
			};

			return opCode switch {
				Add
				or Mul
				or Div 
				or Mod
				or Eql => new Instruction(opCode, (Register)riA, riB),
				_ => throw new NotImplementedException(),
			};
		}
	};
	public enum InstructionType {
		Inp,
		Add,
		Mul,
		Div,
		Mod,
		Eql,
	}
	public abstract record RegisterOrIntValue();
	public record Register(string Name) : RegisterOrIntValue {
		public override string ToString() => $"{Name,2}";
	}
	
	public record IntValue(int Value) : RegisterOrIntValue {
		public override string ToString() => $"{Value,2}";
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
 * Rules for my input data
 * 

		//  w0 = w13 + 8
		//  w1 = w12 - 7
		//  w2 =  w3 + 7
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
