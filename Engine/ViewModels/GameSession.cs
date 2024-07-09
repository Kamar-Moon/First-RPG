﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.ViewModels
{
    public class GameSession
    {
         public Player CurrentPlayer { get; set; }

        public GameSession()
        {
            CurrentPlayer = new Player();
            CurrentPlayer.Name = "Kamar";
            CurrentPlayer.CharacterClass = "Fighter";
            CurrentPlayer.Gold = 1000;
            CurrentPlayer.ExperiencePoints = 0;
            CurrentPlayer.Level = 1;
        }

    }
}