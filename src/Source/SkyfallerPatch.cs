using System;
using System.Linq;
using Harmony;
using RimWorld;
using Verse;

namespace Alien
{
    
    [HarmonyPatch(typeof(Skyfaller), "Impact")]
    public class SkyfallerPatch
    {
        private const int replacementPercent = 20;

        [HarmonyPrefix]
        public static void Patch(Skyfaller __instance)
        {
            foreach (var thing in __instance.innerContainer.ToList())
            {
                TryToInfectSpacer(thing);

                TryToReplaceChunk(thing, __instance.innerContainer);
            }
        }

        private static void TryToInfectSpacer(Thing thing)
        {
            var spaceRefugee = (thing as ActiveDropPod)?.Contents?.innerContainer?.FirstOrDefault(passenger => passenger.Faction == Faction.OfSpacer) as Pawn;

            if (spaceRefugee == null || !IsRandomSuccess()) return;

            var hediff = HediffMaker.MakeHediff(HediffDef.Named("THU_FaceHugged"), spaceRefugee);

            spaceRefugee.health?.AddHediff(hediff);
            
            Log.Message("infected a space refugee (" + replacementPercent + "% chance)");
        }

        private static void TryToReplaceChunk(Thing thing, ThingOwner container)
        {
            if (thing?.def?.defName != "ShipChunk" || !IsRandomSuccess()) return;

            container.Remove(thing);
            container.TryAdd(ThingMaker.MakeThing(ThingDef.Named("THU_XenoShipChunk")));

            Log.Message("replaced a chunk (" + replacementPercent + "% chance)");
        }

        private static bool IsRandomSuccess()
        {
            return new Random().Next(0, 100) <= replacementPercent;
        }
    }
}