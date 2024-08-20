using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;


namespace Engine.Models
{
    public class Monster : BaseNotificationClass //notify UI of hit points changing
    {
        private int _hitPoints;

        public string Name { get; set; }
        public string ImageName {get; set;}
        public int MaximumHitPoints { get; private set; }
        public int HitPoints
        {
            get { return _hitPoints; }
            private set 
            {
                _hitPoints = value;
                OnPropertyChanged(nameof(HitPoints));
            }
        }

        public int RewardExperiencePoints { get; private set; }
        public int RewardGold { get; private set; }

        public ObservableCollection<ItemQuantity> Inventory { get; set; } // loot items from monster

        public Monster(string name, string imageName,
            int maximumHitPoints, int hitPoints,
            int rewardExperiencePoints, int rewardGold)
        {
            Name = name;
            ImageName = string.Format("pack://application:,,,/Engine;component/Images/Monsters/{0}", imageName);
            MaximumHitPoints = maximumHitPoints;
            HitPoints = hitPoints;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;

            Inventory = new ObservableCollection<ItemQuantity>();
        }
    }
}
