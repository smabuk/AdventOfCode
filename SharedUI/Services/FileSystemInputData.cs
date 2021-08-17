﻿namespace AdventOfCode.Services;

public class FileSystemInputData : IInputDataService {
	public string DataFolder { get; set; } = "../Data";

	public string GetInputData(int year, int day, string? username = null) {

		string filename = string.IsNullOrWhiteSpace(username) switch {
			true => Path.GetFullPath(Path.Combine(DataFolder, $"{year}_{day:D2}.txt")),
			false => Path.GetFullPath(Path.Combine(DataFolder, $"{year}_{day:D2}_{username}.txt"))
		};

		if (File.Exists(filename)) {
			return File.ReadAllText(filename);
		}

		return "";
	}

	public bool SaveInputData(string data, int year, int day, string? username = null) {

		if (string.IsNullOrWhiteSpace(data)) {
			return false;
		}
		string filename = string.IsNullOrWhiteSpace(username) switch {
			true => Path.GetFullPath(Path.Combine(DataFolder, $"{year}_{day:D2}.txt")),
			false => Path.GetFullPath(Path.Combine(DataFolder, $"{year}_{day:D2}_{username}.txt"))
		};

		// If directory doesn't exist then don;t try and write the file
		if (!Directory.Exists(Path.GetFullPath(Path.Combine(DataFolder)))) {
			return false;
		}

		// Don't overwrite the file
		if (!File.Exists(filename)) {
			File.WriteAllText(filename, data);
			return true;
		}

		return false;
	}
}
