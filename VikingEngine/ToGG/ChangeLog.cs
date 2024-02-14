using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG
{
    static class ChangeLog
    {
        public static string Text()
        {
            return PlatformSettings.SteamVersion + Environment.NewLine +
                Environment.NewLine +
                "DEVELOPMENT NOTES" + Environment.NewLine +
                "Working towards a small release, will mostly work on visuals and sound from here on." + Environment.NewLine +

                Environment.NewLine +
                "CHANGE LOG" + Environment.NewLine +

                "Single player mode is ready, but untested" + Environment.NewLine +
                "Death/Respawn HUD" + Environment.NewLine +

                Environment.NewLine +
                "1st version of the demo is complete" + Environment.NewLine +
                "Remade the Cyclops level" + Environment.NewLine +
                "Achievements added" + Environment.NewLine +
                "Rebalanced most of the stronger enemies" + Environment.NewLine +
                "Reduced battle dice damage by 30%, increased surges by 100% (to give more player control)" + Environment.NewLine +
                "Redesigned unit info hud" + Environment.NewLine +
                "Dynamic attack roll speed" + Environment.NewLine +
                "Redesign of tutorial" + Environment.NewLine +
                "Projectile recruit hero variant" + Environment.NewLine +

                Environment.NewLine +
                "Lobby ready check" + Environment.NewLine +
                "Fourth and final story level for the demo" + Environment.NewLine +
                "Big overhaul on all the story levels" + Environment.NewLine +
                "Campfire gives Rest to the heroes" + Environment.NewLine +
                "New knight ultimate" + Environment.NewLine +
                "Stun condition" + Environment.NewLine +
                "Fixed version number" + Environment.NewLine +

                Environment.NewLine +
                "Third story level prototype is ready" + Environment.NewLine +
                "Bloated goblin and Cannon troll units" + Environment.NewLine +
                "Second story level prototype is ready" + Environment.NewLine +
                "Doors opened with rune keys" + Environment.NewLine +

                Environment.NewLine +
                "Removed 3 damage from attack dice" + Environment.NewLine +
                "First story level prototype is ready" + Environment.NewLine +
                "Editor place gadgets" + Environment.NewLine +
                "Editor copy/paste" + Environment.NewLine +
                "Three new goblin monsters" + Environment.NewLine +

                Environment.NewLine +
                "Three new melee minions" + Environment.NewLine +
                "Khaja knife dance strategy" + Environment.NewLine +
                "Khaja pet rat" + Environment.NewLine +

                Environment.NewLine +
                "Send items to adjacent players (backpack)" + Environment.NewLine +
                "Ghost lobbies fix" + Environment.NewLine +
                "New hero: Khaja" + Environment.NewLine +

                Environment.NewLine +
                "Tutorial" + Environment.NewLine +
                "Drag n drop for units uses pathfinding assistance" + Environment.NewLine +
                "Game is stable with 3-4 players now" + Environment.NewLine +
                "Action buttons" + Environment.NewLine +

                Environment.NewLine +
                "New hero: the recruit" + Environment.NewLine +
                "New monster: dark priest" + Environment.NewLine +
                "Units Tags added to editor, Killing all tagged units will end the game" + Environment.NewLine +
                "The layout of the Nagaboss level is randomized, you can use RoomFlags in the editor for the same result" + Environment.NewLine;
        }
    }
}
