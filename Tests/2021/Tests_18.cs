using static AdventOfCode.Solutions.Year2021.Day18;
namespace AdventOfCode.Tests.Year2021;

public class Tests_18_Snailfish {
	[Theory]
	[InlineData(new string[] {
		"[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]",
		"[[[5,[2,8]],4],[5,[[9,9],0]]]",
		"[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]",
		"[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]",
		"[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]",
		"[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]",
		"[[[[5,4],[7,7]],8],[[8,3],8]]",
		"[[9,3],[[9,9],[6,[4,9]]]]",
		"[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
		"[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]",
	}, 4140)]
	public void Part1(string[] input, int expected) {
		_ = int.TryParse(SolutionRouter.SolveProblem(2021, 18, 1, input), out int actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(new string[] { "target area: x=20..30, y=-10..-5" }, 112)]
	[InlineData(new string[] { "target area: x=-20..-30, y=-10..-5" }, 112)]
	public void Part2(string[] input, long expected) {
		_ = long.TryParse(SolutionRouter.SolveProblem(2021, 18, 2, input), out long actual);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("[1,2]", "[[3,4],5]", "[[1,2],[[3,4],5]]")]
	[InlineData("[[[[4,3],4],4],[7,[[8,4],9]]]", "[1,1]", "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]")]
	[InlineData("[1,1]", "[2,2]", "[[1,1],[2,2]]")]
	[InlineData("[[1,1],[2,2]]", "[3,3]", "[[[1,1],[2,2]],[3,3]]")]
	[InlineData("[[[1,1],[2,2]],[3,3]]", "[4,4]", "[[[[1,1],[2,2]],[3,3]],[4,4]]")]
	[InlineData("[[[[1,1],[2,2]],[3,3]],[4,4]]", "[5,5]", "[[[[3,0],[5,3]],[4,4]],[5,5]]")]
	[InlineData("[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]", "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]", "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]")]

	public void Snailfish_Addition(string lho, string rho, string expected) {
		SnailfishNumber actual = new SnailfishNumber(lho).Plus(new SnailfishNumber(rho));
		Assert.Equal(expected, actual.Number);
	}


	[Theory]
	[InlineData("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]")]
	[InlineData("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]")]
	[InlineData("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]")]
	[InlineData("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]")]
	[InlineData("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
	public void Snailfish_Explode(string input, string expected) {
		string actual = SnailfishNumber.Explode(input);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("[[1,2],[[3,4],5]]", 143)]
	[InlineData("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]", 1384)]
	[InlineData("[[[[1,1],[2,2]],[3,3]],[4,4]]", 445)]
	[InlineData("[[[[3,0],[5,3]],[4,4]],[5,5]]", 791)]
	[InlineData("[[[[5,0],[7,4]],[5,5]],[6,6]]", 1137)]
	[InlineData("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", 3488)]
	[InlineData("[[[[6,6],[7,6]],[[7,7],[7,0]]],[[[7,7],[7,7]],[[7,8],[9,9]]]]", 4140)]
	public void Snailfish_Magnitude(string input, long expected) {
		long actual = new SnailfishNumber(input).Magnitude;
		Assert.Equal(expected, actual);
	}



}
