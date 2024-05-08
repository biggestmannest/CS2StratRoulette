using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Extensions;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Knockback : Strategy
	{
		private static readonly Vector Velocity = new(400f, 400f, 300f);

		private static readonly FrozenDictionary<string, float> Weights =
			new Dictionary<string, float>(System.StringComparer.OrdinalIgnoreCase)
			{
				{ "glock", .2f },
				{ "hkp2000", .2f },
				{ "usp_silencer", .1f },
				{ "elite", .2f },
				{ "p250", .2f },
				{ "tec9", .25f },
				{ "fiveseven", .25f },
				{ "cz75a", .2f },
				{ "deagle", .5f },
				{ "revolver", .4f },

				{ "mp9", .3f },
				{ "mac10", .3f },
				{ "bizon", .3f },
				{ "mp7", .3f },
				{ "ump45", .3f },
				{ "p90", .3f },
				{ "mp5sd", .25f },

				{ "famas", .5f },
				{ "galilar", .5f },
				{ "m4a1", .6f },
				{ "m4a1_silencer", .35f },
				{ "ak47", .6f },
				{ "aug", .65f },
				{ "sg556", .65f },
				{ "ssg08", .4f },

				{ "awp", 1f },
				{ "scar20", .7f },
				{ "g3sg1", .7f },

				{ "nova", 1f },
				{ "xm1014", 1f },
				{ "mag7", 1f },
				{ "sawedoff", .9f },

				{ "m249", .5f },
				{ "negev", .5f },
			}.ToFrozenDictionary();

		public override string Name =>
			"Knockback II";

		public override string Description =>
			"Your guns now have a noticeable knock back.";

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			plugin.RegisterEventHandler<EventWeaponFire>(this.OnWeaponFire);

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			plugin.DeregisterEventHandler<EventWeaponFire>(this.OnWeaponFire);

			return true;
		}

		private HookResult OnWeaponFire(EventWeaponFire @event, GameEventInfo _)
		{
			if (!this.Running)
			{
				return HookResult.Continue;
			}

			var controller = @event.Userid;

			if (controller is null || !controller.TryGetPlayerPawn(out var pawn))
			{
				return HookResult.Continue;
			}

			var weapon = @event.Weapon.Substring(7 /*="weapon_".Length*/);

			if (!Knockback.Weights.TryGetValue(weapon, out var weight))
			{
				weight = 0f;
			}

			var vAngle = pawn.V_angle;
			var velocity = pawn.AbsVelocity;
			var forward = new Vector(0f, 0f, 0f);

			NativeAPI.AngleVectors(vAngle.Handle, forward.Handle, System.IntPtr.Zero, System.IntPtr.Zero);

			velocity += (Knockback.Velocity * weight).Multi(forward * -1f);

			pawn.Teleport(pawn.AbsOrigin, pawn.V_angle, velocity);

			return HookResult.Continue;
		}
	}
}
