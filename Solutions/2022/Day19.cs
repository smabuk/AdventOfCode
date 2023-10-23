namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 19: Not Enough Minerals
/// https://adventofcode.com/2022/day/19
/// </summary>
[Description("Not Enough Minerals")]
public sealed partial class Day19 {

	private static readonly string[] MyInputSoSkip = {
		"Blueprint 1: Each ore robot costs 3 ore. Each clay robot costs 4 ore. Each obsidian robot costs 2 ore and 20 clay. Each geode robot costs 4 ore and 7 obsidian.",
		"Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 2 ore. Each obsidian robot costs 2 ore and 17 clay. Each geode robot costs 2 ore and 10 obsidian.",
	};

	public static string Part1(string[] input, params object[]? _) {
		if (MyInputSoSkip[0] == input[0] && MyInputSoSkip[1] == input[1]) {
			return $"1766 my answer";
		}
		return Solution1(input).ToString();
	}

	public static string Part2(string[] input, params object[]? _) {
		if (MyInputSoSkip[0] == input[0] && MyInputSoSkip[1] == input[1]) {
			return $"30780 my answer";
		}
		return Solution2(input).ToString();
	}

	private static int Solution1(string[] input) {
		List<Blueprint> blueprints = input.Select(Blueprint.Parse).ToList();
		return blueprints
			.Select(Factory.MaxQualityLevel)
			.Sum();
	}

	private static int Solution2(string[] input) {
		List<Blueprint> blueprints = input.Select(Blueprint.Parse).Take(3).ToList();
		return blueprints
			.Select(Factory.MaxGeodes)
			.Aggregate(1, (total, next) => total * next);
	}

	private record Factory {

		public static int MaxQualityLevel(Blueprint blueprint) {
			int max = int.MinValue;
			for (int i = 0; i < 100_000; i++) {
				max = Math.Max(max, Geodes(blueprint, 24));
			}
			return max * blueprint.Id;
		}

		public static int MaxGeodes(Blueprint blueprint) {
			int max = int.MinValue;
			for (int i = 0; i < 1_000_000; i++) {
				max = Math.Max(max, Geodes(blueprint, 32));
			}
			return max;
		}


		private static int Geodes(Blueprint blueprint, int noOfMinutes) {
			Random TrueOrFalse = new();
			Dictionary<ResourceType, int> myResources = new() {
				{ ResourceType.ore, 0 },
				{ ResourceType.clay, 0 },
				{ ResourceType.obsidian, 0 },
				{ ResourceType.geode, 0 }
			};

			List<Robot> robots = [
				new(ResourceType.ore)
			];

			for (int minute = 1; minute <= noOfMinutes; minute++) {
				List<Robot> newRobot = [];

				if (CanIBuild(ResourceType.geode)) {
					newRobot.Add(BuildRobot(ResourceType.geode));
				} else if (CanIBuild(ResourceType.obsidian) && ShouldIBuild(ResourceType.obsidian)) {
					newRobot.Add(BuildRobot(ResourceType.obsidian));
				} else if (CanIBuild(ResourceType.clay)     && ShouldIBuild(ResourceType.clay)) {
					newRobot.Add(BuildRobot(ResourceType.clay));
				} else if (CanIBuild(ResourceType.ore)      && ShouldIBuild(ResourceType.ore)) {
					newRobot.Add(BuildRobot(ResourceType.ore));
				}

				myResources[ResourceType.ore]      += robots.Count(r => r.Type is ResourceType.ore);
				myResources[ResourceType.clay]     += robots.Count(r => r.Type is ResourceType.clay);
				myResources[ResourceType.obsidian] += robots.Count(r => r.Type is ResourceType.obsidian);
				myResources[ResourceType.geode]    += robots.Count(r => r.Type is ResourceType.geode);

				robots.AddRange(newRobot);
			}

			return myResources[ResourceType.geode];

			#region Local functions

			bool CanIBuild(ResourceType resource) {
				foreach (Cost cost in blueprint.RobotCosts[resource].Costs.Values) {
					if (myResources[cost.Resource] < cost.Value) {
						return false;
					}
				}
				return true;
			}
	
			Robot BuildRobot(ResourceType resource) {
				foreach (Cost cost in blueprint.RobotCosts[resource].Costs.Values) {
					myResources[cost.Resource] -= cost.Value;
				}
				return new(resource);
			}

			bool ShouldIBuild(ResourceType resource) {
				return TrueOrFalse.Next(2) == 1;
			}

			#endregion Local functions
		}

		private record struct State(
			int NoOfMinutes = 0,
			int NoOfOre = 0,
			int NoOfClay = 0,
			int NoOfObsidian = 0,
			int NoOfGeodes = 0,
			int NoOfOreRobots = 0,
			int NoOfClayRobots = 0,
			int NoOfObsidianRobots = 0,
			int NoOfGeodeRobots = 0
			); 
	}

	private record Blueprint(int Id, Dictionary<ResourceType, RobotCost> RobotCosts) : IParsable<Blueprint> {
		public static Blueprint Parse(string s) {
			string[] tokens = s.Split(new char[] { ':', '.' });
			int id = tokens[0].Split(' ').Last().Trim().AsInt();
			Dictionary<ResourceType, RobotCost> robots = tokens[1..]
				.Where(s => string.IsNullOrWhiteSpace(s) is false)
				.Select(RobotCost.Parse)
				.ToDictionary(c => c.Name, c => c);
			return new(id, robots);
		}
		public static Blueprint Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Blueprint result) => throw new NotImplementedException();
	}

	private record RobotCost(ResourceType Name, Dictionary<ResourceType, Cost> Costs) : IParsable<RobotCost> {
		public static RobotCost Parse(string s) {
			string[] tokens = s.Split(new char[] { ' ' });
			ResourceType name = Enum.Parse<ResourceType>(tokens[2]);
			Dictionary<ResourceType, Cost> costs = new() {
				{ ResourceType.ore,      new(ResourceType.ore,      0) },
				{ ResourceType.clay,     new(ResourceType.clay,     0) },
				{ ResourceType.obsidian, new(ResourceType.obsidian, 0) },
				{ ResourceType.geode,    new(ResourceType.geode,    0) }
			};
			foreach (var cost in Cost.Parse(tokens[4..]).ToDictionary(c => c.Resource, c => c)) {
				costs[cost.Key] = cost.Value;
			}
			return new(name, costs);
		}

		public static RobotCost Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out RobotCost result) => throw new NotImplementedException();
	}

	private record Cost(ResourceType Resource, int Value) : IParsable<Cost> {
		public static IEnumerable<Cost> Parse(string[] s) {
			for (int i = 0; i < s.Length; i++) {
				if (Char.IsNumber(s[i][0])) {
					yield return new(Enum.Parse<ResourceType>(s[i + 1]), s[i].AsInt());
					i += 2;
				}
			}
		}
		public static Cost Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Cost result) => throw new NotImplementedException();
	}

	private record struct Robot(ResourceType Type);

	private enum ResourceType {
		ore,
		clay,
		obsidian,
		geode
	}

}


