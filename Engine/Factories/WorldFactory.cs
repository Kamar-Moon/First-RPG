using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    internal static class WorldFactory
    {
        internal static World CreateWorld()
        {
            World newWorld = new World();

            newWorld.AddLocation(-2, -1, "Farmer's Field",
                "There are rows of corn growing here, with gaint rats hiding between them.",
                "FarmField.png");

            newWorld.LocationAt(-2, -1).AddMonster(2, 100);

            newWorld.AddLocation(-1, -1, "Farmer's House", "This is the house of your neighbour, Farmer James.",
                "Farmhouse.png");

            newWorld.AddLocation(0, -1, "Home", "This is your home.", 
                "Home.png");

            newWorld.AddLocation(-1, 0, "Trading Shop", "This is the shop for trading, Mary runs this place.",
                 "Trader.png");

            newWorld.AddLocation(0, 0, "Town Sqaure", "The heart of the town, in the middle there is a fountain.",
                 "TownSquare.png");

            newWorld.AddLocation(1, 0, "Town Gate", "The town's mighty gate, its protects the town from gaint spiders.",
                 "TownGate.png");

            newWorld.AddLocation(2, 0, "Spider Forest", "The tress in this forest house the creepy crawly spiders. Spider webs cover these trees.",
                 "SpiderForest.png");

            newWorld.LocationAt(2,0).AddMonster(3, 100);

            newWorld.AddLocation(0, 1, "Herbalist's Hut", "A small quaint hut that has herbs drying from the roof.",
                "HerbalistsHut.png");

            newWorld.LocationAt(0, 1).QuestsAvailableHere.Add(QuestFactory.GetQuestID(1)); //no need for temp variable

            newWorld.AddLocation(0, 2, "Herbalist's Garden", "There are many mysterious plants here, watch out, snakes are hiding in them!",
               "HerbalistsGarden.png");

            newWorld.LocationAt(0, 2).AddMonster(1, 100);

            return newWorld;
        }
    }
}
