using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace AJOArcades
{
    public class CompArcadeCartridge : ThingComp //: CompSlottable
    {
        public ArcadeGenreDef genre;
        public string name = null, desc = null;
        private Color? color = null;
        public Color Color
        {
            get
            {
                if (color == null) color = genre.gameColorGenerator.NewRandomizedColor();
                return (Color)color;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (name == null) ; // generate name from genre name generator
            if (desc == null) ; // generate name from genre desc generator
            if (color == null) color = genre.gameColorGenerator.NewRandomizedColor();
        }
    }
}
