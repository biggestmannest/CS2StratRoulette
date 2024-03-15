using CS2StratRoulette.Extensions;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class PropHunt : Strategy
	{
		public override string Name =>
			"Prop Hunt";

		public override string Description =>
			"garry simon's modifications.";

		private readonly System.Random random = new();
		private string[] models = System.Array.Empty<string>();

		public override bool Start(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			var set = new HashSet<string>(System.StringComparer.Ordinal);
			const string designerName = "prop_physics_multiplayer";

			foreach (var entity in Utilities.FindAllEntitiesByDesignerName<CDynamicProp>(designerName))
			{
				if (!entity.IsValid)
				{
					continue;
				}

				var prop = new CPhysicsPropMultiplayer(entity.Handle);

				if (!prop.IsValid)
				{
					continue;
				}

				set.Add(Constants.Signatures.GetModel.Invoke(prop.Handle));
			}

			this.models = set.ToArray();

			foreach (var controller in Utilities.GetPlayers())
			{
				if (!controller.TryGetPlayerPawn(out var pawn) || controller.Team is CsTeam.Terrorist)
				{
					continue;
				}

				pawn.SetModel(this.models[this.random.Next(this.models.Length)]);
			}

			return true;
		}

		public override bool Stop(ref CS2StratRoulettePlugin plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			return true;
		}
	}
}
