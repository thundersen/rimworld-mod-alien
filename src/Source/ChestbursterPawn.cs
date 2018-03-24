using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Alien
{
    public class ChestbursterPawn : Pawn
    {
        private bool isInHidingPlace;
        
        private bool ishidden;

        private long ticksSinceBorn;
        
        
        public void HideInHidingPlace()
        {
            isInHidingPlace = true;
            
            //ToDo: stop stargin jobs; just despawn, destroy or whatever and wait until xeno can be spawned
            //Or: Just schedule the xenomorph and then destroy
            
            
            Destroy();
            Messages.Message("Chestburster is hiding now and can't be found before transforming".Translate(),
                MessageTypeDefOf.NegativeEvent);
        }
        
        public override void Tick()
        {
            base.Tick();

            DecideVisibilityForPlayer();

            ticksSinceBorn++;

            if (ticksSinceBorn > 1000 && Rand.Value < .001f)
                TransformIntoXenomorph();
        }

        private void DecideVisibilityForPlayer()
        {
            if (isInHidingPlace)
                return;

            var canHideNow = false;//  IsOutOfSight();

            if (ishidden && !canHideNow)
                Log.Message("Chestburster was discovered");
            else if (!ishidden && canHideNow)
                Log.Message("Chestburster is hiding now");

            ishidden = canHideNow;
        }

        private void TransformIntoXenomorph()
        {
            SpawnXenomorph();

            Destroy();
        }

        private void SpawnXenomorph()
        {
            var xenomorph = CreateXenomorph();
            xenomorph.Position = Position;
            xenomorph.SpawnSetup(Map, false);
        }

        private static Pawn CreateXenomorph()
        {
            var faction = FactionUtility.DefaultFactionFrom(FactionDef.Named("THU_Xenomorph"));
            
            var pawnKindDef = PawnKindDef.Named("THU_XenomorphDrone");

            var request = new PawnGenerationRequest(pawnKindDef, newborn: true, faction: faction);
            
            var xenomorph = PawnGenerator.GeneratePawn(request);
            
            Find.LetterStack.ReceiveLetter("XENOMORPH!", "", LetterDefOf.ThreatBig, xenomorph);
            
            return xenomorph;
        }
        
        public override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            if (!ishidden)
                base.DrawAt(drawLoc, flip);
        }

        public override void Draw()
        {
            if (!ishidden)
                base.Draw();
        }
        
        private bool IsOutOfSight()
        {
            const int maxDistance = 5;
            var anythingThatPreventsHiding = GenClosest.ClosestThingReachable(Position, MapHeld, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(this, Danger.Deadly, TraverseMode.PassDoors), maxDistance, PreventsHiding);

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
    }
}