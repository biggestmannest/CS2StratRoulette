#!/bin/bash

css_releases_uri=https://api.github.com/repos/roflmuffin/CounterStrikeSharp/releases
addons_dir=~/cs2-data/game/csgo/addons
metamod_dir=~/cs2-data/game/csgo/addons/metamod
css_dir=~/cs2-data/game/csgo/addons/counterstrikesharp

css_download=$(curl $css_releases_uri | jq -r '.[0].assets[] | select(.name | test("^counterstrikesharp-with-runtime-build-\\d+-linux")) | .browser_download_url')

# save previous config
mv $css_dir/configs/admins.json ~/

# cleanup previous installation
rm -rf $css_dir
rm -f $metamod_dir/counterstrikesharp.vdf

# download new version
wget -O $addons_dir/css.zip "$css_download"

# unzips to /tmp with all the stuffs
unzip -d $addons_dir/tmp $addons_dir/css.zip

mv $addons_dir/tmp/addons/counterstrikesharp $css_dir
mv $addons_dir/tmp/addons/metamod/counterstrikesharp.vdf $metamod_dir/

# copy over previous config
mv ~/admins.json $css_dir/configs/

# cleanup
rm -rf $addons_dir/tmp
rm -f $addons_dir/css.zip
