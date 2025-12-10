namespace AdventOfCode.Web.Components.AoC;

/// <summary>
/// Represents a frame in the visualiser output history
/// </summary>
public record VisualiserFrame(
	string TextOutput,
	List<DrawingCommand> DrawingCommands,
	int CanvasWidth,
	int CanvasHeight
)
{
	/// <summary>
	/// Gets whether this frame contains any drawing commands
	/// </summary>
	public bool HasDrawing => DrawingCommands.Count > 0;
}
