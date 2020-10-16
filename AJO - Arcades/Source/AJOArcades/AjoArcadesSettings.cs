using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;

namespace AJOArcades
{
    class AjoArcadesSettings : ModSettings
    {
        public static Dictionary<SkillDef, Color> skillColors;
        public static int maxCartridgesBeforeSelectInRadius = 100;
        public static float maxDistanceForCartridgesWhenTooMany = 15f;
    }
}
