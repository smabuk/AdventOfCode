using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdventOfCode.Solutions.Helpers;

using static AdventOfCode.Solutions.Helpers.ArgumentHelpers;

namespace AdventOfCode.Solutions.Year2020 {
	/// <summary>
	/// Day 16: Ticket Translation
	/// https://adventofcode.com/2020/day/16
	/// </summary>
	public class Day16 {

		record Input(IEnumerable<TicketField> TicketFields, Ticket YourTicket, IEnumerable<Ticket> NearbyTickets);
		record TicketField(string Name, int Range1Lower, int Range1Upper, int Range2Lower, int Range2Upper, List<int> AllowedValues);
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

		private static string Solution2(string[] input) {
			Input instructions = Parse(input);
			return "** Solution not written yet **";
		}


		private static Input Parse(string[] input) {
			int i = 0;
			List<TicketField> ticketFields = new();
			List<Ticket> nearbyTickets = new();
			while (!string.IsNullOrEmpty(input[i])) {
				string name = input[i].Split(": ")[0];
				string[] ranges = input[i].Split(": ")[1].Split(" or ");
				TicketField tf = new(name,
					int.Parse(ranges[0].Split("-")[0]),
					int.Parse(ranges[0].Split("-")[1]),
					int.Parse(ranges[1].Split("-")[0]),
					int.Parse(ranges[1].Split("-")[1]),
					new()
					);
				List<int> values = Enumerable.Range(tf.Range1Lower, tf.Range1Upper + 1 - tf.Range1Lower).Select(i => i)
					.Union(Enumerable.Range(tf.Range2Lower, tf.Range2Upper + 1 - tf.Range2Lower).Select(i => i)).ToList();
				ticketFields.Add(tf with { AllowedValues = values });
				i++;
			}

			i += 2;
			Ticket yourTicket = new(input[i].Split(",").Select( i => int.Parse(i)).ToArray());


			for (i += 3; i < input.Length; i++) {
				nearbyTickets.Add(new(input[i].Split(",").Select( i => int.Parse(i)).ToArray()));
			}


			Input theInput = new(ticketFields, yourTicket, nearbyTickets);
			return theInput;
		}





		#region Problem initialisation
		public static string Part1(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution1(input).ToString();
		}
		public static string Part2(string[]? input, params object[]? args) {
			if (input is null) { return "Error: No data provided"; }
			input = input.StripTrailingBlankLineOrDefault();
			return Solution2(input).ToString();
		}
		#endregion

	}
}
