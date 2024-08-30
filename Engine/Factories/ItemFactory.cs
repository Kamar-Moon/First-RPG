using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    public static class ItemFactory
    {
        private static List<GameItem> _standardGameItems;

        static ItemFactory()
        {
            _standardGameItems = new List<GameItem>(); //new empty List of type GameItem

            _standardGameItems.Add(new Weapon(1001, "Pointy Stick", 1, 1, 2));
            _standardGameItems.Add(new Weapon(1002, "Rusty Sword", 5, 1, 3));
            _standardGameItems.Add(new GameItem(9001, "Snake Fang", 1));
            _standardGameItems.Add(new GameItem(9002, "Snake Skin", 2));
            _standardGameItems.Add(new GameItem(9003, "Rat Tail", 1));
            _standardGameItems.Add(new GameItem(9004, "Rat Fur", 2));
            _standardGameItems.Add(new GameItem(9005, "Spider Fang", 1));
            _standardGameItems.Add(new GameItem(9006, "Spider Silk", 2));

        }

        //Create Copy of item
        public static GameItem CreateGameItem(int itemTypeID)
        {
            //on list varible use LINQ to find first item that matches the itemType ID that matches if not found return default null
            GameItem standardItem = _standardGameItems.FirstOrDefault(item => item.ItemTypeID == itemTypeID);

            if (standardItem != null)
            {
                if(standardItem is Weapon)
                {
                    return (standardItem as Weapon).Clone(); //use the Clone function from the Weapon class to
                                                             //return copy of the Weapon object - not GameItem
                }

                return standardItem.Clone();
            }

            return null;
        }
    }
}
