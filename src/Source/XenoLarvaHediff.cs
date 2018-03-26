using Verse;

namespace Alien
{
    class XenoLarvaHediff : HediffWithComps
    {
        public override void Notify_PawnDied()
        {
            base.Notify_PawnDied();

            XenoLifecycle.Instance().SpawnChestburster(pawn);
        }
    }    
}