using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.Factories;
using Engine.EventArgs;
using System.Collections.Specialized;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {
        public event EventHandler<GameMessagesEventArgs> OnMessageRaised; //This is how the View “subscribes” to the event
                                                                          //GameMessageEventArgs which tells the subscribers what type
                                                                          //of event arguments to look for, so they can use the data
                                                                          //passed in the event.

        private Location _currentLocation;
        private Monster _currentMonster;
        public Player CurrentPlayer { get; set; } 
        public World CurrentWorld { get; set; }
        public Location CurrentLocation

        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;

                OnPropertyChanged(nameof(CurrentLocation)); // Update location UI atrributes (image, name, description)
                OnPropertyChanged(nameof(HasLocationToNorth)); //Update the button visibility - if there is a location in that direction display button else hide.
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToWest));
                OnPropertyChanged(nameof(HasLocationToSouth));

                GivePlayerQuestAtLocation();
                GetMonsterAtLocation();
            }
        }


        public Monster CurrentMonster
        {
            get { return _currentMonster; }
            set
            {
                _currentMonster = value;
                OnPropertyChanged(nameof(CurrentMonster));
                OnPropertyChanged(nameof(HasMonster));

                if(CurrentMonster != null)
                {
                    RaiseMessage(""); // Create a blank line between messages
                    RaiseMessage($"You see a {CurrentMonster.Name} here!");
                }
            }
        }

        public Weapon CurrentWeapon { get; set; } // what wepaon does the player currently have selected.


        public bool HasLocationToNorth
        {
            get
            {
                return CurrentWorld.LocationAt(CurrentLocation.xCoordinate, CurrentLocation.yCoordinate + 1) != null;
            }
        } 

        public bool HasLocationToEast
        {
            get
            {
                return CurrentWorld.LocationAt(CurrentLocation.xCoordinate + 1, CurrentLocation.yCoordinate) != null;
            }
        }

        public bool HasLocationToWest
        {
            get
            {
                return CurrentWorld.LocationAt(CurrentLocation.xCoordinate - 1, CurrentLocation.yCoordinate) != null;
            }
        }

        public bool HasLocationToSouth
        {
            get
            {
                return CurrentWorld.LocationAt(CurrentLocation.xCoordinate, CurrentLocation.yCoordinate - 1) != null;
            }
        }

        public bool HasMonster => CurrentMonster != null;  // Doing => is the same as doing HasLocation gets above

        public GameSession()
        {
            CurrentPlayer = new Player 
                            {
                                Name ="Kami", 
                                CharacterClass ="Fighter",
                                HitPoints = 10,
                                Gold = 100,
                                ExperiencePoints = 0,
                                Level = 1
                            };

            if(!CurrentPlayer.Weapons.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001)); // Give the Player a pointy stick to start off
            }

            CurrentWorld = WorldFactory.CreateWorld();

            CurrentLocation = CurrentWorld.LocationAt(0, 0);
        }


        public void MoveNorth()
        {
            if (HasLocationToNorth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.xCoordinate, CurrentLocation.yCoordinate + 1);
            }
        }

        public void MoveEast()
        {
            if (HasLocationToSouth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.xCoordinate + 1, CurrentLocation.yCoordinate);
            }
        }
        public void MoveWest()
        {
            if (HasLocationToWest)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.xCoordinate - 1, CurrentLocation.yCoordinate);
            }
        }

        public void MoveSouth()
        {
            if (HasLocationToSouth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.xCoordinate, CurrentLocation.yCoordinate - 1);
            }
        }

        public void GivePlayerQuestAtLocation()
        {
            foreach(Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                //  If the Player's quest list does not have a quest with the same ID as a quest in the Location's Avalible
                // Quests, then add the Location's quests to the PLayer's quest list.
                if(!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
                    {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));
                }
            }
        }

        private void GetMonsterAtLocation()
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }

        public void AttackCurrentMonster()
        {
            if (CurrentWeapon == null)
            {
                RaiseMessage("You must select a weapon to attack, silly.");
                return; // Gaurd Clause or early escape
            }
            // If you do have a weapon proceed...
            int damageToMonster = RandomNumberGenerator.NumberBetween(CurrentWeapon.MinDamage, CurrentWeapon.MaxDamage);
            if(damageToMonster == 0)
            {
                RaiseMessage($"You missed the {CurrentMonster.Name}.");
            }
            else
            {
                CurrentMonster.HitPoints -= damageToMonster;
                RaiseMessage($"You hit the {CurrentMonster.Name} for {damageToMonster} points.");
            }
            // If monster is killed, collect the rewards and loot.
            if (CurrentMonster.HitPoints <= 0)
            {
                RaiseMessage(""); // give a break between messages
                RaiseMessage($"You defeated the {CurrentMonster.Name}!");
                CurrentPlayer.ExperiencePoints += CurrentMonster.RewardExperiencePoints;
                RaiseMessage($"You recived {CurrentMonster.RewardExperiencePoints} experience points!");
                CurrentPlayer.Gold += CurrentMonster.RewardGold;
                RaiseMessage($"You recived {CurrentMonster.RewardGold} gold!");
                foreach (ItemQuantity itemQuantity in CurrentMonster.Inventory)
                {
                    GameItem item = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                    CurrentPlayer.AddItemToInventory(item);
                    RaiseMessage($"You recived {itemQuantity.Quantity} {item.Name}.");
                }
                // Get another monster to Fight
                GetMonsterAtLocation();
            }
            else
            {
                // If the monster is still alive, let the monster attack
                int damageToPlayer = RandomNumberGenerator.NumberBetween(CurrentMonster.MiniumumDamge, CurrentMonster.MaximumDamage);
                if (damageToPlayer == 0)
                {
                    RaiseMessage("The Monster attcks, but misses you. Lucky!");
                }
                else
                {
                    CurrentPlayer.HitPoints -= damageToPlayer;
                    RaiseMessage($"The {CurrentMonster.Name} hit you for {damageToPlayer} points!");
                }
                // If the PLayer is Killed, move them back to their home + heal them.
                if (CurrentPlayer.HitPoints <= 0)
                {
                    RaiseMessage("");
                    RaiseMessage($"The {CurrentMonster.Name} killed you.");
                    CurrentLocation = CurrentWorld.LocationAt(0, -1); //Player's home
                    CurrentPlayer.HitPoints = CurrentPlayer.Level * 10; // Comepletely heal player
                }
            }
        }

        // The code checks if OnMessageRaised has any subscribers (it will be “null”, if there are no subscribers).
        // If there are subscribers, it will invoke the event, passing a new GameMessageEventArgs object that has
        // the desired message text.
        private void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessagesEventArgs(message));
        }

    }
}
