namespace AdventOfCode.Web.Components.AoC;

/// <summary>
/// Represents the type of drawing command
/// </summary>
public enum DrawingCommandType
{
	/// <summary>
	/// Initialize or resize the canvas
	/// </summary>
	Canvas,

	/// <summary>
	/// Set the current drawing color
	/// </summary>
	Colour,

	/// <summary>
	/// Move the pen to a new position without drawing
	/// </summary>
	Move,

	/// <summary>
	/// Draw a line from one point to another
	/// </summary>
	Line,

	/// <summary>
	/// Draw a point at a specific location
	/// </summary>
	Point
}

/// <summary>
/// Represents a single drawing command with its parameters
/// </summary>
public record DrawingCommand(
	DrawingCommandType Type,
	double X1 = 0,
	double Y1 = 0,
	double X2 = 0,
	double Y2 = 0,
	string Colour = "white"
);
