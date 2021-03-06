﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using RimWorld;

namespace AJOBonsai
{
    class WorkGiver_MaintainBonsai : WorkGiver_Scanner
    {
        public override PathEndMode PathEndMode => PathEndMode.Touch;

        public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
        {
            Danger maxDanger = pawn.NormalMaxDanger();
            // list of non-forbidden bonsai which need maintenance
            List<Building> bonsai = pawn.Map.listerBuildings.allBuildingsColonist.Where(x => (x.TryGetComp<CompBonsai>()?.ShouldMaintain ?? false)
                                                                                             && !x.IsForbidden(pawn)).ToList();
            foreach(Building b in bonsai)
            {
                yield return b.Position;
            }
        }

        public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
        {
            foreach(Thing thing in c.GetThingList(pawn.Map))
            {
                if (!thing.IsForbidden(pawn) && (thing.TryGetComp<CompBonsai>()?.ShouldMaintain ?? false)) return pawn.CanReserve(thing);
            }
            return false;
        }

        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            return pawn.GetLord() != null && base.ShouldSkip(pawn, forced);
        }

        public override Job JobOnCell(Pawn pawn, IntVec3 cell, bool forced = false)
        {
            Job job = JobMaker.MakeJob(AJOBonsaiDefOf.D9AJO_J_MaintainBonsai);
            job.targetA = FirstMaintainableBonsaiInCell(cell, pawn.Map);
            return job;
        }

        // TODO: check forbidden?
        static Thing FirstMaintainableBonsaiInCell(IntVec3 cell, Map map)
        {
            return cell.GetThingList(map).Where(x => (x.TryGetComp<CompBonsai>()?.ShouldMaintain ?? false)).First();
        }
    }
}
