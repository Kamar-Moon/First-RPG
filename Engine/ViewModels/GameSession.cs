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
        private Trader _currentTrader; // backing variable for CurrentTrader property
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

                CompleteQuestsAtLocation();
                GivePlayerQuestAtLocation();
                GetMonsterAtLocation();
                CurrentTrader = CurrentLocation.TraderHere;
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

        public Trader CurrentTrader
        {
            get { return _currentTrader; }
            set
            {
                _currentTrader = value;

                OnPropertyChanged(nameof(CurrentTrader));
                OnPropertyChanged(nameof(HasTrader));
            }
        }

        public Weapon CurrentWeapon { get; set; } // what wepaon does the player currently have selected.

        // Has location Properties, can you move in that direction at the current location.
        public bool HasLocationToNorth =>
            CurrentWorld.LocationAt(CurrentLocation.xCoordinate, CurrentLocation.yCoordinate + 1) != null;


        public bool HasLocationToEast =>
            CurrentWorld.LocationAt(CurrentLocation.xCoordinate + 1, CurrentLocation.yCoordinate) != null;


        public bool HasLocationToWest => 
            CurrentWorld.LocationAt(CurrentLocation.xCoordinate - 1, CurrentLocation.yCoordinate) != null;


        public bool HasLocationToSouth => 
            CurrentWorld.LocationAt(CurrentLocation.xCoordinate, CurrentLocation.yCoordinate - 1) != null;
       

        public bool HasMonster => CurrentMonster != null;  // Doing => is the same as doing HasLocation "gets" 
        public bool HasTrader => CurrentTrader != null;

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

        private void CompleteQuestsAtLocation()
        {
            // For each quest at the location perform the following
            // If the location does not have a quest it doesnt need to go through the loop - just finishes
            foreach(Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                // If location does have quests, check the player's quest list - check for the first one were the quest id
                // matches the quest ID of the location's AND the quest is not completed.
                QuestStatus questToComplete =
                    CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID && !q.isCompleted);

                // if PLayer has an old quest that is completed it returns default
                // If there is a quest in player list that is not completed then questToComplete will be NOT null

                if (questToComplete != null)
                {
                    // If player has all items needed to complete quest, we will proceed with the following code:
                    if (CurrentPlayer.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        // Remove the quest completion items from player's inventory
                        foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                        {
                            // This loop runs for each quest item required example:
                            // if quest needs 5 snake skin to be complete - this loop would run 5 times
                            for (int i = 0; i < itemQuantity.Quantity; i++)
                            {
                                // Remove the first item in the player inventory  that matches the item.ItemTypeID with itemQuantity.ItemID
                                CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.Inventory.First(item => item.ItemTypeID == itemQuantity.ItemID));
                            }
                        }
                        RaiseMessage("");
                        RaiseMessage($"You completed the '{quest.Name}' quest!");
                        // Give Player the quest rewards
                        CurrentPlayer.ExperiencePoints += quest.RewardExperiencePoints;
                        RaiseMessage($"You recived {quest.RewardExperiencePoints} experience points!");
                        CurrentPlayer.Gold += quest.RewardGold;
                        RaiseMessage($"You recived {quest.RewardGold} gold!");
                        foreach (ItemQuantity intemQuantity in quest.RewardItems)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(intemQuantity.ItemID);
                            CurrentPlayer.AddItemToInventory(rewardItem);
                            RaiseMessage($"You recived a {rewardItem.Name}!");
                        }
                        // Mark the quest as completed
                        questToComplete.isCompleted = true;
                    }
                }
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
                    RaiseMessage("");
                    RaiseMessage($"You Recived the '{quest.Name}' quest");
                    RaiseMessage(quest.Description);
                    RaiseMessage("Return with:");
                    foreach(ItemQuantity itemQuantity in quest.ItemsToComplete)
                    {
                        RaiseMessage($" {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }
                    RaiseMessage("And you will recieve:");
                    RaiseMessage($" {quest.RewardExperiencePoints} experience points");
                    RaiseMessage($" {quest.RewardGold} gold");
                    foreach(ItemQuantity itemQuantity in quest.RewardItems)
                    {
                        RaiseMessage($" {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }

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
