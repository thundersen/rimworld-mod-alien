using Verse;

namespace Alien
{
    public class FaceHuggedDamageWorker : DamageWorker
    {
        public override DamageResult Apply(DamageInfo dinfo, Thing victim)
        {
            var hitPawn = victim as Pawn;

            var hediff = HediffMaker.MakeHediff(HediffDef.Named("THU_FaceHugged"), hitPawn);
            
            hitPawn?.health?.AddHediff(hediff);

            RemoveFacehugger(dinfo.Instigator);

            return base.Apply(dinfo, victim);
        }

        private static void RemoveFacehugger(Thing facehugger)
        {
            // the facehugger still seems to try one more attack
            // https://trello.com/c/0Ps9Y1ZQ/27-bug-nullexception-after-removing-facehugger-on-successful-hug
            facehugger?.Destroy();
        }
    }
    
    /*
        var rand = Rand.Value; 
        if (rand <= Def.AddHediffChance) 
        {
            if (hitPawn != null)
            {
                Messages.Message("TST_PlagueBullet_SuccessMessage".Translate(launcher.Label, hitPawn.Label),
                    new MessageTypeDef());

                //This checks to see if the character has a heal differential, or hediff on them already.
                var plagueOnPawn = hitPawn.health?.hediffSet?.GetFirstHediffOfDef(Def.HediffToAdd);
                var randomSeverity = Rand.Range(0.15f, 0.30f);
                if (plagueOnPawn != null)
                {
                    //If they already have plague, add a random range to its severity.
                    //If severity reaches 1.0f, or 100%, plague kills the target.
                    plagueOnPawn.Severity += randomSeverity;
                }
                else
                {
                    //These three lines create a new health differential or Hediff,
                    //put them on the character, and increase its severity by a random amount.
                    var hediff = HediffMaker.MakeHediff(Def.HediffToAdd, hitPawn);
                    hediff.Severity = randomSeverity;
                    hitPawn.health?.AddHediff(hediff, null, null);
                }
            }
        }
        else //failure!
        {
            
                // * Motes handle all the smaller visual effects in RimWorld.
                // * Dust plumes, symbol bubbles, and text messages floating next to characters.
                // * This mote makes a small text message next to the character.
                 
            MoteMaker.ThrowText(hitThing.PositionHeld.ToVector3(), hitThing.MapHeld, "TST_PlagueBullet_FailureMote".Translate(Def.AddHediffChance), 12f);
            
        }
    */
}