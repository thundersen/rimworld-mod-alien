using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Alien
{
    public class XenoLifecycle : Thing
    {
        private static XenoLifecycle singleton;
        
        private readonly List<ChestbursterPawn> chestbursters = new List<ChestbursterPawn>();

        public static XenoLifecycle Instance()
        {
            return singleton ?? (singleton = new XenoLifecycle());
        }

        private XenoLifecycle()
        {
            def = new ThingDef {tickerType = TickerType.Normal, isSaveable = false};
            Find.TickManager.RegisterAllTickabilityFor(this);
        }

        public override void Tick()
        {
            if (chestbursters.Count == 0) return;

            chestbursters.Where(cb => cb.SpawnXenoAtTick <= Find.TickManager.TicksGame).ToList()
                .ForEach(SpawnXenomorph);
        }

        public void SpawnChestburster(Pawn infectedPawn)
        {
            var chestburster = SpawnXenoPawn("THU_Chestburster", infectedPawn.Corpse.Position, infectedPawn.Corpse.Map) as ChestbursterPawn;
            
            if (chestburster == null)
                throw new AlienException("Spawned chestburster is not of class " + nameof(ChestbursterPawn));
            
            Find.LetterStack.ReceiveLetter("Chestburster!",
                "THU_Chestburster_SuccessMessage".Translate(infectedPawn.Label, infectedPawn.gender.GetPossessive()),
                LetterDefOf.ThreatBig, chestburster);
        }

        // called from chestburster to make sure they are also registered when debug-spawned
        public void Register(ChestbursterPawn chestburster)
        {
            if (!chestbursters.Contains(chestburster))
                chestbursters.Add(chestburster);
        }

        private void SpawnXenomorph(ChestbursterPawn chestburster)
        {
            var spawnPosition = chestburster.FindXenoSpawnPosition();
            
            var xenomorph = SpawnXenoPawn("THU_XenomorphDrone", spawnPosition, chestburster.SpawnXenoOnMap);
            
            if (!chestburster.Destroyed)
                chestburster.Destroy();

            chestbursters.Remove(chestburster);
            
            Find.LetterStack.ReceiveLetter("XENOMORPH!", "", LetterDefOf.ThreatBig, xenomorph);
        }

        private static Pawn SpawnXenoPawn(string pawnKindDefName, IntVec3 position, Map map)
        {
            var pawn = CreateXenoPawn(pawnKindDefName);
            pawn.Position = position;
            pawn.SpawnSetup(map, false);
            return pawn;
        }

        private static Pawn CreateXenoPawn(string pawnKindDefName)
        {
            var faction = FactionUtility.DefaultFactionFrom(FactionDef.Named("THU_Xenomorph"));
            
            var pawnKindDef = PawnKindDef.Named(pawnKindDefName);

            var request = new PawnGenerationRequest(pawnKindDef, newborn: true, faction: faction);
            
            return PawnGenerator.GeneratePawn(request);
        }
    }
}