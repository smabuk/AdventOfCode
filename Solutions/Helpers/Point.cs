namespace AdventOfCode.Solutions.Helpers;

public record struct Point(int X, int Y) {
	public Point((int X, int Y) point) : this(point.X, point.Y) { }

	public static implicit operator (int x, int y)(Point p) {
		p.Deconstruct(out int x, out int y);
		return (x, y);
	}
};
