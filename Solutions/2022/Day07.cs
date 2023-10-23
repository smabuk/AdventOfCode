﻿namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 07: No Space Left On Device
/// https://adventofcode.com/2022/day/7
/// </summary>
[Description("No Space Left On Device")]
public sealed class Day07 {

	[Init]
	public static    void Init(string[] input, params object[]? _) => ParseAndCreate(input);
	public static string Part1(string[] input, params object[]? _) => Solution1().ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2().ToString();

	private static Directory _fileSystem = null!;

	private static int Solution1() {
		return TotalOfSubDirectoriesWithSizeLessThan(_fileSystem, 100_000);
	}

	private static int Solution2() {
		const int DiskSpaceAvailable  = 70_000_000;
		const int RequiredUnusedSpace = 30_000_000;

		int spaceToFreeUp = RequiredUnusedSpace - (DiskSpaceAvailable - _fileSystem.Size);

		return GetAllSubDirectories(_fileSystem)
			.Select(x => x.Size)
			.Where(x => x >= spaceToFreeUp)
			.Min();
	}

	private static Directory ParseAndCreate(string[] terminalOutput) {
		_fileSystem = new("/", null!);
		Directory currentDirectory = _fileSystem;
		for (int i = 0; i < terminalOutput.Length; i++) {
			string output = terminalOutput[i];
			if (output.StartsWith("$ cd")) {
				string directoryName = output[5..];
				if (directoryName == "/") {
					currentDirectory = _fileSystem;
				} else if (directoryName == "..") {
					currentDirectory = currentDirectory.Parent;
				} else {
					currentDirectory.AddDirectory(new(directoryName));
					currentDirectory = currentDirectory.Directories[directoryName];
				}
			} else if (output.StartsWith("dir")) {
				string directoryName = output[4..];
				currentDirectory.AddDirectory(new(directoryName));
		} else if (Char.IsNumber(output[0])) {
				string[] tokens = output.Split(" ");
				currentDirectory.AddFile(tokens[1], tokens[0].AsInt());
			}
		}
		return _fileSystem;
	}

	private static int TotalOfSubDirectoriesWithSizeLessThan(Directory directory, int targetSize) {
		int size = directory.Size <= targetSize ? directory.Size : 0;
		foreach (Directory subDir in directory.Directories.Values) {
			size += TotalOfSubDirectoriesWithSizeLessThan(subDir, targetSize);
		}
		return size;
	}

	private static List<Directory> GetAllSubDirectories(Directory directory) {
		List<Directory> subDirs = [directory];
		foreach (Directory subDir in directory.Directories.Values) {
			subDirs.AddRange(GetAllSubDirectories(subDir));
		}
		return subDirs;
	}


	private record File(string Name, int Size);
	
	private record Directory(string Name, Directory Parent) {
		public Dictionary<string, Directory> Directories { get; set; } = [];
		public Dictionary<string, File> Files { get; set; } = [];

		public void AddDirectory(string name) {
			_ = Directories.TryAdd(name, new Directory(name, this));
		}

		public void AddFile(string name, int size) {
			_ = Files.TryAdd(name, new(name, size));
		}

		public int Size => Directories.Sum(x => x.Value.Size) + Files.Sum(x => x.Value.Size);
	};


}
