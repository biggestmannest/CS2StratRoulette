#!/bin/bash

mode=Release

while getopts "d:t:b:" opt; do
  case "$opt" in
    "d") mode=Debug ;;
    "t") git_tag="$OPTARG" ;;
    "b") git_branch="$OPTARG" ;;
    "help") git_branch="$OPTARG" ;;
  esac
done

build_dir=CS2StratRoulette/bin/"${mode}"/net8.0
css_dir=~/cs2-data/game/csgo/addons/counterstrikesharp

mv "${build_dir}*" "${css_dir}/plugins/CS2StratRoulette/"
