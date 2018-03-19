using RimWorld;
using Verse;

namespace Alien
{
    class FaceHuggedHediff : HediffWithComps
    {
        public override void Notify_PawnDied()
        {
            base.Notify_PawnDied();
            
            SpawnChestburster();
        }

        private void SpawnChestburster()
        {
            var chestburster = CreateChestburster();
            chestburster.Position = pawn.Corpse.Position;
            chestburster.SpawnSetup(pawn.Corpse.Map, false);
        }

        // for new every host creates a drone. later create different xenos for animals
        private Pawn CreateChestburster()
        {
            var faction = FactionUtility.DefaultFactionFrom(FactionDef.Named("THU_Xenomorph"));
            var pawnKindDef = PawnKindDef.Named("THU_Chestburster");
            var request = new PawnGenerationRequest(pawnKindDef, newborn: true, faction: faction);

            var chestburster = PawnGenerator.GeneratePawn(request);
            
            Find.LetterStack.ReceiveLetter("Chestburster!", "THU_Chestburster_SuccessMessage".Translate(pawn?.Label, pawn?.gender.GetPossessive()), LetterDefOf.ThreatBig, chestburster);
            
            return chestburster;
        }
    }    
}