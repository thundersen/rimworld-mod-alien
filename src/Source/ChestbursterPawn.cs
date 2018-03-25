using System;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Alien
{
    public class ChestbursterPawn : Pawn
    {
        //can be set for debugging and must be null for release
        private readonly int? overrideXenoSpawnDelay = 2500;
        
        private const int maxDistanceVisible = 5;
        private const int maxDistanceForHidingPlace = 18;
        private const int maxDistanceForXenoSpawnWhenHiding = 30;
        
        private Map mapLastSeen;
        private bool hasDisappeared;
        

        public int  SpawnXenoAtTick { get; }
        public Map  SpawnXenoOnMap => Map ?? mapLastSeen;

        public ChestbursterPawn()
        {
            SpawnXenoAtTick = Find.TickManager.TicksGame +
                overrideXenoSpawnDelay ?? GameTime.RandomTickNextNight(Find.TickManager.TicksGame, GenLocalDate.HourOfDay(this));

            Log.Message("Will spawn xeno at " + SpawnXenoAtTick + " (now its " + Find.TickManager.TicksGame + ")");
            
            XenoLifecycle.Instance().Register(this);
        }

        public override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            if (!IsOutOfSight())
                base.DrawAt(drawLoc, flip);
        }

        public override void Draw()
        {
            if (!IsOutOfSight())
                base.Draw();
        }
        
        
        public IntVec3 FindXenoSpawnPosition()
        {
            return hasDisappeared
                ? CellFinderLoose.RandomCellWith(IsSuitableXenoSpawnPosition, mapLastSeen) 
                : Position;
        }

        private bool IsSuitableXenoSpawnPosition(IntVec3 p)
        {
            return p.DistanceTo(Position) < maxDistanceForXenoSpawnWhenHiding;
        }

        public void DisappearInHidingPlace()
        {
            mapLastSeen = Map;
            
            Destroy();
            
            hasDisappeared = true;

            Messages.Message("THU_Chestburster_InHidingPlace".Translate(), MessageTypeDefOf.NegativeEvent);
        }

        private bool IsOutOfSight()
        {
            var anythingThatPreventsHiding = 
                GenClosest.ClosestThingReachable(Position, Map, 
                    ThingRequest.ForGroup(ThingRequestGroup.Pawn), 
                    PathEndMode.OnCell, 
                    TraverseParms.For(this, Danger.Deadly, TraverseMode.PassDoors), 
                    maxDistanceVisible, 
                    PreventsHiding);

            return anythingThatPreventsHiding == null;
        }

        private bool PreventsHiding(Thing other)
        {
            if (other == this || other.Faction?.def?.defName == Faction?.def?.defName)
            {
                return false;
            }

            var otherPawn = other as Pawn;
            return otherPawn != null &&
                   otherPawn.Spawned &&
                   !otherPawn.RaceProps.Animal;
        }
        
        public Thing FindHidingPlace()
        {
            return FindClosest(IsAwesomeHidingPlace) ?? FindClosest(IsOKHidingPlace);
        }

        private Thing FindClosest(Predicate<Thing> filter)
        {
            return GenClosest.ClosestThingReachable(Position, Map,
                ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(this),
                maxDistanceForHidingPlace, filter);
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