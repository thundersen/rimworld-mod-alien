using RimWorld;
using Verse;

namespace Alien
{
    public class ShipChunk : Building
    {
        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            var map = Map;
            var position = Position;
            
            base.Destroy(mode);
            
            if (mode == DestroyMode.Deconstruct)
                GenSpawn.Spawn(ThingDef.Named("THU_XenoEgg"), position, map);
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            SetUpWarningLetter(map);
        }

        private void SetUpWarningLetter(Map map)
        {
            var signalTag = "xenoShipChunkApproached-" + Find.UniqueIDsManager.GetNextSignalTagID();
            var notificationArea = CellRect.CenteredOn(Position, 2);

            CreateLetter(map, signalTag, notificationArea);
            CreateTrigger(map, signalTag, notificationArea);
        }

        private static void CreateLetter(Map map, string signalTag, CellRect notificationArea)
        {
            var signalAction_Letter = (SignalAction_Letter) ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter);
            signalAction_Letter.signalTag = signalTag;
            signalAction_Letter.letter = LetterMaker.MakeLetter("THU_LetterLabel_ShipPart".Translate(),
                "THU_LetterText_ShipPart".Translate(), LetterDefOf.NeutralEvent, new TargetInfo(notificationArea.CenterCell, map));
            GenSpawn.Spawn(signalAction_Letter, notificationArea.CenterCell, map);
        }

        private static void CreateTrigger(Map map, string signalTag, CellRect notificationArea)
        {
            var rectTrigger = (RectTrigger) ThingMaker.MakeThing(ThingDefOf.RectTrigger);
            rectTrigger.signalTag = signalTag;
            rectTrigger.Rect = notificationArea.ClipInsideMap(map);
            rectTrigger.destroyIfUnfogged = false;
            GenSpawn.Spawn(rectTrigger, notificationArea.CenterCell, map);
        }
    }
}