using CounterStrikeSharp.API.Core;
using System.Diagnostics.CodeAnalysis;
using CounterStrikeSharp.API;

namespace CS2StratRoulette.Strategies
{
	[SuppressMessage("ReSharper", "UnusedType.Global")]
	public sealed class NoJumping : Strategy
	{
		public override string Name =>
			"oops, lost my spacebar!";

		public override string Description =>
			"You cannot jump.";

		private const string Enable = "sv_jump_impulse 1";
		private const string Disable = "sv_jump_impulse 301.993";

		public override bool Start(ref Base plugin)
		{
			if (!base.Start(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(NoJumping.Enable);

			return true;
		}

		public override bool Stop(ref Base plugin)
		{
			if (!base.Stop(ref plugin))
			{
				return false;
			}

			Server.ExecuteCommand(NoJumping.Disable);

			return true;
		}
	}
}
