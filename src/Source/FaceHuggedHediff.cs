using RimWorld;
using Verse;

namespace Alien
{
    class FaceHuggedHediff : HediffWithComps
    {
        public override void Notify_PawnDied()
        {
            base.Notify_PawnDied();
            
            Log.Message("HE DEEEEEED...");

            if (pawn.def.race.Animal == false)
            {
                var corpse = pawn.Corpse;
                var faction = FactionUtility.DefaultFactionFrom(FactionDef.Named("THU_Xenomorph"));
                var chestburster = PawnGenerator.GeneratePawn(PawnKindDef.Named("THU_XenomorphDrone"), faction);
                chestburster.Position = corpse.Position;
                chestburster.SpawnSetup(corpse.Map, false);
            }
            else
            {
                Messages.Message("an animal has succumbed to nanite infection, and have been deemed inappropriate for assimilation. The nanites have consumed and destroyed the corpse.", MessageTypeDefOf.NeutralEvent);
                pawn.Corpse.Destroy();
            }
        }
    }
}