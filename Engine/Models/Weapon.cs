using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Weapon :GameItem
    {
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public Weapon(int itemTypeID, string name, int price, int minDamage, int maxDamge) 
            : base(itemTypeID, name, price) //pass in the parameters to this class, they get sent to the base/parent class were they will be set.
        {
            MinDamage = minDamage;
            MaxDamage = MaxDamage;
        }


        public new Weapon Clone() //Override GameItem function of same name with new
        {
            return new Weapon(ItemTypeID, Name, Price, MinDamage, MaxDamage);
        }
    }
}
