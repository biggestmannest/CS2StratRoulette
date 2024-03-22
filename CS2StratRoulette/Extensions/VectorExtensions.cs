using CounterStrikeSharp.API.Modules.Utils;

namespace CS2StratRoulette.Extensions
{
	public static class VectorExtensions
	{
		public static readonly Vector Zero = new(0f, 0f, 0f);

		public static QAngle Angle2(this Vector origin, Vector direction)
		{
			const float threshold = 0.001f;
			const float rad2deg = 180f * float.Pi;

			var x = origin.X - direction.X;
			var y = origin.Y - direction.Y;

			if (y >= -threshold && y <= threshold)
			{
				y = threshold;
			}

			var angle = (float.Atan(x / y) * rad2deg);

			return new QAngle(0f, angle, 0f);
		}
	}
}
