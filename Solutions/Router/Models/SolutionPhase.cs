//using static AdventOfCode.Solutions.SolutionResultType;
namespace AdventOfCode.Solutions;

public record struct SolutionPhase(string Phase, string Answer, TimeSpan Elapsed, Exception? Exception = null)
{
	public SolutionPhase(string Phase, string Answer) : this(Phase, Answer, TimeSpan.Zero) { }
	public SolutionPhase(string Phase)                : this(Phase, "", TimeSpan.Zero) { }

	public static SolutionPhase ExceptionPart(string phase) => new($"{EXCEPTION}{phase}", EXCEPTION_MESSAGE, TimeSpan.Zero);
	public static SolutionPhase NoInput      => new(PHASE_SOLUTION,  NO_INPUT_MESSAGE,      TimeSpan.Zero);
	public static SolutionPhase NoSolution   => new(PHASE_SOLUTION,  NO_SOLUTION_MESSAGE,   TimeSpan.Zero);
	public static SolutionPhase NoParameters => new(PHASE_SOLUTION,  NO_PARAMETERS_MESSAGE, TimeSpan.Zero);


	public static implicit operator (string Phase, string Answer, TimeSpan Elapsed)(SolutionPhase value)
		=> (value.Phase, value.Answer, value.Elapsed);

	public static implicit operator SolutionPhase((string Phase, string Answer, TimeSpan Elapsed) value)
		=> new(value.Phase, value.Answer, value.Elapsed);
}
