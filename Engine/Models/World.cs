using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class World
    {
        private List<Location> _locations = new List<Location>(); //instancite new list of locations

        internal void AddLocation(int xCoordinate, int yCoordinate, string name, string description, string imageName)  //only world class in the Engine assembly should access this function thus it is made internal
        {
            Location loc = new Location();
            loc.xCoordinate = xCoordinate;
            loc.yCoordinate = yCoordinate;
            loc.Name = name;
            loc.Description = description;
            loc.ImageName = imageName;

            _locations.Add(loc);

        }

        public Location LocationAt(int xCoordinate, int yCoordinate) //return a Location object if the x & y coordinate passed to the function match
        {
            foreach(Location loc in _locations)
            {
                if (loc.xCoordinate == xCoordinate && loc.yCoordinate == yCoordinate)
                {
                    return loc;
                }
            }
            return null;
        }
    }
}
