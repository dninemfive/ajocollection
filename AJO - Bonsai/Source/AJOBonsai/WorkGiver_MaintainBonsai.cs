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
    class WorkGiver_MaintainBonsai : WorkGiver_Scanner
    {
        public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
        {
            Danger maxDanger = pawn.NormalMaxDanger();
            // list of bonsai which need maintenance
            List<Building> bonsai = pawn.Map.listerBuildings.allBuildingsColonist.Where(x => x.TryGetComp<CompBonsai>()?.ShouldMaintain ?? false).ToList();
            foreach(Building b in bonsai)
            {
                yield return b.Position;
            }
        }
    }
}
