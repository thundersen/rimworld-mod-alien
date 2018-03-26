using RimWorld;
using Verse;
using Verse.AI;

namespace Alien
{
    public class EggBuilding : Building
    {
        private const int eggRadius = 2;
        private const int warmupTicks = 500;

        private int lifespanTicks;
        
        public override void Tick()
        {
            base.Tick();

            lifespanTicks++;

            if (lifespanTicks > warmupTicks && HasTargetCloseBy())
                Hatch();
        }

        private bool HasTargetCloseBy()
        {
            var targetPawn = GenClosest.ClosestThingReachable(Position, Map,
                ThingRequest.ForGroup(ThingRequestGroup.Pawn), 
                PathEndMode.OnCell, 
                TraverseParms.For(TraverseMode.ByPawn),
                eggRadius);
            return targetPawn != null;
        }

        private void Hatch()
        {
            var spawnXenoPawn = XenoLifecycle.SpawnXenoPawn("THU_Facehugger", Position, Map);
            
            spawnXenoPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, forceWake: true);
           
            GenSpawn.Spawn(ThingDef.Named("THU_XenoEggHatched"), Position, Map);
        }
    }
}