<div align="center">
<h1>CS2 Strat Roulette</h1>

[![License](https://img.shields.io/github/license/biggestmannest/CS2StratRoulette?style=for-the-badge&logo=codesandbox&logoColor=eeeeee&color=aaff44&labelColor=222222)](https://github.com/biggestmannest/CS2StratRoulette/blob/master/LICENSE)
[![Stars](https://img.shields.io/github/stars/biggestmannest/CS2StratRoulette?style=for-the-badge&logo=starship&logoColor=eeeeee&color=ffcc11&labelColor=222222)](https://github.com/biggestmannest/CS2StratRoulette/stargazers)
[![Issues](https://img.shields.io/github/issues-raw/biggestmannest/CS2StratRoulette?style=for-the-badge&logo=gitbook&logoColor=eeeeee&color=9c0000&labelColor=222222)](https://github.com/biggestmannest/CS2StratRoulette/issues)
[![CounterStrikeSharp](https://img.shields.io/badge/Counter_Strike_Sharp-v232-blue?style=for-the-badge&logo=csharp&logoColor=eeeeee&logoSize=2&label=CounterStrikeSharp&labelColor=222222&color=004cc7)](https://github.com/roflmuffin/CounterStrikeSharp/releases/tag/v232)

<h3>Counter-Strike 2 plugin for getting random enforced strategies each round</h3>

</div>

Inspired by both the [CSGO chaos mod](https://github.com/b0ink/csgo-chaos-mod) and [CSGO Strat Roulette](https://strat-roulette.github.io/), this plugin has over 50 fun strategies to spice up your regular game of Counter-Strike 2. Every round, players may experience game-changing twists such as: <b>Gladiator</b>, <b>Retakes</b>, <b>VIP</b>, <b>Tanks</b> and much more!

> [!WARNING]
> Please keep in mind that this plugin will most likely not be supported/maintained for future versions.

## Requirements
You can find an installation guide for the requirements on the [CounterStrikeSharp website](https://docs.cssharp.dev/docs/guides/getting-started.html).

- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp) @ ≥228
- [SourceMod](https://www.sourcemm.net/downloads.php?branch=dev) @ ≥2.X.X

## Known issues
During testing we are experiencing seemingly random crashes when a new round starts and a new strategy is selected and initialized. We will try to make an effort to eliminate this issue as it is quite significant.

There are also cases where persons may have their weapons removed except for the knife but won't get their knife automatically equipped. We suspect this related to networking and might be solved by the use of `Server.NextFrame`.

## Installation
- Make a folder called `CS2StratRoulette` inside `csgo/addons/counterstrikesharp/plugins/`
- Download the latest [release](https://github.com/biggestmannest/CS2StratRoulette/releases) of the plugin
- Copy the 3 files from the zip folder into the `CS2StratRoulette` folder
