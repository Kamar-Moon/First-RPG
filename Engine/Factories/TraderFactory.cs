using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    public static class TraderFactory
    {
        private static readonly List<Trader> _traders = new List<Trader>();

        static TraderFactory()
        {
            // Create Traders & add items to their inventory
            Trader MaryAnne = new Trader("Mary-Anne");
            MaryAnne.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            Trader FarmerJeb = new Trader("Farmer Jeb");
            FarmerJeb.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            Trader LilyTheHerbilist = new Trader("Lily The Herbilist");
            LilyTheHerbilist.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            // Add created Traders to _traders list
            AddTraderToList(MaryAnne);
            AddTraderToList(FarmerJeb);
            AddTraderToList(LilyTheHerbilist);
        }
        public static Trader GetTraderByName(string name)
        {
            // Return the first trader object that matches the passed in name param 
            return _traders.FirstOrDefault(t => t.Name == name);
        }
        private static void AddTraderToList(Trader trader)
        {
            // We want Traders to have unique names 
            // If passed in Trader object's name matches the name of any trader in the _traders list
            // throw exception. Else add the Trader oject to list
            if (_traders.Any(t => t.Name == trader.Name))
            {
                throw new ArgumentException($"There is already a trader named '{trader.Name}");
            }
            _traders.Add(trader);
        }
    }
}
