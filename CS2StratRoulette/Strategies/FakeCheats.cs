using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Timers;
using System.Diagnostics.CodeAnalysis;
using System;

namespace CS2StratRoulette.Strategies
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class FakeCheats : IStrategy
    {
        /// <inheritdoc cref="IStrategy.Name"/>
        public string Name => "Fake Cheats";

        /// <inheritdoc cref="IStrategy.Description"/>
        public string Description => "Spams fake cheat advertisements in the chat.";

        /// <inheritdoc cref="IStrategy.Running"/>
        public bool Running { get; private set; }

        private static string[] fakeMsgs = {
                "NEVER VAC AND YOU KNOW! ONLY @ GOGlobal.cheat",
                "AIM, WALLS & MORE @ GOGlobal.cheat",
                "50% OFF ONLY ON GOGlobal.cheat",
                "FREE 14 DAY SPINBOT TRIAL @ GOGlobal.cheat",
                "VISIT GOGlobal.cheat TODAY TO DOWNLOAD BEST HACKS",
                "VAC UNDEDECTED SOFT AIM @ GOGlobal.cheat",
                "UNDETECTED AIMBOT & WALLHACKS @ GOGlobal.cheat",
                "EXCLUSIVE ESP & TRIGGERBOT @ GOGlobal.cheat",
                "LIMITED TIME OFFER: 60% DISCOUNT @ GOGlobal.cheat",
                "TRY OUR AIM ASSISTANCE TODAY @ GOGlobal.cheat",
                "INSTANT HEADSHOTS GUARANTEED @ GOGlobal.cheat",
                "UNDISTURBED BY VAC BANS @ GOGlobal.cheat",
                "EXPLORE OUR ESP FEATURES @ GOGlobal.cheat",
                "UNTRACEABLE AIMBOT TECHNOLOGY @ GOGlobal.cheat",
                "ENHANCE YOUR GAMING EXPERIENCE @ GOGlobal.cheat",
                "UNDISCOVERED CHEAT METHODS @ GOGlobal.cheat"
            };

        private Timer? timer;

        /// <inheritdoc cref="IStrategy.Start"/>
        public bool Start(ref CS2StratRoulettePlugin plugin)
        {
            if (this.Running)
            {
                return false;
            }

            var random = new Random();
            var players = Utilities.GetPlayers();
            this.timer = new Timer(3.0f, () =>
            {
                int index = random.Next(fakeMsgs.Length);
                var player = players[random.Next(0, players.Count)];
                string randomMsg = fakeMsgs[index];
                Console.WriteLine("asdasdasddddddasdasd");
                if (player.IsValid)
                {
                    player.ExecuteClientCommandFromServer($"say {randomMsg}");
                }
            }, TimerFlags.REPEAT);

            this.Running = true;

            return true;
        }

        /// <inheritdoc cref="IStrategy.Stop"/>
        public bool Stop(ref CS2StratRoulettePlugin plugin)
        {
            if (!this.Running)
            {
                return false;
            }

            this.Running = false;

            if (this.timer is not null)
            {
                this.timer.Kill();
            }

            return true;
        }
    }
}
