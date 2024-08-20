using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Location
    {
        public int xCoordinate { get; set; }

        public int yCoordinate { get; set; }

        public string Name { get; set; }

        public string Description {get; set;}

        public string ImageName { get; set; }

        public List<Quest> QuestsAvailableHere { get; set; } = new List<Quest>();

    }
}
