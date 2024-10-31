namespace AdventOfCode.Solutions;

public record struct SolutionPhaseResult(string Phase, string Answer, TimeSpan Elapsed, Exception? Exception = null)
{
	public SolutionPhaseResult(string Phase, string Answer) : this(Phase, Answer, TimeSpan.Zero) { }
	public SolutionPhaseResult(string Phase)                : this(Phase, "", TimeSpan.Zero) { }

	public static SolutionPhaseResult NoInput      => new(PHASE_SOLUTION,  NO_INPUT_MESSAGE,      TimeSpan.Zero);
	public static SolutionPhaseResult NoSolution   => new(PHASE_SOLUTION,  NO_SOLUTION_MESSAGE,   TimeSpan.Zero);
	public static SolutionPhaseResult NoParameters => new(PHASE_SOLUTION,  NO_PARAMETERS_MESSAGE, TimeSpan.Zero);


	public static implicit operator (string Phase, string Answer, TimeSpan Elapsed)(SolutionPhaseResult value)
		=> (value.Phase, value.Answer, value.Elapsed);

	public static implicit operator SolutionPhaseResult((string Phase, string Answer, TimeSpan Elapsed) value)
		=> new(value.Phase, value.Answer, value.Elapsed);
}
