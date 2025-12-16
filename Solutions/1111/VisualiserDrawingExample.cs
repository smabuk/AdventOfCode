namespace AdventOfCode.Solutions._1111;

/// <summary>
/// Example demonstrating the drawing capabilities of the visualiser component.
/// This is a sample implementation showing how to use canvas-based drawing commands.
/// </summary>
public static class VisualiserDrawingExample
{
	private static Action<string[], bool>? _visualise = null;

	/// <summary>
	/// Initialize the visualiser
	/// </summary>
	public static void InitVisualiser(Action<string[], bool>? visualise) => _visualise = visualise;

	/// <summary>
	/// Example 1: Draw a simple box
	/// </summary>
	public static void DrawBox()
	{
		if (_visualise is null) {
			return;
		}

		// Initialize canvas
		_visualise(["canvas:400:400"], true);

		// Draw a red box
		_visualise([
			"colour:red",
			"line:50:50:350:50",
			"line:350:50:350:350",
			"line:350:350:50:350",
			"line:50:350:50:50"
		], false);
	}

	/// <summary>
	/// Example 2: Draw a spiral pattern
	/// </summary>
	public static void DrawSpiral()
	{
		if (_visualise is null) {
			return;
		}

		_visualise(["canvas:600:600"], true);

		int centerX = 300;
		int centerY = 300;
		int maxRadius = 250;
		int steps = 360;

		for (int i = 0; i < steps; i++) {
			double angle = i * 6.0 * Math.PI / 180.0;
			double radius = i / (double)steps * maxRadius;

			int x = centerX + (int)(radius * Math.Cos(angle));
			int y = centerY + (int)(radius * Math.Sin(angle));

			// Change color based on angle
			int hue = i * 360 / steps;
			_visualise([
				$"colour:hsl({hue}, 100%, 50%)",
				$"point:{x}:{y}"
			], false);
		}
	}

	/// <summary>
	/// Example 3: Draw a grid with path finding visualization
	/// </summary>
	public static void DrawPathFinding()
	{
		if (_visualise is null) {
			return;
		}

		int gridSize = 20;
		int cellSize = 25;
		int canvasSize = gridSize * cellSize;

		_visualise([$"canvas:{canvasSize}:{canvasSize}"], true);

		// Draw grid lines
		_visualise(["colour:#333333"], false);
		for (int i = 0; i <= gridSize; i++) {
			int pos = i * cellSize;
			_visualise([
				$"line:{pos}:0:{pos}:{canvasSize}",
				$"line:0:{pos}:{canvasSize}:{pos}"
			], false);
		}

		// Draw obstacles (walls)
		_visualise(["colour:#666666"], false);
		int[][] obstacles = [
			[5, 5], [5, 6], [5, 7], [5, 8],
			[10, 10], [11, 10], [12, 10], [13, 10],
			[15, 5], [15, 6], [15, 7]
		];

		foreach (int[] obs in obstacles) {
			int x1 = obs[0] * cellSize;
			int y1 = obs[1] * cellSize;
			int x2 = x1 + cellSize;
			int y2 = y1 + cellSize;

			_visualise([
				$"line:{x1}:{y1}:{x2}:{y1}",
				$"line:{x2}:{y1}:{x2}:{y2}",
				$"line:{x2}:{y2}:{x1}:{y2}",
				$"line:{x1}:{y2}:{x1}:{y1}"
			], false);
		}

		// Draw path (green)
		_visualise(["colour:#00ff00"], false);
		int[][] path = [
			[1, 1], [2, 1], [3, 1], [4, 1],
			[4, 2], [4, 3], [4, 4], [4, 5],
			[4, 6], [4, 7], [4, 8], [4, 9],
			[5, 9], [6, 9], [7, 9], [8, 9],
			[9, 9], [9, 10], [9, 11], [9, 12],
			[10, 12], [11, 12], [12, 12], [13, 12],
			[14, 12], [14, 11], [14, 10], [14, 9],
			[14, 8], [14, 7], [14, 6], [14, 5],
			[14, 4], [15, 4], [16, 4], [17, 4], [18, 4]
		];

		for (int i = 0; i < path.Length - 1; i++) {
			int x1 = (path[i][0] * cellSize) + (cellSize / 2);
			int y1 = (path[i][1] * cellSize) + (cellSize / 2);
			int x2 = (path[i + 1][0] * cellSize) + (cellSize / 2);
			int y2 = (path[i + 1][1] * cellSize) + (cellSize / 2);

			_visualise([$"line:{x1}:{y1}:{x2}:{y2}"], false);
		}

		// Draw start (blue) and end (red) points
		_visualise(["colour:#0000ff"], false);
		int startX = (path[0][0] * cellSize) + (cellSize / 2);
		int startY = (path[0][1] * cellSize) + (cellSize / 2);
		_visualise([$"point:{startX}:{startY}"], false);

		_visualise(["colour:#ff0000"], false);
		int endX = (path[^1][0] * cellSize) + (cellSize / 2);
		int endY = (path[^1][1] * cellSize) + (cellSize / 2);
		_visualise([$"point:{endX}:{endY}"], false);
	}

	/// <summary>
	/// Example 4: Animated wave pattern
	/// </summary>
	public static void DrawWaveAnimation()
	{
		if (_visualise is null) {
			return;
		}

		int width = 800;
		int height = 400;
		_visualise([$"canvas:{width}:{height}"], true);

		// Create multiple frames for animation
		for (int frame = 0; frame < 50; frame++) {
			_visualise([$"canvas:{width}:{height}"], true);

			double phase = frame * 0.1;
			int prevX = 0;
			int prevY = height / 2;

			for (int x = 0; x < width; x += 5) {
				double y = (height / 2) + (Math.Sin((x * 0.02) + phase) * (height / 4));
				int currentY = (int)y;

				// Color based on position
				int hue = x * 360 / width;
				_visualise([
					$"colour:hsl({hue}, 70%, 50%)",
					$"line:{prevX}:{prevY}:{x}:{currentY}"
				], false);

				prevX = x;
				prevY = currentY;
			}
		}
	}
}
