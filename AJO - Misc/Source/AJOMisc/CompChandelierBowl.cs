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
        private static readonly Material BowlMat = MaterialPool.MatFrom("Cupro/Object/Furniture/Chandelier/New/LargeChandelierGlass");
        
        public override void PostDraw()
        {
            Matrix4x4 matrix = default(Matrix4x4);
            Vector2 drawSize = base.parent.def.graphicData.drawSize;
            matrix.SetTRS(base.parent.TrueCenter(),
                          Quaternion.identity,
                          new Vector3(drawSize.x, drawSize.y, 1f)); // I'm actually not sure if Vector2.y corresponds to Vector3.y or Vector3.z in RW's world, but this shouldn't:tm: matter
            Graphics.DrawMesh(MeshPool.plane10, default(Matrix4x4), BowlMat, 0);
        }

        public override string CompInspectStringExtra()
        {
            string ret = base.CompInspectStringExtra();
            if (Prefs.DevMode) ret += "CompChandelierBowl";
            return ret;
        }
    }
}
