using System;
using System.Linq;
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
            if (pawn.CurJob?.def == JobDefOf.THU_GoIntoHiding)
                return null;
            
            Log.Message("HIDING!");

            var hidingPlace = FindHidingPlace(pawn);

            if (hidingPlace == null) return null;
            
            Log.Message("...in " + hidingPlace.Label);
            
            return new Job(JobDefOf.THU_GoIntoHiding, hidingPlace.Position);
        }
        
        private static Thing FindHidingPlace(Pawn pawn)
        {
            const int maxDistance = 18;

            var awesomeHidingPlace = ClosestThingReachable(pawn, maxDistance, IsAwesomeHidingPlace);
            
            return awesomeHidingPlace ?? ClosestThingReachable(pawn, maxDistance, IsOKHidingPlace);
        }

        private static Thing ClosestThingReachable(Pawn pawn, int maxDistance, Predicate<Thing> filter)
        {
            return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map,
                ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(pawn),
                maxDistance, filter);
        }

        private static bool IsAwesomeHidingPlace(Thing thing)
        {
            return typeof(Building_Vent) == thing.def.thingClass || 
                IsAnyOf(thing, 
                    ThingDefOf.Ship_Beam, 
                    ThingDefOf.Ship_Reactor,
                    ThingDefOf.Ship_CryptosleepCasket,
                    ThingDefOf.Ship_ComputerCore,
                    ThingDefOf.Ship_Engine,
                    ThingDefOf.Ship_SensorCluster);
        }

        private static bool IsOKHidingPlace(Thing thing)
        {
            return IsAnyOf(thing, 
                ThingDefOf.Grave, 
                ThingDefOf.SolarGenerator,
                ThingDefOf.GeothermalGenerator, 
                ThingDefOf.Cooler, 
                ThingDefOf.VanometricPowerCell,
                ThingDefOf.CommsConsole);
        }
        
        private static bool IsAnyOf(Thing thing, params ThingDef[]  thingDefs)
        {
            return thingDefs.Select(td => td.GetType()).Contains(thing.def.GetType());
        }
    }
}