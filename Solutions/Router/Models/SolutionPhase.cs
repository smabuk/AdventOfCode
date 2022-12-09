using static AdventOfCode.Solutions.SolutionResultType;
namespace AdventOfCode.Solutions;

public record struct SolutionPhase(SolutionResultType Result, string Phase, string Answer, TimeSpan Elapsed) {
	public SolutionPhase(string Phase, string Answer) : this(SUCCESS, Phase, Answer, new()) { }
	public SolutionPhase(string Phase) : this(SUCCESS, Phase, "", new()) { }

	private const string NO_SOLUTION_MESSAGE = "** No solution **";
	private const string NO_INPUT_MESSAGE = "** NO INPUT DATA **";
	private const string NO_PARAMETERS_MESSAGE = "** NO PARAMETERS **";

	public static SolutionPhase NoInput => new(NO_INPUT, "Solution", NO_INPUT_MESSAGE, new());
	public static SolutionPhase NoSolution => new(NO_SOLUTION, "Solution", NO_SOLUTION_MESSAGE, new());
	public static SolutionPhase NoSolutionPart1 => new(NO_SOLUTION, "Part1", NO_SOLUTION_MESSAGE, new());
	public static SolutionPhase NoSolutionPart2 => new(NO_SOLUTION, "Part2", NO_SOLUTION_MESSAGE, new());
	public static SolutionPhase NoParameters => new(NO_PARAMETERS, "Solution", NO_PARAMETERS_MESSAGE, new());


	public static implicit operator (SolutionResultType Result, string Phase, string Answer, TimeSpan Elapsed)(SolutionPhase value) {
		return (value.Result, value.Phase, value.Answer, value.Elapsed);
	}

	public static implicit operator SolutionPhase((SolutionResultType Result, string Phase, string Answer, TimeSpan Elapsed) value) {
		return new SolutionPhase(value.Result, value.Phase, value.Answer, value.Elapsed);
	}
}
