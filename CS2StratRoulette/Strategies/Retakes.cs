using System.Collections.Generic;
using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using CS2StratRoulette.Constants;
using CS2StratRoulette.Enums;
using CS2StratRoulette.Extensions;
using CS2StratRoulette.Helpers;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class Retakes : Strategy
	{
		public override string Name =>
			"Retakes";

		public override string Description =>
			"Bomb will be planted at a random bombsite. T's have to defend, CT's have to retake the site and defuse.";

		public override StrategyFlags Flags =>
			StrategyFlags.AlwaysVisible;

		private const string ExtendC4Timer = "mp_c4timer 60";
		private const string NormalC4Timer = "mp_c4timer 40";

		private readonly List<CCSPlayerController> cts = new(Server.MaxPlayers / 2);
		private readonly List<CCSPlayerController> ts = new(Server.MaxPlayers / 2);

		private static readonly System.Random Random = new();

		private BombSite bombsite;

		private string mapName = string.Empty;

		public override bool CanRun()
		{
			return RetakeSpots.Maps.ContainsKey(Server.MapName);
		}

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			var rulesProxy = Game.RulesProxy();
			var rules = rulesProxy?.GameRules;

			if (rulesProxy is not null && rules is not null)
			{
				rules.FreezeTime = 0;
				rules.BuyTimeEnded = true;
				rules.FreezePeriod = false;

				Utilities.SetStateChanged(rulesProxy, "CCSGameRulesProxy", "m_pGameRules");
			}

			Server.ExecuteCommand(Retakes.ExtendC4Timer);

			this.mapName = Server.MapName;

			Player.ForEach((controller) =>
			{
				// ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
				switch (controller.Team)
				{
					case CsTeam.Terrorist:
						this.ts.Add(controller);
						break;
					case CsTeam.CounterTerrorist:
						this.cts.Add(controller);
						break;
				}
			});

			var randomT = this.ts[Retakes.Random.Next(this.ts.Count)];

			this.PlantTheBomb(randomT);

			this.TeleportPlayers();

			var siteAnnouncement = this.bombsite switch
			{
				BombSite.A => "Bomb has been planted on bombsite A",
				BombSite.B => "Bomb has been planted on bombsite B",
				_          => string.Empty,
			};

			Player.ForEach((controller) =>
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					return;
				}

				controller.PrintToCenter(siteAnnouncement);

				// if (controller.Team is CsTeam.CounterTerrorist)
				// {
				// 	controller.GiveNamedItem("item_defuser");
				// }

				if (controller.Team is CsTeam.Terrorist)
				{
					pawn.RemoveC4();
				}
			});

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(Retakes.NormalC4Timer);

			return true;
		}

		private static void SendBombPlantedEvent(CCSPlayerController bombCarrier, BombSite bombSite)
		{
			if (!bombCarrier.IsValid)
			{
				return;
			}

			var pEvent = NativeAPI.CreateEvent("bomb_planted", true);

			NativeAPI.SetEventPlayerController(pEvent, "userid", bombCarrier.Handle);
			NativeAPI.SetEventInt(pEvent, "userid", (int)bombCarrier.Index);
			NativeAPI.SetEventInt(pEvent, "site", (int)bombSite);
			NativeAPI.FireEvent(pEvent, false);
		}

		private void PlantTheBomb(CCSPlayerController player)
		{
			if (!RetakeSpots.Maps.TryGetValue(this.mapName, out var map))
			{
				return;
			}

			this.bombsite = (Retakes.Random.FiftyFifty() ? BombSite.A : BombSite.B);
			var randomSite = this.bombsite is BombSite.A ? map.BombA : map.BombB;

			var position = randomSite[Retakes.Random.Next(randomSite.Length)];

			var bombEntity = Utilities.CreateEntityByName<CPlantedC4>("planted_c4");

			player.Teleport(
				position,
				QAngle.Zero,
				Vector.Zero
			);

			if (bombEntity?.AbsOrigin is null || player.AbsOrigin is null)
			{
				return;
			}

			bombEntity.AbsOrigin.X = player.AbsOrigin.X;
			bombEntity.AbsOrigin.Y = player.AbsOrigin.Y;
			bombEntity.AbsOrigin.Z = player.AbsOrigin.Z;
			bombEntity.HasExploded = false;

			bombEntity.BombSite = (int)this.bombsite;

			bombEntity.BombTicking = true;
			bombEntity.CannotBeDefused = false;

			bombEntity.DispatchSpawn();

			var gameRules = Game.Rules();

			if (gameRules is null)
			{
				return;
			}

			gameRules.BombPlanted = true;
			gameRules.BombDefused = false;
			Retakes.SendBombPlantedEvent(player, this.bombsite);
			System.Console.WriteLine($"Bomb planted at Bombsite: {this.bombsite}");
		}

		private void TeleportPlayers()
		{
			if (!RetakeSpots.Maps.TryGetValue(this.mapName, out var map))
			{
				return;
			}

			var ctTeleportSpots = this.GetCtPositions(map);
			var tTeleportSpots = this.GetTPositions(map);

			Retakes.TeleportTeam(this.cts, ctTeleportSpots, CsTeam.CounterTerrorist);
			Retakes.TeleportTeam(this.ts, tTeleportSpots, CsTeam.Terrorist);
		}

		private static void TeleportTeam(List<CCSPlayerController> controllers, Vector[] positions, CsTeam team)
		{
			System.Random.Shared.Shuffle(positions);

			var n = 0;
			var retakeWeapons = team is CsTeam.CounterTerrorist ? RetakeBuys.CT : RetakeBuys.T;
			var weapons = Retakes.Random.Next(3) switch
			{
				0 => retakeWeapons.Eco,
				1 => retakeWeapons.Force,
				_ => retakeWeapons.Full,
			};

			foreach (var controller in controllers)
			{
				if (!controller.TryGetPlayerPawn(out var pawn))
				{
					continue;
				}

				Server.NextFrame(() =>
				{
					pawn.KeepWeaponsByType(CSWeaponType.WEAPONTYPE_KNIFE);

					if (controller.Team is CsTeam.Terrorist)
					{
						pawn.RemoveC4();
					}
				});

				Server.NextFrame(() => controller.GiveNamedItem(weapons[Retakes.Random.Next(weapons.Length)]));

				pawn.Teleport(positions[n++], QAngle.Zero, Vector.Zero);

				if (n >= positions.Length)
				{
					n = 0;
				}
			}
		}

		private Vector[] GetCtPositions(RetakeSpots.AllRetakeSpots map)
		{
			return this.bombsite is BombSite.B ? map.CtsB : map.CtsA;
		}

		private Vector[] GetTPositions(RetakeSpots.AllRetakeSpots map)
		{
			return this.bombsite is BombSite.B ? map.TsB : map.TsA;
		}
	}
}
