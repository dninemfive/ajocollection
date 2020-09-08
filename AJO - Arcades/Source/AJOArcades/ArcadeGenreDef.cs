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
#pragma warning disable CS0649
        List<SkillWeight> requiredSkills;
        public RulePackDef gameNameGenerator;
        public RulePackDef gameDescGenerator;
        public ColorGenerator gameColorGenerator;
#pragma warning restore CS0649
    }
    public class SkillWeight
    {
        public SkillDef skill;
        public float weight;
    }
}
