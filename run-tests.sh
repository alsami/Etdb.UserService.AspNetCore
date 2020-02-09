#!/usr/bin/env bash
dotnet test --configuration Release --nologo -p:CollectCoverage=true -p:Threshold=0 -p:ThresholdType=line -p:ThresholdStat=total
