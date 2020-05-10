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
        public RulePackDef gameNameGenerator;
        public RulePackDef gameDescGenerator;
        public ColorGenerator gameColorGenerator;
    }
    public class SkillWeight
    {
        public SkillDef skill;
        public float weight;
    }
}
