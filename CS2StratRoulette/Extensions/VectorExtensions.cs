using CounterStrikeSharp.API.Modules.Utils;

namespace CS2StratRoulette.Extensions
{
	public static class VectorExtensions
	{
		public static readonly Vector Zero = new(0f, 0f, 0f);
		public static readonly Vector One = new(1f, 1f, 1f);

		public static readonly Vector Up = new(0f, 0f, 1f);
		public static readonly Vector Down = new(0f, 0f, -1f);

		// https://developer.valvesoftware.com/wiki/Coordinates
		public static readonly Vector North = new(0f, 1f, 0f);
		public static readonly Vector East = new(1f, 0f, 0f);
		public static readonly Vector South = new(0f, -1f, 0f);
		public static readonly Vector West = new(-1f, 0f, 0f);

		public static Vector Unit(this Vector @this) =>
			@this.Divide(@this.Abs());

		public static float Dot(this Vector @this, Vector rhs) =>
			((@this.X * rhs.X) + (@this.Y * rhs.Y) + (@this.Z * rhs.Z));

		public static Vector Abs(this Vector @this) =>
			new(float.Abs(@this.X), float.Abs(@this.Y), float.Abs(@this.Z));

		public static Vector Clamp(this Vector lhs, float min, float max) =>
			new(float.Clamp(lhs.X, min, max), float.Clamp(lhs.Y, min, max), float.Clamp(lhs.Z, min, max));

		public static Vector Multi(this Vector lhs, Vector rhs) =>
			new(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z);

		public static Vector Multi(this Vector lhs, Angle rhs) =>
			new(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z);

		public static Vector Divide(this Vector lhs, Vector rhs) =>
			new((rhs.X == 0f) ? 0f : (lhs.X / rhs.X),
				(rhs.Y == 0f) ? 0f : (lhs.Y / rhs.Y),
				(rhs.Z == 0f) ? 0f : (lhs.Z / rhs.Z));
	}
}
