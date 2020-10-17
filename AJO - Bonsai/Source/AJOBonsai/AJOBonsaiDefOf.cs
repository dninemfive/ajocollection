using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AJOBonsai
{
    [DefOf]
    public static class AJOBonsaiDefOf
    {
        public static JobDef D9AJO_J_MaintainBonsai;

        static AJOBonsaiDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(AJOBonsaiDefOf));
        }
    }
}
