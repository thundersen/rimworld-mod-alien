using RimWorld;
using Verse;
using Verse.AI;

namespace Alien
{
    public class ChestbursterJobGiver : ThinkNode_JobGiver
    {
        [DefOf]
        public static class JobDefOf
        {
            public static JobDef THU_GoIntoHiding;
        }
        
        protected override Job TryGiveJob(Pawn pawn)
        {
            var chestburster = pawn as ChestbursterPawn;
            
            if (chestburster == null || chestburster.CurJob?.def == JobDefOf.THU_GoIntoHiding)
                return null;
            
            Log.Message("Chestburster trying to hide...");

            var hidingPlace = chestburster.FindHidingPlace();

            if (hidingPlace == null) return null;
            
            Log.Message("...in " + hidingPlace.Label);
            
            return new Job(JobDefOf.THU_GoIntoHiding, hidingPlace.Position);
        }
    }
}