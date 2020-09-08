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
    public class CompArcadeCartridge : ThingComp
    {
        private static HashSet<string> existingNames = new HashSet<string>();

        public ArcadeGenreDef Genre;
        public TaggedString Name = null, Desc = null;
        private Color? color = null;
        public Color Color
        {
            get
            {
                if (color == null) color = Genre.gameColorGenerator.NewRandomizedColor();
                return (Color)color;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (Name == null) Name = NameGenerator.GenerateName(Genre.gameNameGenerator, existingNames, true); // generate name from genre name generator
            existingNames.Add(Name);
            if (Desc == null) Desc = TaleReference.Taleless.GenerateText(TextGenerationPurpose.ArtDescription, Genre.gameDescGenerator); // generate name from genre desc generator
            if (color == null) color = Genre.gameColorGenerator.NewRandomizedColor();
        }
    }
}
