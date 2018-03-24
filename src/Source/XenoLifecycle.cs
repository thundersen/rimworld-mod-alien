using System.ComponentModel;
using RimWorld;
using Verse;

namespace Alien
{
    public class XenoLifecycle : Thing
    {
        private readonly TickManager tickManager;

        private static XenoLifecycle singleton;
        
        public static XenoLifecycle Instance()
        {
            return singleton ?? (singleton = new XenoLifecycle());
        }
        
        private XenoLifecycle()
        {
            Log.Message("Instantiating XenoLifecycle");
            //tickManager = Find.TickManager;
            //tickManager.RegisterAllTickabilityFor(this);
        }

        public override void TickRare()
        {
            Log.Message("Ticking");
        }

        public void SpawnChestburster(Pawn infectedPawn)
        {
            var chestburster = CreateChestburster(infectedPawn);
            chestburster.Position = infectedPawn.Corpse.Position;
            chestburster.SpawnSetup(infectedPawn.Corpse.Map, false);
        }

        private static Pawn CreateChestburster(Pawn infectedPawn)
        {
            var faction = FactionUtility.DefaultFactionFrom(FactionDef.Named("THU_Xenomorph"));
            var pawnKindDef = PawnKindDef.Named("THU_Chestburster");
            var request = new PawnGenerationRequest(pawnKindDef, newborn: true, faction: faction);

            var chestburster = PawnGenerator.GeneratePawn(request);
            
            Find.LetterStack.ReceiveLetter("Chestburster!", "THU_Chestburster_SuccessMessage".Translate(infectedPawn?.Label, infectedPawn?.gender.GetPossessive()), LetterDefOf.ThreatBig, chestburster);
            
            return chestburster;
        }
    }
}