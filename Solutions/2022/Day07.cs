using System.IO;

namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 07: No Space Left On Device
/// https://adventofcode.com/2022/day/7
/// </summary>
[Description("No Space Left On Device")]
public sealed partial class Day07 {
	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private record Directory(string Name, Directory Parent) {
		public Dictionary<string, Directory> Directories { get; set; } = new();
		public Dictionary<string, File> Files { get; set; } = new();

		public void AddDirectory(string name) {
			Directories.TryAdd(name, new Directory(name, this));
		}
		public void AddFile(File file) {
			Files.TryAdd(file.Name, file);
		}

		public int Size => Directories.Sum(x => x.Value.Size) + Files.Sum(x => x.Value.Size);
	};

	private record File(string Name, int Size);

	private static int Solution1(string[] input) {
		string[] terminalOutput = input;
		Directory fileSystem = new("/", null!);
		Directory currentDirectory = fileSystem;
		for (int i = 0; i < terminalOutput.Length; i++) {
			string output = terminalOutput[i];
			if (output.StartsWith("$ cd")) {
				string directoryName = output[5..];
				if (directoryName == "/") {
					currentDirectory = fileSystem;
				} else if (directoryName == "..") {
					currentDirectory = currentDirectory.Parent;
				} else {
					currentDirectory.AddDirectory(new(directoryName));
					currentDirectory = currentDirectory.Directories[directoryName];
				}
			} else if (output.StartsWith("dir")) {
				string directoryName = output[4..];
				currentDirectory.AddDirectory(new(directoryName));
				// tempCurrentDirectory = currentDirectory.Directories[directoryName];
			} else if (Char.IsNumber(output[0])) {
				string[] tokens = output.Split(" ");
				currentDirectory.AddFile(new(tokens[1], int.Parse(tokens[0])));
			}
		}

		return WalkDirectoryTreeAndCountLarge(fileSystem);
	}

	private static int WalkDirectoryTreeAndCountLarge(Directory directory) {
		int size = directory.Size <= 100000 ? directory.Size : 0;
		foreach (Directory subDir in directory.Directories.Values) {
			// Resursive call for each subdirectory.
			size += WalkDirectoryTreeAndCountLarge(subDir);
		}
		return size;
	}


	private static string Solution2(string[] input) {
		//string inputLine = input[0];
		//List<string> inputs = input.ToList();
		//List<RecordType> instructions = input.Select(i => ParseLine(i)).ToList();
		return "** Solution not written yet **";
	}

	//private static RecordType ParseLine(string input) {
	//	//MatchCollection match = InputRegEx().Matches(input);
	//	Match match = InputRegEx().Match(input);
	//	if (match.Success) {
	//		return new(match.Groups["opts"].Value, int.Parse(match.Groups["number"].Value));
	//	}
	//	return null!;
	//}

	[GeneratedRegex("""(?<opts>opt1|opt2|opt3) (?<number>[\+\-]\d+)""")]
	private static partial Regex InputRegEx();
}
