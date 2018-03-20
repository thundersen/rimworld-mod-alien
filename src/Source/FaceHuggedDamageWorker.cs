using RimWorld;
using Verse;

namespace Alien
{
    public class FaceHuggedDamageWorker : DamageWorker
    {
        public override DamageResult Apply(DamageInfo dinfo, Thing victim)
        {
            if (Rand.Value <= .5f)
                HugFace(dinfo, victim);

            return base.Apply(dinfo, victim);
        }

        private static void HugFace(DamageInfo dinfo, Thing victim)
        {
            var hitPawn = victim as Pawn;

            var hediff = HediffMaker.MakeHediff(HediffDef.Named("THU_FaceHugged"), hitPawn);

            hitPawn?.health?.AddHediff(hediff);

            RemoveFacehugger(dinfo.Instigator);
            
            Messages.Message("THU_FaceHugged_SuccessMessage".Translate(hitPawn?.Label),
                MessageTypeDefOf.NegativeEvent);
        }

        private static void RemoveFacehugger(Thing facehugger)
        {
            // the facehugger still seems to try one more attack
            // https://trello.com/c/0Ps9Y1ZQ/27-bug-nullexception-after-removing-facehugger-on-successful-hug
            facehugger?.Destroy();
        }
    }
}