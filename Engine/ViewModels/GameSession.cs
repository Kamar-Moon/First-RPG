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

        // The code checks if OnMessageRaised has any subscribers (it will be “null”, if there are no subscribers).
        // If there are subscribers, it will invoke the event, passing a new GameMessageEventArgs object that has
        // the desired message text.
        private void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessagesEventArgs(message));
        }

    }
}
