using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AJOArcades
{
    public class ArcadeGenreDef : Def
    {
        List<SkillWeight> requiredSkills;
        RulePackDef gameNameGenerator;
        RulePackDef gameDescGenerator;
    }
    public class SkillWeight
    {
        public SkillDef skill;
        public float weight;
    }
}
