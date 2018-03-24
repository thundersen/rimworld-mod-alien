using Verse;

namespace Alien
{
    class FaceHuggedHediff : HediffWithComps
    {
        public override void Notify_PawnDied()
        {
            base.Notify_PawnDied();

            XenoLifecycle.Instance().SpawnChestburster(pawn);
        }
    }    
}