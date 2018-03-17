﻿//What I need

using RimWorld;
using Verse;
using Verse.AI;
//Maybe?

namespace Alien
{
    public class Facehugger : Pawn
    {
        private IntVec3 lastSpot = IntVec3.Invalid;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref lastSpot, "lastSpot");
        }
         
        //Do I need this?
        public override void PreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            base.PreApplyDamage(dinfo, out absorbed);

            if (!(dinfo.Instigator is Pawn)) return;
            if (jobs == null) return;
            if (jobs.curJob.def == JobDefOf.AttackMelee) return;
            var j = new Job(JobDefOf.AttackMelee, dinfo.Instigator)
            {
                expiryInterval = 1000,
                checkOverrideOnExpire = true,
                expireRequiresEnemiesNearby = true
            };
            jobs.EndCurrentJob(JobCondition.Incompletable);
            jobs.TryTakeOrderedJob(j);
        }
/*
        public override void TickRare()
        {
            base.TickRare();
            ObservationEffect();
        }

        public Predicate<Thing> Predicate => delegate (Thing t)
        {
            if (t == null)
                return false;
            if (t == this)
                return false;
            if (!t.Spawned)
                return false;
            Pawn pawn1 = t as Pawn;
            if (pawn1 == null)
                return false;
            if (pawn1.Dead)
                return false;
            if (pawn1 is CosmicHorrorPawn)
                return false;
            if (pawn1.Faction == null)
                return false;
            if (this.Faction != null && pawn1.Faction != null)
            {
                if (this.Faction == pawn1.Faction)
                    return false;
                if (!this.Faction.HostileTo(pawn1.Faction))
                    return false;
            }

            if (pawn1.needs == null)
                return false;
            if (pawn1.needs.mood == null)
                return false;
            if (pawn1.needs.mood.thoughts == null)
                return false;
            if (pawn1.needs.mood.thoughts.memories == null)
                return false;
            return true;
        };

        public void ObservationEffectLive(Pawn target)
        {
            try
            {
                if (this.StoringBuilding() == null && target.RaceProps.Humanlike)
                {
                    Thought_MemoryObservation thought_MemoryObservation;
                    thought_MemoryObservation = (Thought_MemoryObservation)ThoughtMaker.MakeThought(DefDatabase<ThoughtDef>.GetNamed("Observed" + this.def.ToString()));
                    thought_MemoryObservation.Target = this;
                    target.needs.mood.thoughts.memories.TryGainMemory(thought_MemoryObservation);
                }

                ///This area gives sanity loss, if the witness sees a living cosmic horror.
                Cthulhu.Utility.ApplySanityLoss(target, this.sanityLossRate, this.sanityLossMax);
            }
            catch (NullReferenceException)
            { }
        }

        public void ObservationEffectDead(Pawn target)
        {
            try
            {
                Corpse ourBody = this.Corpse;
                if (ourBody == null) return;

                ///This area gives the thought for witnessing a cosmic horror.
                if (this == null) return;
                if (!ourBody.Spawned) return;
                if (ourBody.StoringBuilding() == null)
                {
                    ThoughtDef defToImplement = DefDatabase<ThoughtDef>.GetNamedSilentFail("Observed" + this.def.ToString());
                    if (defToImplement == null) return;
                    Thought_MemoryObservation thought_MemoryObservation;
                    thought_MemoryObservation = (Thought_MemoryObservation)ThoughtMaker.MakeThought(defToImplement);
                    thought_MemoryObservation.Target = this;
                    target.needs.mood.thoughts.memories.TryGainMemory(thought_MemoryObservation);
                }
            }
            catch (NullReferenceException)
            { }
        }

        /// <summary>
        /// Checks around the cosmic horror for pawns to give sanity loss.
        /// Also gives a bad memory of the experinece
        /// </summary>
        public void ObservationEffect()
        {
            try
            {
                //This finds a suitable target pawn.
                Predicate<Thing> predicate = this.Predicate;
                
                Thing thing2 = GenClosest.ClosestThingReachable(this.PositionHeld, this.MapHeld, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(this, Danger.Deadly, TraverseMode.PassDoors, false), 15, predicate);
                if (thing2 != null && thing2.Position != IntVec3.Invalid)
                {
                    if (GenSight.LineOfSight(thing2.Position, this.PositionHeld, this.MapHeld))
                    {
                        if (thing2 is Pawn target)
                        {
                            if (target.RaceProps != null)
                            {
                                if (!target.RaceProps.IsMechanoid)
                                {
                                    if (!this.isInvisible)
                                    {
                                        if (!this.Dead && this.MapHeld != null)
                                        {
                                            ObservationEffectLive(target);
                                        }
                                        else
                                        {
                                            ObservationEffectDead(target);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (NullReferenceException)
            { }
        }
*/
    }


}
