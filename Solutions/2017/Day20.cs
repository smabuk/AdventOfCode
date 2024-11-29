using static AdventOfCode.Solutions._2017.Day20Constants;
using static AdventOfCode.Solutions._2017.Day20Types;
namespace AdventOfCode.Solutions._2017;

/// <summary>
/// Day 20: Particle Swarm
/// https://adventofcode.com/2017/day/20
/// </summary>
[Description("Particle Swarm")]
public sealed partial class Day20 {

	[Init]
	public static   void  Init(string[] input, params object[]? args) => LoadParticles(input);
	public static string Part1(string[] input, params object[]? args) => Solution1(input).ToString();
	public static string Part2(string[] input, params object[]? args) => input.Length > 10
		? NO_SOLUTION_WRITTEN_MESSAGE
		: Solution2(input).ToString();

	private static List<Particle> _particles = [];

	private static void LoadParticles(string[] input) => 
		_particles = [
			.. input
			.Index()
			.Select(item => $"{item.Index}, {item.Item}")
			.As<Particle>()];

	private static int Solution1(string[] input) {
		return _particles
			.OrderBy(p => p.Acceleration.ManhattanDistance(Point3d.Zero))
			.ThenBy(p => p.Velocity.ManhattanDistance(Point3d.Zero))
			.ThenBy(p => p.Position.ManhattanDistance(Point3d.Zero))
			.First()
			.Id;
	}

	private static int Solution2(string[] input) {

		List<Particle> particles = [.. _particles];

		for (int iter = 0; iter < 10_000; iter++) {
			for (int i = 0; i < particles.Count; i++) {
				particles[i] = particles[i].Update();
			}

			HashSet<Point3d> dupePositions = [..particles
				.CountBy(p => p.Position)
				.Where(kvp => kvp.Value > 1)
				.Select(kvp => kvp.Key)];

			_ = particles.RemoveAll(p => p.Position.IsIn(dupePositions));

			if (particles.Count == 1) {
				break;
			}
		}

		return particles.Count;
	}
}

file static class Day20Extensions
{
	public static Particle Update(this Particle particle)
	{
		Point3d newVelocity = particle.Velocity + particle.Acceleration;
		Point3d newPosition = particle.Position + particle.Velocity;
		return particle with {
			Position = newPosition,
			Velocity = newVelocity,
		};
	}
}

internal sealed partial class Day20Types
{

	public sealed record Particle(int Id ,Point3d Position, Point3d Velocity, Point3d Acceleration) : IParsable<Particle>
	{
		public static Particle Parse(string s, IFormatProvider? provider)
		{
			string[] tokens = s.TrimmedSplit([',', 'p', 'v', 'a', '=', '<', '>']);
			return new(
				tokens[0].As<int>(),
				new(tokens[1].As<int>(), tokens[2].As<int>(), tokens[3].As<int>()),
				new(tokens[4].As<int>(), tokens[5].As<int>(), tokens[6].As<int>()),
				new(tokens[7].As<int>(), tokens[8].As<int>(), tokens[9].As<int>())
				);
		}

		public static Particle Parse(string s) => Parse(s, null);
		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Particle result)
			=> ISimpleParsable<Particle>.TryParse(s, provider, out result);
	}
}

file static class Day20Constants
{
}
