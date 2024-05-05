using System.Runtime.CompilerServices;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace CS2StratRoulette.Helpers
{
	public static class Player
	{
		private static readonly CCSPlayerController[] Slots;

		public static int Count => Player.Slots.Length;

		static Player()
		{
			Player.Slots = new CCSPlayerController[Server.MaxPlayers];
		}

		public static CCSPlayerController Get(int slot)
		{
			if (slot < 0 || slot > Player.Slots.Length)
			{
				throw new System.Exception("Slot is out of bounds");
			}

			return Player.Slots[slot];
		}

		public static void Replace(CCSPlayerController controller)
		{
			if (controller.Slot < 0 || controller.Slot > Player.Slots.Length)
			{
				throw new System.Exception("Slot is out of bounds");
			}

			Player.Slots[controller.Slot] = controller;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Remove(CCSPlayerController controller)
		{
			Player.Remove(controller.Slot);
		}

		public static void Remove(int slot)
		{
			if (slot < 0 || slot > Player.Slots.Length)
			{
				throw new System.Exception("Slot is out of bounds");
			}

			Player.Slots[slot] = new CCSPlayerController(System.IntPtr.Zero);
		}

		public static void ForEach(System.Action<CCSPlayerController> cb)
		{
			foreach (var controller in Player.Slots)
			{
				if (controller.IsValid)
				{
					cb(controller);
				}
			}
		}
	}
}
