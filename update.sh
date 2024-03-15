#!/bin/bash

git fetch && git reset --hard origin/master

dotnet publish CS2StratRoulette --configuration Release
