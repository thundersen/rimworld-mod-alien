using System;
using System.Linq;
using Harmony;
using RimWorld;
using Verse;

namespace Alien
{
    
    [HarmonyPatch(typeof(Skyfaller), "Impact")]
    public class ShipChunkPatch
    {
        private const int replacementPercent = 20;

        [HarmonyPrefix]
        public static void Patch(Skyfaller __instance)
        {
            foreach (var thing in __instance.innerContainer.ToList())
            {
                if (thing?.def?.defName != "ShipChunk" || new Random().Next(0, 100) > replacementPercent) continue;

                __instance.innerContainer.Remove(thing);
                __instance.innerContainer.TryAdd(ThingMaker.MakeThing(ThingDef.Named("THU_XenoShipChunk")));    
                
                Log.Message("replaced a chunk (" + replacementPercent + "% chance)");
                Log.Message("Logged exceptions after dropped chunks are a known issue of " + nameof(Alien) + " and seem to be harmless");
            }
        }
    }
}