using static AdventOfCode.Solutions.SolutionResultType;
namespace AdventOfCode.Solutions;

public record struct SolutionPhase(SolutionResultType Result, string Phase, string Answer, TimeSpan Elapsed) {
	public SolutionPhase(string Phase, string Answer) : this(SUCCESS, Phase, Answer, new()) { }
	public SolutionPhase(string Phase)                : this(SUCCESS, Phase, "",     new()) { }

	//public static readonly string EXCEPTION_MESSAGE     = "** Exception **";
	//public static readonly string NO_SOLUTION_MESSAGE   = "* No solution *";
	//public static readonly string NO_INPUT_MESSAGE      = "* NO INPUT DATA *";
	//public static readonly string NO_PARAMETERS_MESSAGE = "* NO PARAMETERS *";

	//public static readonly string PHASE_SOLUTION = "Solution";
	//public static readonly string PHASE_INIT     = "Init";
	//public static readonly string PHASE_PART1    = "Part1";
	//public static readonly string PHASE_PART2    = "Part2";
	//public static readonly string EXCEPTION_PART1 = "ExceptionPart1";
	//public static readonly string EXCEPTION_PART2 = "ExceptionPart2";

	public static SolutionPhase NoInput         => new(NO_INPUT,      PHASE_SOLUTION,  NO_INPUT_MESSAGE,      new());
	public static SolutionPhase NoSolution      => new(NO_SOLUTION,   PHASE_SOLUTION,  NO_SOLUTION_MESSAGE,   new());
	public static SolutionPhase NoSolutionPart1 => new(NO_SOLUTION,   PHASE_PART1,     NO_SOLUTION_MESSAGE,   new());
	public static SolutionPhase NoSolutionPart2 => new(NO_SOLUTION,   PHASE_PART2,     NO_SOLUTION_MESSAGE,   new());
	public static SolutionPhase NoParameters    => new(NO_PARAMETERS, PHASE_SOLUTION,  NO_PARAMETERS_MESSAGE, new());
	public static SolutionPhase ExceptionPart1  => new(EXCEPTION,     EXCEPTION_PART1, EXCEPTION_MESSAGE,     new());
	public static SolutionPhase ExceptionPart2  => new(EXCEPTION,     EXCEPTION_PART2, EXCEPTION_MESSAGE,     new());

	public Exception? Exception{ get; init; }
	//public void SetException(Exception exception) => Exception = exception;

	public static implicit operator (SolutionResultType Result, string Phase, string Answer, TimeSpan Elapsed)(SolutionPhase value)
		=> (value.Result, value.Phase, value.Answer, value.Elapsed);

	public static implicit operator SolutionPhase((SolutionResultType Result, string Phase, string Answer, TimeSpan Elapsed) value)
		=> new(value.Result, value.Phase, value.Answer, value.Elapsed);
}
