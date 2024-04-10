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
		private static readonly Vector Velocity = new(200f, 200f, 200f);

		private static readonly FrozenDictionary<string, float> Weights =
			new Dictionary<string, float>(System.StringComparer.OrdinalIgnoreCase)
			{
				{ "weapon_deagle", .5f },
				{ "weapon_revolver", .4f },
				{ "weapon_glock", .2f },
				{ "weapon_usp_silencer", .1f },
				{ "weapon_cz75a", .2f },
				{ "weapon_fiveseven", .25f },
				{ "weapon_p250", .2f },
				{ "weapon_tec9", .25f },
				{ "weapon_elite", .2f },
				{ "weapon_hkp2000", .2f },

				{ "weapon_mp9", .4f },
				{ "weapon_mac10", .4f },
				{ "weapon_bizon", .35f },
				{ "weapon_mp7", .4f },
				{ "weapon_ump45", .4f },
				{ "weapon_p90", .4f },
				{ "weapon_mp5sd", .3f },

				{ "weapon_famas", .5f },
				{ "weapon_galilar", .5f },
				{ "weapon_m4a4", .6f },
				{ "weapon_m4a1_silencer", .45f },
				{ "weapon_ak47", .6f },
				{ "weapon_aug", .65f },
				{ "weapon_sg553", .65f },
				{ "weapon_ssg08", .55f },
				{ "weapon_awp", .8f },
				{ "weapon_scar20", .7f },
				{ "weapon_g3sg1", .7f },

				{ "weapon_nova", .65f },
				{ "weapon_xm1014", .65f },
				{ "weapon_mag7", .65f },
				{ "weapon_sawedoff", .65f },
				{ "weapon_m249", .65f },

				{ "weapon_negev", .5f },
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

			const string weaponFire = "weapon_fire";
			plugin.DeregisterEventHandler(weaponFire, this.OnWeaponFire, true);

			return true;
		}

		private HookResult OnWeaponFire(EventWeaponFire @event, GameEventInfo _)
		{
			if (!this.Running)
			{
				return HookResult.Continue;
			}

			var controller = @event.Userid;

			if (!controller.TryGetPlayerPawn(out var pawn))
			{
				return HookResult.Continue;
			}

			var weapon = @event.Weapon;

			System.Console.WriteLine($"[Knockback::OnWeaponFire] event_weapon {weapon}");

			if (weapon is null || !Knockback.Weights.TryGetValue(weapon, out var weight))
			{
				weight = 0f;
			}

			var vAngle = pawn.V_angle;
			var velocity = pawn.AbsVelocity;

			var forward = new Vector(0f, 0f, 0f);

			NativeAPI.AngleVectors(vAngle.Handle, forward.Handle, System.IntPtr.Zero, System.IntPtr.Zero);

			velocity += (Knockback.Velocity * weight).Multi(forward * -1f);

			pawn.Teleport(
				pawn.AbsOrigin ?? VectorExtensions.Zero,
				vAngle,
				velocity
			);

			return HookResult.Continue;
		}
	}
}
