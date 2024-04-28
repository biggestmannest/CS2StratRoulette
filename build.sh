#!/bin/bash

mode=Release
git_tag=
git_branch=

while getopts "d:t:b" opt; do
  case "$opt" in
    "d") mode=Debug ;;
    "t") git_tag="$OPTARG" ;;
    "b") git_branch="$OPTARG" ;;
    "*") exit 1 ;;
  esac
done

git fetch --all --tags --prune

if [ -n "$git_tag" ] && [ -n "$git_branch" ]; then
  git checkout tags/"$git_tag" -b "$git_branch"
fi
if [ -n "$git_branch" ]; then
  git checkout -b "$git_branch"
else
  git reset --hard origin/master
fi

dotnet build CS2StratRoulette -c $mode
