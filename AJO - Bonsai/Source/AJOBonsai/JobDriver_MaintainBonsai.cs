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
    public class JobDriver_MaintainBonsai : JobDriver_PlantWork
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            LocalTargetInfo target = base.job.targetA;
            return target.IsValid && base.pawn.Reserve(target, job);
        }

        protected override void Init()
        {
            base.xpPerTick = SkillTuning.XpPerTickGrowing * 1.2f; // TODO: mod settings to tweak this value
        }

        protected override Toil PlantWorkDoneToil()
        {
            Toil toil = new Toil();
            toil.initAction = delegate
            {
                toil.actor.CurJob.targetA.Thing.TryGetComp<CompBonsai>()?.Maintain();
            };
            toil.defaultCompleteMode = ToilCompleteMode.Instant;
            return toil;
        }
    }
}
