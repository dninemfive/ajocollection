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
        private float workDone = 0f, workRequired = 250f; // TODO: mod settings (also, should be able to be tweaked per-def)

        public float xpPerTick = SkillTuning.XpPerTickGrowing * 1.2f; // TODO: mod settings to tweak this value

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            LocalTargetInfo target = base.job.targetA;
            return target.IsValid && base.pawn.Reserve(target, job);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            // taken from JobDriver_PlantWork: validate current jobs
            yield return Toils_JobTransforms.MoveCurrentTargetIntoQueue(TargetIndex.A);
            // this one probably isn't needed, should look at removing it
            Toil initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A);
            yield return initExtractTargetFromQueue;
            yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
            yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
            // go to bonsai
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
            // do progress bar while standing near bonsai
            Toil maintain = new Toil();
            maintain.tickAction = delegate
            {
                Pawn pawn = maintain.actor;
                if(pawn.skills != null)
                {
                    pawn.skills.Learn(SkillDefOf.Plants, xpPerTick);
                }
                workDone += pawn.GetStatValue(StatDefOf.PlantWorkSpeed);
                if(workDone >= workRequired)
                {
                    // TODO: "bonsai maintained" sound effect?
                    workDone = 0f;
                    base.ReadyForNextToil();
                }
            };
            maintain.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            maintain.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            maintain.defaultCompleteMode = ToilCompleteMode.Never;
            maintain.WithEffect(EffecterDefOf.Sow, TargetIndex.A);
            maintain.WithProgressBar(TargetIndex.A, () => workDone / workRequired, true);
            // maintain.PlaySustainerOrSound(() => ) // TODO: maintaining sound effect, maybe just use sowing one?
            maintain.activeSkill = (() => SkillDefOf.Plants);
            yield return maintain;
            // reset bonsai maintenance
            // TODO: maintenance should be proportional to pawn skill. Maybe a higher maintenance cap?
            Toil resetMaintenance = new Toil();
            resetMaintenance.tickAction = delegate
            {
                resetMaintenance.actor.CurJob.targetA.Thing.TryGetComp<CompBonsai>()?.Maintain();
            };
            resetMaintenance.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return resetMaintenance;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref workDone, "workDone", 0f, false);
        }
    }
}
