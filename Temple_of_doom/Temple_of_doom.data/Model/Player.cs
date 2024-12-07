﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Temple_of_doom.data.Model;

namespace Temple_of_doom.Model
{
    public class Player
    {
        private int startRoomID;
        private int[] startCoordinates;
        private int lives;
        private List<Item> backpack;
        private IDoorStrategy strategy;

        public Player(IDoorStrategy strategy)
        {
            this.strategy = strategy;
        }

        public void SetStrategy(IDoorStrategy strategy)
        {
            this.strategy = strategy;
        }

        public void OpenDoor(Door door)
        {
            strategy.OpenDoor(door);
        }

    }
}