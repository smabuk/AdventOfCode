namespace AdventOfCode.Solutions._2020;

/// <summary>
/// Day 16: Ticket Translation
/// https://adventofcode.com/2020/day/16
/// </summary>
[Description("Ticket Translation")]
public class Day16 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	record Input(IEnumerable<TicketField> TicketFields, Ticket YourTicket, IEnumerable<Ticket> NearbyTickets);
	record TicketField(string Name, List<int> AllowedValues);
	record Ticket(int[] Values);

	private static int Solution1(string[] input) {
		Input instructions = Parse(input);

		List<int> allowedValues = instructions
			.TicketFields.SelectMany(i => i.AllowedValues)
			.ToList();

		int ticketScanningErrorRate = instructions
			.NearbyTickets.SelectMany(i => i.Values)
			.Where(i => !allowedValues.Contains(i))
			.Sum();

		return ticketScanningErrorRate;
	}

	private static long Solution2(string[] input) {
		Input instructions = Parse(input);

		List<int> allowedValues = instructions
			.TicketFields.SelectMany(i => i.AllowedValues)
			.ToList();

		List<Ticket> validTickets = new();

		foreach (Ticket ticket in instructions.NearbyTickets) {
			if (ticket.Values.All(t => allowedValues.Contains(t))) {
				validTickets.Add(ticket);
			}
		}

		int noOfFields = instructions.TicketFields.Count();
		List<(string name, int position)> answers = new();

		foreach (TicketField tf in instructions.TicketFields) {
			for (int i = 0; i < noOfFields; i++) {
				IEnumerable<int> values = validTickets.Select(x => x.Values[i]);
				if (values.All(x => tf.AllowedValues.Contains(x))) {
					answers.Add((tf.Name, i));
				}
			}
		}

		List<long> departureValues = new();

		for (int i = 0; i < noOfFields; i++) {
			var names = answers
				.GroupBy(x => x.name)
				.Select(group => new { Name = group.Key, Count = group.Count() })
				.Where(g => g.Count == 1);
			foreach (var item in names) {
				int position = answers.
					Where(x => x.name == item.Name)
					.SingleOrDefault().position;
				_ = answers.RemoveAll(a => a.name == item.Name);
				_ = answers.RemoveAll(a => a.position == position);
				if (item.Name.StartsWith("departure")) {
					departureValues.Add(instructions.YourTicket.Values[position]);
				}
			}
		}
		return departureValues.Aggregate((long x, long y) => x * y);
	}


	private static Input Parse(string[] input) {
		string line;
		int i = 0;
		List<TicketField> ticketFields = new();
		List<Ticket> nearbyTickets = new();
		while (!string.IsNullOrEmpty(input[i])) {
			line = input[i];
			Match match = Regex.Match(line, @"(?<name>[ \w]+): (?<r1lower>\d+)-(?<r1upper>\d+) or (?<r2lower>\d+)-(?<r2upper>\d+)");
			if (match.Success) {
				int r1lower = int.Parse(match.Groups["r1lower"].Value);
				int r1upper = int.Parse(match.Groups["r1upper"].Value);
				int r2lower = int.Parse(match.Groups["r2lower"].Value);
				int r2upper = int.Parse(match.Groups["r2upper"].Value);
				List<int> values =
					Enumerable.Range(r1lower, r1upper + 1 - r1lower)
					.Select(i => i)
					.Union(
						Enumerable.Range(r2lower, r2upper + 1 - r2lower)
						.Select(i => i))
					.ToList();
				ticketFields.Add(new(match.Groups["name"].Value, values));
			}
			i++;
		}

		i += 2;
		line = input[i];
		Ticket yourTicket = new(line
			.Split(",")
			.Select(i => int.Parse(i)).ToArray());

		for (i += 3; i < input.Length; i++) {
			line = input[i];
			nearbyTickets.Add(new(line
							.Split(",")
							.Select(i => int.Parse(i)).ToArray()));
		}

		return new(ticketFields, yourTicket, nearbyTickets);
	}
}
