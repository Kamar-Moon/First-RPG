using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.Factories;
using System.Collections.Specialized;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {
        private Location _currentLocation;
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

    }
}
