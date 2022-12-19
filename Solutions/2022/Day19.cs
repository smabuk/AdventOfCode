namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 19: Not Enough Minerals
/// https://adventofcode.com/2022/day/19
/// </summary>
[Description("Not Enough Minerals")]
public sealed partial class Day19 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		List<Blueprint> blueprints = input.Select(Blueprint.Parse).ToList();
		return blueprints
			.Select(Factory.MaxQualityLevel)
			.Sum();

		// 4724 too high - works for sample input
	}

	private static string Solution2(string[] input) {
		return "** Solution not written yet **";
	}


	private record Factory() {
		private static List<ChoicesState> choicesStates = new();

		public static int MaxQualityLevel(Blueprint blueprint) {
			int max = int.MinValue;
			for (int i = 0; i < 100_000; i++) {
				max = Math.Max(max, QualityLevel(blueprint, true));
			}
			return max;
		}


		public static int QualityLevel(Blueprint blueprint, bool tryMonteCarlo = false) {
			Random TrueOrFalse = new();
			Dictionary<Resource, int> myResources = new() {
				{ Resource.ore, 0 },
				{ Resource.clay, 0 },
				{ Resource.obsidian, 0 },
				{ Resource.geode, 0 }
			};

			List<Robot> robots = new() {
				new(Resource.ore)
			};

			Enum.GetValues<Resource>();
			for (int minute = 0; minute < 24; minute++) {
				List<Robot> building = new();

				if (CanIBuild(Resource.geode)) {
					building.Add(BuildRobot(Resource.geode));
				} else if (CanIBuild(Resource.obsidian)
					//&& myResources[Resource.clay] >= blueprint.RobotCosts[Resource.geode].Costs[Resource.clay].Value
					//&& myResources[Resource.ore]  >= blueprint.RobotCosts[Resource.geode].Costs[Resource.ore].Value
					&& MonteCarlo()
					) {
					building.Add(BuildRobot(Resource.obsidian));
				} else if (CanIBuild(Resource.clay)
				//	&& myResources[Resource.ore] > blueprint.RobotCosts[Resource.obsidian].Costs[Resource.ore].Value
					&& MonteCarlo()
					) {
					building.Add(BuildRobot(Resource.clay));
				} else if (CanIBuild(Resource.ore)
				//	&& myResources[Resource.ore] > blueprint.RobotCosts[Resource.obsidian].Costs[Resource.ore].Value
					&& MonteCarlo()
					) {
					building.Add(BuildRobot(Resource.ore));
				}

				myResources[Resource.ore]      += robots.Count(r => r.Type is Resource.ore);
				myResources[Resource.clay]     += robots.Count(r => r.Type is Resource.clay);
				myResources[Resource.obsidian] += robots.Count(r => r.Type is Resource.obsidian);
				myResources[Resource.geode]    += robots.Count(r => r.Type is Resource.geode);
				robots.AddRange(building);
			}

			return myResources[Resource.geode] * blueprint.Id;

			bool CanIBuild(Resource resource) {
				foreach (Cost cost in blueprint.RobotCosts[resource].Costs.Values) {
					if (myResources[cost.Resource] < cost.Value) {
						return false;
					}
				}
				return true;
			}
	
			Robot BuildRobot(Resource resource) {
				foreach (Cost cost in blueprint.RobotCosts[resource].Costs.Values) {
					myResources[cost.Resource] -= cost.Value;
				}
				return new(resource);
			}

			bool MonteCarlo() => TrueOrFalse.Next(2) == 1;
		}


		private record ChoicesState {
		}
	}



	private record Blueprint(int Id, Dictionary<Resource, RobotCost> RobotCosts) : IParsable<Blueprint> {
		public static Blueprint Parse(string s) {
			string[] tokens = s.Split(new char[] { ':', '.' });
			int id = tokens[0].Split(' ').Last().Trim().AsInt();
			Dictionary<Resource, RobotCost> robots = tokens[1..]
				.Where(s => string.IsNullOrWhiteSpace(s) is false)
				.Select(RobotCost.Parse)
				.ToDictionary(c => c.Name, c => c);
			return new(id, robots);
		}
		public static Blueprint Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Blueprint result) => throw new NotImplementedException();
	}

	private record RobotCost(Resource Name, Dictionary<Resource, Cost> Costs) : IParsable<RobotCost> {
		public static RobotCost Parse(string s) {
			string[] tokens = s.Split(new char[] { ' ' });
			Resource name = Enum.Parse<Resource>(tokens[2]);
			Dictionary<Resource, Cost> costs = new() {
				{ Resource.ore,      new(Resource.ore,      0) },
				{ Resource.clay,     new(Resource.clay,     0) },
				{ Resource.obsidian, new(Resource.obsidian, 0) },
				{ Resource.geode,    new(Resource.geode,    0) }
			};
			foreach (var cost in Cost.Parse(tokens[4..]).ToDictionary(c => c.Resource, c => c)) {
				costs[cost.Key] = cost.Value;
			}
			return new(name, costs);
		}

		public static RobotCost Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out RobotCost result) => throw new NotImplementedException();
	}

	private record Cost(Resource Resource, int Value) : IParsable<Cost> {
		public static IEnumerable<Cost> Parse(string[] s) {
			for (int i = 0; i < s.Length; i++) {
				if (Char.IsNumber(s[i][0])) {
					yield return new(Enum.Parse<Resource>(s[i + 1]), s[i].AsInt());
					i += 2;
				}
			}
		}
		public static Cost Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Cost result) => throw new NotImplementedException();
	}

	private record Robot(Resource Type);

	private enum Resource {
		ore,
		clay,
		obsidian,
		geode
	}

}


