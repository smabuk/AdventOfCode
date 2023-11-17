namespace AdventOfCode.Solutions._2018;

/// <summary>
/// Day 14: Chocolate Charts
/// https://adventofcode.com/2018/day/14
/// </summary>
[Description("Chocolate Charts")]
public sealed partial class Day14 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static string Solution1(string[] input) {
		int noOfRecipes = input[0].AsInt();

		List<int> recipes = new([3, 7]);
		string tail = "37";
		List<int> elfCurrentRecipes = [0, 1];

		do {
			_ = CreateNewRecipes(recipes, elfCurrentRecipes, tail);
		} while (recipes.Count < noOfRecipes + 10);

		return string.Join("", recipes.Skip(noOfRecipes).Take(10));
	}

	private static int Solution2(string[] input) {
		string recipesToFind = input[0];

		List<int> recipes = new([3, 7]);
		string tail = "37";
		List<int> elfCurrentRecipes = [0, 1];

		do {
			tail = CreateNewRecipes(recipes, elfCurrentRecipes, tail);
		} while (!tail.Contains(recipesToFind));

		return string.Join("", recipes).IndexOf(recipesToFind);
	}

	private static string CreateNewRecipes(List<int> recipes, List<int> elfCurrentRecipes, string tail)
	{
		int newRecipes = recipes[elfCurrentRecipes[0]] + recipes[elfCurrentRecipes[1]];
		if (newRecipes > 9) {
			recipes.Add(1);
			recipes.Add(newRecipes - 10);
		} else {
			recipes.Add(newRecipes);
		}
		tail += newRecipes;

		for (int i = 0; i < elfCurrentRecipes.Count; i++) {
			int recipe = recipes[elfCurrentRecipes[i]];
			int noOfSteps = recipe + 1;
			elfCurrentRecipes[i] = (elfCurrentRecipes[i] + noOfSteps) % recipes.Count;
		}

		return tail.Length > 9 ? tail[^9..] : tail;
	}


}
