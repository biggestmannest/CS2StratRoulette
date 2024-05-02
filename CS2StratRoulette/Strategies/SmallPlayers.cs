using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class SmallPlayers : Strategy
	{
		public override string Name =>
			"Ants";

		public override string Description =>
			"smallest man 2024";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			foreach (var player in Utilities.GetPlayers())
			{
				if (!player.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				if (pawn.CBodyComponent is null || pawn.CBodyComponent.SceneNode is null)
				{
					return false;
				}

				pawn.CBodyComponent.SceneNode.GetSkeletonInstance().Scale = 0.6f;

				pawn.VelocityModifier = 1.5f;
				pawn.GravityScale = 1.5f;

				Utilities.SetStateChanged(pawn, "CGameSceneNode", "m_flScale");
				Utilities.SetStateChanged(pawn, "CBaseEntity", "m_CBodyComponent");
			}

			plugin.RegisterEventHandler<EventPlayerHurt>(this.OnPlayerHurt);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			foreach (var player in Utilities.GetPlayers())
			{
				if (!player.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				if (pawn.CBodyComponent is null || pawn.CBodyComponent.SceneNode is null)
				{
					return false;
				}

				pawn.CBodyComponent.SceneNode.GetSkeletonInstance().Scale = 1f;

				pawn.VelocityModifier = 1f;
				pawn.GravityScale = 1f;

				Utilities.SetStateChanged(pawn, "CGameSceneNode", "m_flScale");
				Utilities.SetStateChanged(pawn, "CBaseEntity", "m_CBodyComponent");
			}

			plugin.DeregisterEventHandler<EventPlayerHurt>(this.OnPlayerHurt);

			return true;
		}

		private HookResult OnPlayerHurt(EventPlayerHurt @event, GameEventInfo _)
		{
			var player = @event.Userid;

			if (player is null || !player.TryGetPlayerPawn(out var pawn))
			{
				return HookResult.Continue;
			}

			pawn.VelocityModifier = 1.5f;
			pawn.GravityScale = 1.5f;

			return HookResult.Continue;
		}
	}
}
