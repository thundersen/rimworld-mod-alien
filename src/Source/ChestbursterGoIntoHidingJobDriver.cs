using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace Alien
{
    public class ChestbursterGoIntoHidingJobDriver : JobDriver_Flee
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            return base.MakeNewToils()
                .Concat(new Toil
                {
                    atomicWithPrevious = true,
                    defaultCompleteMode = ToilCompleteMode.Instant,
                    initAction = () => XenoLifecycle.HideChestburster(pawn)
                });
        }
        
    }
}