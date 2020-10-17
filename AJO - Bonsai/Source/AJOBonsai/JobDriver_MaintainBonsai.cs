using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using RimWorld;

namespace AJOBonsai
{
    public class JobDriver_MaintainBonsai : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            LocalTargetInfo target = base.job.targetA;
            return target.IsValid && base.pawn.Reserve(target, job);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            // base.xpPerTick = SkillTuning.XpPerTickGrowing * 1.2f; // TODO: mod settings to tweak this value
            // taken from JobDriver_PlantWork: validate current jobs
            yield return Toils_JobTransforms.MoveCurrentTargetIntoQueue(TargetIndex.A);
            Toil initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A);
            yield return initExtractTargetFromQueue;
            yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
            yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
            // go to plant
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
            // reset bonsai maintenance
            Toil toilMaintain = new Toil();
            toilMaintain.initAction = delegate
            {
                toilMaintain.actor.CurJob.targetA.Thing.TryGetComp<CompBonsai>()?.Maintain();
            };
            toilMaintain.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return toilMaintain;
        }
    }
}
