namespace AdventOfCode.Solutions._2015;

/// <summary>
/// Day 15: Science for Hungry People
/// https://adventofcode.com/2015/day/15
/// </summary>
[Description("Science for Hungry People")]
public class Day15 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record Ingredient(string Name, int Capacity, int Durability, int Flavour, int Texture, int Calories) {
		public static long TotalScore => 0;
	};

	private static long Solution1(string[] input) {
		List<Ingredient> ingredients = input.Select(i => ParseLine(i)).ToList();

		long highScore = 0;
		if (ingredients.Count == 2) {
			Ingredient ingredient1 = ingredients[0];
			Ingredient ingredient2 = ingredients[1];
			for (int i = 1; i <= 100; i++) {
				int qty1 = i;
				int qty2 = 100 - i;
				int capacity = ingredient1.Capacity * qty1 + ingredient2.Capacity * qty2;
				int durability = ingredient1.Durability * qty1 + ingredient2.Durability * qty2;
				int flavour = ingredient1.Flavour * qty1 + ingredient2.Flavour * qty2;
				int texture = ingredient1.Texture * qty1 + ingredient2.Texture * qty2;
				capacity = capacity < 0 ? 0 : capacity;
				durability = durability < 0 ? 0 : durability;
				flavour = flavour < 0 ? 0 : flavour;
				texture = texture < 0 ? 0 : texture;
				long totalScore = capacity * durability * flavour * texture;
				if (totalScore > highScore) {
					highScore = totalScore;
				}
			}
		} else if (ingredients.Count == 4) {
			Ingredient ingredientA = ingredients[0];
			Ingredient ingredientB = ingredients[1];
			Ingredient ingredientC = ingredients[2];
			Ingredient ingredientD = ingredients[3];
			for (int a = 0; a < 100; a++) {
				for (int b = 0; b <= 100; b++) {
					for (int c = 0; c <= 100; c++) {
						for (int d = 0; d < 100; d++) {
							if (a + b + c + d == 100) {
								int capacity =
									ingredientA.Capacity * a
									+ ingredientB.Capacity * b
									+ ingredientC.Capacity * c
									+ ingredientD.Capacity * d;
								int durability =
									ingredientA.Durability * a
									+ ingredientB.Durability * b
									+ ingredientC.Durability * c
									+ ingredientD.Durability * d;
								int flavour =
									ingredientA.Flavour * a
									+ ingredientB.Flavour * b
									+ ingredientC.Flavour * c
									+ ingredientD.Flavour * d;
								int texture =
									ingredientA.Texture * a
									+ ingredientB.Texture * b
									+ ingredientC.Texture * c
									+ ingredientD.Texture * d;
								capacity = capacity < 0 ? 0 : capacity;
								durability = durability < 0 ? 0 : durability;
								flavour = flavour < 0 ? 0 : flavour;
								texture = texture < 0 ? 0 : texture;
								long totalScore = capacity * durability * flavour * texture;
								if (totalScore > highScore) {
									highScore = totalScore;
								}
							}
						}
					}
				}
			}
		}

		return highScore;
	}



	private static long Solution2(string[] input) {
		List<Ingredient> ingredients = input.Select(i => ParseLine(i)).ToList();
		const int CALORIE_COUNT = 500;

		long highScore = 0;
		if (ingredients.Count == 2) {
			Ingredient ingredient1 = ingredients[0];
			Ingredient ingredient2 = ingredients[1];
			for (int i = 1; i <= 100; i++) {
				int qty1 = i;
				int qty2 = 100 - i;
				int calories =
					ingredient1.Calories * qty1
					+ ingredient2.Calories * qty2;
				if (calories == CALORIE_COUNT) {
					int capacity = ingredient1.Capacity * qty1 + ingredient2.Capacity * qty2;
					int durability = ingredient1.Durability * qty1 + ingredient2.Durability * qty2;
					int flavour = ingredient1.Flavour * qty1 + ingredient2.Flavour * qty2;
					int texture = ingredient1.Texture * qty1 + ingredient2.Texture * qty2;
					capacity = capacity < 0 ? 0 : capacity;
					durability = durability < 0 ? 0 : durability;
					flavour = flavour < 0 ? 0 : flavour;
					texture = texture < 0 ? 0 : texture;
					long totalScore = capacity * durability * flavour * texture;
					if (totalScore > highScore) {
						highScore = totalScore;
					}
				}
			}
		} else if (ingredients.Count == 4) {
			Ingredient ingredientA = ingredients[0];
			Ingredient ingredientB = ingredients[1];
			Ingredient ingredientC = ingredients[2];
			Ingredient ingredientD = ingredients[3];
			for (int a = 0; a < 100; a++) {
				for (int b = 0; b <= 100; b++) {
					for (int c = 0; c <= 100; c++) {
						for (int d = 0; d < 100; d++) {
							if (a + b + c + d == 100) {
								int calories =
									ingredientA.Calories * a
									+ ingredientB.Calories * b
									+ ingredientC.Calories * c
									+ ingredientD.Calories * d;
								if (calories == CALORIE_COUNT) {
									int capacity =
										ingredientA.Capacity * a
										+ ingredientB.Capacity * b
										+ ingredientC.Capacity * c
										+ ingredientD.Capacity * d;
									int durability =
										ingredientA.Durability * a
										+ ingredientB.Durability * b
										+ ingredientC.Durability * c
										+ ingredientD.Durability * d;
									int flavour =
										ingredientA.Flavour * a
										+ ingredientB.Flavour * b
										+ ingredientC.Flavour * c
										+ ingredientD.Flavour * d;
									int texture =
										ingredientA.Texture * a
										+ ingredientB.Texture * b
										+ ingredientC.Texture * c
										+ ingredientD.Texture * d;
									capacity = capacity < 0 ? 0 : capacity;
									durability = durability < 0 ? 0 : durability;
									flavour = flavour < 0 ? 0 : flavour;
									texture = texture < 0 ? 0 : texture;
									long totalScore = capacity * durability * flavour * texture;
									if (totalScore > highScore) {
										highScore = totalScore;
									}
								}
							}
						}
					}
				}
			}
		}

		return highScore;
	}

	private static Ingredient ParseLine(string input) {
		Match match = Regex.Match(input, @"(\w+): capacity ([\+\-]*\d+), durability ([\+\-]*\d+), flavor ([\+\-]*\d+), texture ([\+\-]*\d+), calories ([\+\-]*\d+)");
		if (match.Success) {
			return new Ingredient(
				match.Groups[1].Value,
				int.Parse(match.Groups[2].Value),
				int.Parse(match.Groups[3].Value),
				int.Parse(match.Groups[4].Value),
				int.Parse(match.Groups[5].Value),
				int.Parse(match.Groups[6].Value)
				);
		}
		return null!;
	}
}
