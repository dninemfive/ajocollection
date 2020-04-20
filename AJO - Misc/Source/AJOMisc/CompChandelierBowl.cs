using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;

namespace AJOMisc
{
    [StaticConstructorOnStartup]
    class CompChandelierBowl : ThingComp
    {
        private static readonly Material BowlMat = MaterialPool.MatFrom("Cupro/Object/Furniture/Chandelier/New/LargeChandelierGlass", ShaderDatabase.Transparent);
        
        public override void PostDraw()
        {
            Matrix4x4 matrix = default(Matrix4x4);
            Vector2 drawSize = base.parent.def.graphicData.drawSize;
            matrix.SetTRS(base.parent.TrueCenter(),
                          Quaternion.identity,
                          new Vector3(drawSize.x, 1f, drawSize.y));
            Graphics.DrawMesh(MeshPool.plane10, matrix, BowlMat, 0);
        }

        public override string CompInspectStringExtra()
        {
            string ret = base.CompInspectStringExtra();
            if (Prefs.DevMode) ret += "CompChandelierBowl";
            return ret;
        }
    }
}
