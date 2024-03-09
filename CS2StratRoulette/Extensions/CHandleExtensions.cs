using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Diagnostics.CodeAnalysis;

namespace CS2StratRoulette.Extensions
{
	public static class CHandleExtensions
	{
		public static bool TryGetValue<T>(this CHandle<T> cHandle, [NotNullWhen(true)] out T? entity)
			where T : NativeEntity
		{
			entity = null;

			if (!cHandle.IsValid || cHandle.Value is null)
			{
				return false;
			}

			entity = cHandle.Value;

			return true;
		}
	}
}
