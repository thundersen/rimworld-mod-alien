#!/bin/sh

GAME_DLL_DIR=../game/RimWorldLinux_Data/Managed

cp ${GAME_DLL_DIR}/Assembly-CSharp.dll lib
cp ${GAME_DLL_DIR}/UnityEngine.dll lib

LOCAL_STEAM_MOD_DIR=../local-steam-workshop

cp ${LOCAL_STEAM_MOD_DIR}/818773962/Assemblies/0Harmony.dll lib

