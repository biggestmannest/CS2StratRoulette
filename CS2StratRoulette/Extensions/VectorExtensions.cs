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

		public static QAngle Angle2(this Vector origin, Vector target)
		{
			const float rad2deg = 180f / float.Pi;

			var diff = target.Y - origin.Y;
			var north = origin + new Vector(0f, diff, 0f);

			var angle =
				float.Atan2(
					north.Cross2D(target),
					north.Dot2D(target)
				) * rad2deg;

			return new QAngle(0f, angle, 0f);
		}

		public static float Dot2D(this Vector lhs, Vector rhs) =>
			(lhs.X * rhs.X) + (lhs.Y * rhs.Y);

		public static float Dot(this Vector lhs, Vector rhs) =>
			(lhs.X * rhs.X) + (lhs.Y * rhs.Y) + (lhs.Z * rhs.Z);

		public static float Cross2D(this Vector lhs, Vector rhs) =>
			(lhs.X * rhs.Y) - (lhs.Y * rhs.X);

		public static Vector Unit2D(this Vector @this)
		{
			var u = @this.Divide(@this.Abs());
			u.Z = 0f; // 🤙

			return u;
		}

		public static Vector Abs(this Vector @this) =>
			new(float.Abs(@this.X), float.Abs(@this.Y), float.Abs(@this.Z));

		public static Vector Clamp(this Vector lhs, float min, float max) =>
			new(float.Clamp(lhs.X, min, max), float.Clamp(lhs.Y, min, max), float.Clamp(lhs.Z, min, max));

		public static Vector Divide(this Vector lhs, Vector rhs) =>
			new(lhs.X / rhs.X, lhs.Y / rhs.Y, lhs.Z / rhs.Z);
	}
}
