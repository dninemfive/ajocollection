﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;

namespace AJOArcades
{
    public class Building_Arcade : Building
    {
        public CompQuality Quality => base.GetComp<CompQuality>();
        public ArcadeGenreDef genre;

        public List<SkillWeight> skillWeights;

        // Cabinet color: weighted blend of skill colors
        public override Color DrawColor
        {
            get
            {
                // Want to see if I can avoid creating a helper struct here. First three floats per array are HSV; last one is the weight
                List<float[]> HSVWs = new List<float[]>();
                foreach (SkillWeight skw in skillWeights)
                {
                    Color.RGBToHSV(AjoArcadesSettings.skillColors[skw.skill], out float H, out float S, out float V);
                    HSVWs.Add(new float[] { H, S, V, skw.weight });
                }
                List<(float, float)> HWs = HSVWs.Select(x => (x[0], x[3])).ToList();
                float s_final = HSVWs.Select(x => x[1]).ToList().Max();
                List<(float, float)> VWs = HSVWs.Select(x => (x[2], x[3])).ToList();
                return Color.HSVToRGB(WeightedMean(HWs), s_final, WeightedMean(VWs));
            }
        }
        private float WeightedMean(List<(float, float)> values)
        {
            float runningNumerator = 0, runningDenominator = 0;
            foreach ((float, float) val in values)
            {
                runningNumerator += val.Item1 * val.Item2;
                runningDenominator += val.Item2;
            }
            return runningNumerator / runningDenominator;
        }

        // Title/pad color: genre color
        // idea: ColorGenerator for each genre, generate it on the slottable and use it here
        public override Color DrawColorTwo => AjoArcadesSettings.genreColors[genre];
    }
}