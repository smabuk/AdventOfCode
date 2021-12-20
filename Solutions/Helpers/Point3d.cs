namespace AdventOfCode.Solutions.Helpers;

public record struct Point3d(int X, int Y, int Z) {

	public Point3d((int X, int Y, int Z) point) : this(point.X, point.Y, point.Z) { }





	public static implicit operator Point3d((int x, int y, int z) point) =>
		new(point.x, point.y, point.z);

	public static implicit operator (int x, int y, int z)(Point3d point) =>
		(point.X, point.Y, point.Z);

	public static Point3d operator +(in Point3d lhs, in Point3d rhs) =>
		new Point3d(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
	public static Point3d operator -(in Point3d lhs, in Point3d rhs) =>
		new Point3d(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
	public static Point3d operator *(in Point3d lhs, in Point3d rhs) =>
		new Point3d(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z);
	public static Point3d operator *(in Point3d lhs, int rhs) =>
		new Point3d(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs);
	public static Point3d operator *(int lhs, in Point3d rhs) =>
		new Point3d(rhs.X * lhs, rhs.Y * lhs, rhs.Z * lhs);
	public static Point3d operator -(in Point3d value) =>
		new Point3d(-value.X, -value.Y, -value.Z);

	public static implicit operator Point3d(int v) {
		throw new NotImplementedException();
	}

	public void Deconstruct(out int x, out int y, out int z) {
		x = X;
		y = Y;
		z = Z;
	}
}
