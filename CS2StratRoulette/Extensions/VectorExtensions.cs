using CounterStrikeSharp.API.Modules.Utils;

namespace CS2StratRoulette.Extensions
{
	public static class VectorExtensions
	{
		public static readonly Vector Zero = new(0f, 0f, 0f);
		public static readonly Vector One = new(1f, 1f, 1f);

		public static readonly Vector North = new(1f, 0f, 0f);
		public static readonly Vector East = new(0f, 1f, 0f);
		public static readonly Vector South = new(-1f, 0f, 0f);
		public static readonly Vector West = new(0f, -1f, 0f);

		public static Vector Unit(this Vector @this) =>
			@this.Divide(@this.Abs());

		public static Vector Abs(this Vector @this) =>
			new(float.Abs(@this.X), float.Abs(@this.Y), float.Abs(@this.Z));

		public static Vector Clamp(this Vector lhs, float min, float max) =>
			new(float.Clamp(lhs.X, min, max), float.Clamp(lhs.Y, min, max), float.Clamp(lhs.Z, min, max));

		public static Vector Divide(this Vector lhs, Vector rhs) =>
			new((rhs.X == 0f) ? 0f : (lhs.X / rhs.X),
				(rhs.Y == 0f) ? 0f : (lhs.Y / rhs.Y),
				(rhs.Z == 0f) ? 0f : (lhs.Z / rhs.Z));
	}
}
