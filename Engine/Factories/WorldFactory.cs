using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    internal class WorldFactory
    {
        internal World CreateWorld()
        {
            World newWorld = new World();

            newWorld.AddLocation(-2, -1, "Farmer's Field",
                "There are rows of corn growing here, with gaint rats hiding between them.",
                "pack://application:,,,/Engine;component/Images/Locations/FarmField.png");

            newWorld.AddLocation(-1, -1, "Farmer's House", "This is the house of your neighbour, Farmer James.",
                "pack://application:,,,/Engine;component/Images/Locations/Farmhouse.png");

            newWorld.AddLocation(0, -1, "Home", "This is your home.", 
                "pack://application:,,,/Engine;component/Images/Locations/Home.png");

            newWorld.AddLocation(-1, 0, "Trading Shop", "This is the shop for trading, Mary runs this place.",
                 "pack://application:,,,/Engine;component/Images/Locations/Trader.png");

            newWorld.AddLocation(0, 0, "Town Sqaure", "The heart of the town, in the middle there is a fountain.",
                 "pack://application:,,,/Engine;component/Images/Locations/TownSquare.png");

            newWorld.AddLocation(1, 0, "Town Gate", "The town's mighty gate, its protects the town from gaint spiders.",
                 "pack://application:,,,/Engine;component/Images/Locations/TownGate.png");

            newWorld.AddLocation(2, 0, "Spider Forest", "The tress in this forest house the creepy crawly spiders. Spider webs cover these trees.",
                 "pack://application:,,,/Engine;component/Images/Locations/SpiderForest.png");

            newWorld.AddLocation(0, 1, "Herbalist's Hut", "A small quaint hut that has herbs drying from the roof.",
                "pack://application:,,,/Engine;component/Images/Locations/HerbalistsHut.png");

            newWorld.AddLocation(0, 2, "Herbalist's Garden", "There are many mysterious plants here, watch out, snakes are hiding in them!",
               "pack://application:,,,/Engine;component/Images/Locations/HerbalistsGarden.png");

            return newWorld;
        }
    }
}
