﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;
using D9Framework;

namespace AJOBonsai
{
    /// <summary>
    /// Tracks bonsai maintenance and modifies the beauty level accordingly.
    /// </summary>
    public class CompBonsai : CompWithCheapHashInterval
    {
        public CompProperties_Bonsai Props => (CompProperties_Bonsai)base.props;
        CompQuality Quality => base.parent.TryGetComp<CompQuality>();
        private float _maintenance;
        public float Maintenance
        {
            get
            {
                return _maintenance;
            }
            set
            {
                _maintenance = Mathf.Clamp(value, 0f, Props.maxMaintenance);
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            if (IsCheapIntervalTick(Props.tickInterval))
            {
                Maintenance -= Props.maintenanceFallPerDay * (Props.tickInterval / (float)GenDate.TicksPerDay);
                if (Rand.Value < Props.loseQualityChanceFactor.Evaluate(Maintenance))
                {
                    // TODO: add some way to cheaply set the internal quality. Currently, this will completely reset the bonsai's CompArt, which is not desired.
                    Quality.SetQuality((QualityCategory)Mathf.Max((int)Quality.Quality - 1, 0), ArtGenerationContext.Colony);
                    Messages.Message("D9AJO_BonsaiQualityDecreasedFromLackOfMaintenance", base.parent, MessageTypeDefOf.NegativeEvent);
                }                
            }
        }
    }
    public class CompProperties_Bonsai : CompProperties
    {
#pragma warning disable CS0649
        public int tickInterval = 2500;
        public float maxMaintenance = 1f, maintenanceFallPerDay = 0.9f;
        /// <summary>
        /// Chance to lose a quality level when the bonsai ticks based on its current level of maintenance. By default, this increases the lower maintenance is.
        /// </summary>
        public SimpleCurve loseQualityChanceFactor = new SimpleCurve
        {
            new CurvePoint(0f, 0.9f),
            new CurvePoint(0.25f, 0.75f),
            new CurvePoint(0.5f, 0f),
            new CurvePoint(1f, 0f)
        };
        /// <summary>
        /// Chance to gain quality when trimmed based on its current level of maintenance. By default, this increases with higher maintenance.
        /// </summary>
        public SimpleCurve gainQualityChanceFactor = new SimpleCurve
        {
            new CurvePoint(0f, 0f),
            new CurvePoint(0.5f, 1f),
            new CurvePoint(0.75f, 1.25f),
            new CurvePoint(1f, 2f)
        };
#pragma warning restore CS0649
        public CompProperties_Bonsai()
        {
            base.compClass = typeof(CompBonsai);
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (string s in base.ConfigErrors(parentDef)) yield return s;
            if (!parentDef.HasComp(typeof(CompQuality))) yield return parentDef.defName + " has CompBonsai but no CompQuality!";
        }
    }
}
