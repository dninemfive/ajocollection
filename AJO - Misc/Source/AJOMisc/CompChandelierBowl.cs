using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace AJOMisc
{
    [StaticConstructorOnStartup]
    class CompChandelierBowl : ThingComp
    {
        private static readonly Material BowlMat = MaterialPool.MatFrom("Cupro/Object/Furniture/Chandelier/New/LargeChandelierGlass");
        
        public override void PostDraw()
        {
            //todo: SetTRS to calculate size of parent
            Matrix4x4 matrix = default(Matrix4x4);
            matrix.SetTRS(base.parent.DrawPos, default(Quaternion), new Vector3(base.parent.def.graphicData.drawSize.x, 1f, base.parent.def.graphicData.drawSize.y));
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
