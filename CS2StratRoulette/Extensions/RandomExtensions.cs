namespace CS2StratRoulette.Extensions
{
	public static class RandomExtensions
	{
		public static void Shuffle<T>(this System.Random @this, T[] array)
		{
			var i = array.Length;

			while (i > 1)
			{
				var j = @this.Next(i--);
				(array[i], array[j]) = (array[j], array[i]);
			}
		}
	}
}
