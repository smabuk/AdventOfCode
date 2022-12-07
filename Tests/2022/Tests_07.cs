namespace AdventOfCode.Tests._2022;

public class Tests_07_No_Space_Left_On_Device {
	[Theory]
	[InlineData("""
		$ cd /
		$ ls
		dir a
		14848514 b.txt
		8504156 c.dat
		dir d
		$ cd a
		$ ls
		dir e
		29116 f
		2557 g
		62596 h.lst
		$ cd e
		$ ls
		584 i
		$ cd ..
		$ cd ..
		$ cd d
		$ ls
		4060174 j
		8033020 d.log
		5626152 d.ext
		7214296 k
		"""
		, 95437)]
	public void Part1(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 7, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("""
		$ cd /
		$ ls
		dir a
		14848514 b.txt
		8504156 c.dat
		dir d
		$ cd a
		$ ls
		dir e
		29116 f
		2557 g
		62596 h.lst
		$ cd e
		$ ls
		584 i
		$ cd ..
		$ cd ..
		$ cd d
		$ ls
		4060174 j
		8033020 d.log
		5626152 d.ext
		7214296 k
		"""
		, 24933642)]
	public void Part2(string input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2022, 7, 2, input), out int actual);
		Assert.Equal(expected, actual);
	}
}
