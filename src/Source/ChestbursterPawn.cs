using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Alien
{
    public class ChestbursterPawn : Pawn
    {
        private Map mapLastSeen;
        private bool hasDisappeared;
        
        public int  SpawnXenoAtTick { get; }
        public Map  SpawnXenoOnMap => Map ?? mapLastSeen;


        public ChestbursterPawn()
        {
            SpawnXenoAtTick =
                Find.TickManager.TicksGame +
                2500; //GameTime.RandomTickNextNight(Find.TickManager.TicksGame, GenLocalDate.HourOfDay(this)),

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
                ? CellFinderLoose.RandomCellWith(p => p.DistanceTo(Position) < 30, mapLastSeen) 
                : Position;
        }
        
        public void DisappearInHidingPlace()
        {
            mapLastSeen = Map;
            
            Destroy();
            
            hasDisappeared = true;

            Messages.Message("Chestburster is hiding now and can't be found before transforming".Translate(),
                MessageTypeDefOf.NegativeEvent);
        }

        private bool IsOutOfSight()
        {
            const int maxDistance = 5;
            var anythingThatPreventsHiding = GenClosest.ClosestThingReachable(Position, Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(this, Danger.Deadly, TraverseMode.PassDoors), maxDistance, t => PreventsHiding(t, this));

            return anythingThatPreventsHiding == null;
        }

        private static bool PreventsHiding(Thing other, Thing chestburster)
        {
            if (other == chestburster || other.Faction?.def?.defName == chestburster.Faction?.def?.defName)
            {
                return false;
            }

            var otherPawn = other as Pawn;
            return otherPawn != null &&
                   otherPawn.Spawned &&
                   !otherPawn.RaceProps.Animal;
        }
    }
}