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
    /// <summary>
    /// Displays picture of selected cartridge, if present, or empty cartridge icon otherwise. When clicked, creates a dropdown of all owned cartridges to select from.
    /// 
    /// When a cartridge is selected but not installed, a job will be assigned to add the cartridge; if no cartridge is selected but one is installed, a job will be assigned to remove it.
    /// </summary>
    class Command_SelectCartridge : Command
    {
        // TODO: properly cache this. Either reload periodically (n > 1) or use some kind of Dirty() or just carefully keep updated
        private static List<Thing> tmpOwnedCartridges = new List<Thing>();

        public Building_Arcade Arcade;
        public Thing SelectedCart;
        // gray out texture if cartridge not yet installed; maybe give it an outline too?
        // or use e.g. the invisibility shader, perhaps
        private Graphic Graphic => SelectedCart.Graphic;
        private static readonly Texture2D NoCartSelected;

        public Command_SelectCartridge()
        {
            base.icon = NoCartSelected;
            base.defaultLabel = "D9AJO_ArcadeSelectCartridge".Translate();
        }
        
        // TODO: defined behavior for multiple selected arcades. Can't be like Universal Fermenter because cartridges are (theoretically) unique.
        // Should probably just gray out the gizmo when > 1 selected.
        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            List<FloatMenuOption> options = new List<FloatMenuOption>();
            tmpOwnedCartridges.Clear();
            // HaulableEver is just used to narrow down enumerated items as much as possible, if there's a way to get even fewer things without hardcoding the def that would be great            
            tmpOwnedCartridges = Arcade.Map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver).Where(x => x.TryGetComp<CompArcadeCartridge>() != null).ToList();
            // Handling the case where the player has too many cartridges. Doesn't handle them having too many cartridges and none of them are near the arcade, but at least I'm not going to fill their screen with options
            if (tmpOwnedCartridges.Count > AjoArcadesSettings.maxCartridgesBeforeSelectInRadius)
            {
                Messages.Message("D9AJO_TooManyCartridges".Translate(), Arcade, MessageTypeDefOf.CautionInput);
                tmpOwnedCartridges = Arcade.Map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver).Where(
                    x => x.Position.InHorDistOf(Arcade.Position, AjoArcadesSettings.maxDistanceForCartridgesWhenTooMany) 
                      && x.TryGetComp<CompArcadeCartridge>() != null).ToList();
            }
            tmpOwnedCartridges.SortBy(x => x.Label);
            foreach(Thing cart in tmpOwnedCartridges)
            {
                options.Add(new FloatMenuOption(cart.Label, delegate
                {
                    // change target cart on arcade; assign job to change, if possible
                }));
            }
            Find.WindowStack.Add(new FloatMenu(options));
        }

    }
}
