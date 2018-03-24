using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Alien
{
    public class XenoLifecycle : Thing
    {
        private static XenoLifecycle singleton;
        
        private readonly List<SpawnXenoCommand> scheduledSpawns = new List<SpawnXenoCommand>();

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
            if (scheduledSpawns.Count == 0) return;

            scheduledSpawns.Where(s => s.atTick <= Find.TickManager.TicksGame).ToList()
                .ForEach(SpawnXenomorph); 
        }

        public void SpawnChestburster(Pawn infectedPawn)
        {
            var chestburster = SpawnXenoPawn("THU_Chestburster", infectedPawn.Corpse.Position, infectedPawn.Corpse.Map);
            
            ScheduleXenoSpawn(chestburster);
            
            Find.LetterStack.ReceiveLetter("Chestburster!",
                "THU_Chestburster_SuccessMessage".Translate(infectedPawn?.Label, infectedPawn?.gender.GetPossessive()),
                LetterDefOf.ThreatBig, chestburster);
        }

        public static void HideChestburster(Pawn chestburster)
        {
            chestburster.Destroy();

            Messages.Message("Chestburster is hiding now and can't be found before transforming".Translate(),
                MessageTypeDefOf.NegativeEvent);
        }
        
        private class SpawnXenoCommand
        {
            public IntVec3 position;
            public int atTick;
            public Map map;
            public Pawn chestburster;
        }
        
        private void ScheduleXenoSpawn(Pawn chestburster)
        {
            var spawn = new SpawnXenoCommand
            {
                atTick = GameTime.RandomTickNextNight(Find.TickManager.TicksGame, GenLocalDate.HourOfDay(this)),
                position = chestburster.Position,
                map = chestburster.Map,
                chestburster = chestburster
            };

            scheduledSpawns.Add(spawn);

            Log.Message("Will spawn xeno at " + spawn.atTick + " (now its " + Find.TickManager.TicksGame + ")");
        }

        private void SpawnXenomorph(SpawnXenoCommand spawn)
        {
            Log.Message("Spawning xeno at " + Find.TickManager.TicksGame + " (was scheduled for " + spawn.atTick + ")");
            
            var xenomorph = SpawnXenoPawn("THU_XenomorphDrone", spawn.position, spawn.map);

            scheduledSpawns.Remove(spawn);
            
            if (!spawn.chestburster.Destroyed)
                spawn.chestburster.Destroy();
            
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