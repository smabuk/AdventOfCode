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

		LinkedList<int> recipes = new([3, 7]);
		string tail = "37";
		List<LinkedListNode<int>> elfCurrentRecipes = [recipes.First!, recipes.Last!];

		do {
			_ = CreateNewRecipes(recipes, elfCurrentRecipes, tail);
		} while (recipes.Count < noOfRecipes + 10);

		return string.Join("", recipes.Skip(noOfRecipes).Take(10));
	}

	private static int Solution2(string[] input) {
		string recipesToFind = input[0];

		LinkedList<int> recipes = new([3, 7]);
		string tail = "37";
		List<LinkedListNode<int>> elfCurrentRecipes = [recipes.First!, recipes.Last!];

		do {
			tail = CreateNewRecipes(recipes, elfCurrentRecipes, tail);
		} while (!tail.Contains(recipesToFind));

		return string.Join("", recipes).IndexOf(recipesToFind);
	}

	private static string CreateNewRecipes(LinkedList<int> recipes, List<LinkedListNode<int>> elfCurrentRecipes, string tail)
	{
		string newRecipes = elfCurrentRecipes.Select(x => x.Value).Sum().ToString();
		for (int i = 0; i < newRecipes.Length; i++) {
			_ = recipes.AddLast(newRecipes[i].ToString().AsInt());
			tail += newRecipes[i];
		}

		for (int i = 0; i < elfCurrentRecipes.Count; i++) {
			LinkedListNode<int> recipe = elfCurrentRecipes[i];
			int noOfSteps = recipe.Value + 1;
			for (int steps = 0; steps < noOfSteps; steps++) {
				recipe = recipe.Next ?? recipes.First!;
			}
			elfCurrentRecipes[i] = recipe;
		}

		return tail.Length > 9 ? tail[^9..] : tail;
	}


}
