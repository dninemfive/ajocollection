using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;

namespace AJOMisc
{
    public class JobDriver_Dartboart : JobDriver_WatchBuilding
    {
        private static int dartThrowInterval = 400; // probably not worth exposing to XML. Not using const just in case someone needs to change.
        private static float dartSpeed = 20f;

        // Potential optimization: cache pawn's tick offset on Notify_JobStarting. Probably not worth the hassle.

        protected override void WatchTickAction()
        {
            if (base.pawn.IsHashIntervalTick(dartThrowInterval))
            {
                ThrowDart(pawn, TargetA.Cell);
            }
            base.WatchTickAction();
        }

        public static void ThrowDart(Pawn thrower, IntVec3 target)
        {
            // if the pawn can't throw or the map is saturated with motes, do nothing
            if (!thrower.Position.ShouldSpawnMotesAt(thrower.Map) || thrower.Map.moteCounter.SaturatedLowPriority) return;
            Vector3 preciseTarget = target.ToVector3Shifted();
            // Give each player a unique dart. Assumes the relevant defs exist and that at most three players play, otherwise everyone else will default to green.
            ThingDef dart = AjoMiscDefOf.AJO_Mote_GreenDart;
            if (target.x == thrower.Position.x - 1 || target.z == thrower.Position.z - 1) dart = AjoMiscDefOf.AJO_Mote_RedDart;
            else if (target.x == thrower.Position.x + 1 || target.z == thrower.Position.z + 1) dart = AjoMiscDefOf.AJO_Mote_BlueDart;
            // Transforms from the dart. Copied directly from the original.
            MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(dart, null);
            moteThrown.Scale = 1f;
            moteThrown.rotationRate = 0f;
            moteThrown.exactPosition = thrower.DrawPos;
            moteThrown.exactRotation = (preciseTarget - moteThrown.exactPosition).AngleFlat();
            moteThrown.SetVelocity((preciseTarget - moteThrown.exactPosition).AngleFlat(), dartSpeed);
            moteThrown.MoveAngle = (preciseTarget - moteThrown.exactPosition).AngleFlat();
            moteThrown.airTimeLeft = (moteThrown.exactPosition - preciseTarget).MagnitudeHorizontal() / dartSpeed;

            // Throw the dart
            GenSpawn.Spawn(moteThrown, thrower.Position, thrower.Map);
        }
    }
    [StaticConstructorOnStartup]
    public static class AjoMiscDefOf
    {
        public static ThingDef AJO_Mote_RedDart, AJO_Mote_GreenDart, AJO_Mote_BlueDart;

        static AjoMiscDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AjoMiscDefOf));
        }
    }
}
