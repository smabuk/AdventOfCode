using System.Text.Json;

namespace AdventOfCode.Solutions._2022;

/// <summary>
/// Day 13: Distress Signal
/// https://adventofcode.com/2022/day/13
/// </summary>
[Description("Distress Signal")]
public sealed partial class Day13 {

	public static string Part1(string[] input, params object[]? _) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? _) => Solution2(input).ToString();

	private static int Solution1(string[] input) {
		return input
			.Chunk(3)
			.Select(chunk => new Pair(chunk[0], chunk[1]))
			.Select((pair, index) => (p: pair, ix: index))
			.Where((item, index) => item.p.Left.CompareTo(item.p.Right) < 0)
			.Select(x => x.ix + 1)
			.Sum();
	}

	private static int Solution2(string[] input) {
		List<Packet> packets = input
			.Where(i => !string.IsNullOrWhiteSpace(i))
			.Select(i => Packet.Parse(i))
			.ToList();

		Packet p2 = Packet.Parse("[[2]]");
		Packet p6 = Packet.Parse("[[6]]");
		packets.Add(p2);
		packets.Add(p6);

		packets.Sort();

		return (packets.IndexOf(p2) + 1)
			 * (packets.IndexOf(p6) + 1);
	}

	private record Pair(Packet Left, Packet Right) {
		public Pair(string left, string right) : this(Packet.Parse(left), Packet.Parse(right)) { }
	};

	private abstract record Packet : IComparable, IEquatable<Packet>, IParsable<Packet> {
		public static Packet Parse(string value, IFormatProvider? provider = null)
			=> ParseElement(JsonSerializer.Deserialize<JsonElement>(value));

		private static Packet ParseElement(JsonElement el) {
			Packet value = el.ValueKind switch {
				JsonValueKind.Number => new NumberPacket(el.GetInt32()),
				JsonValueKind.Array => new ListPacket(el.EnumerateArray().Select(ParseElement).ToList()),
				_ => throw new NotImplementedException(),
			};
			return value;
		}

		public int CompareTo(object? obj) {
			int RIGHT_ORDER = -1;
			int WRONG_ORDER = 1;
			int UNSURE = 0;
			Packet left = this;
			Packet right = (Packet)obj!;

			return (left, right) switch {
				(not null, null) => WRONG_ORDER,
				(null, not null) => RIGHT_ORDER,
				(NumberPacket ln, NumberPacket rn) => ln.Value < rn.Value ? RIGHT_ORDER : ln.Value > rn.Value ? WRONG_ORDER : UNSURE,
				(ListPacket, NumberPacket) => left.CompareTo(new ListPacket(new() { right })),
				(NumberPacket, ListPacket) => new ListPacket(new() { left }).CompareTo(right),
				(ListPacket lp, ListPacket rp) => ProcessLists(lp, rp),
				_ => UNSURE,
			};

			int ProcessLists(ListPacket left, ListPacket right) {
				for (int i = 0; i < left.Packets.Count; i++) {
					if (right.Packets.Count <= i) {
						return WRONG_ORDER;
					}
					int result = left.Packets[i].CompareTo(right.Packets[i]);
					if (left.Packets[i].CompareTo(right.Packets[i]) != UNSURE) {
						return result;
					};
				}
				if (left.Packets.Count == right.Packets.Count) {
					return UNSURE;
				}
				return RIGHT_ORDER;
			}
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Packet result) => throw new NotImplementedException();
	}

	private record NumberPacket(int Value) : Packet {
		public override string ToString() => $"{Value}";
	}

	private record ListPacket(List<Packet> Packets) : Packet {
		public override string ToString() => $"[{String.Join(",", Packets)}]";
	};


}
