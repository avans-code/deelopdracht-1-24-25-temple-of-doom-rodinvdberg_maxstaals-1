﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temple_of_doom.Model
{
    public class Key
    {
        public List<Door> CompatibleDoors { get; } = new();

        public void AddCompatibleDoor(Door door)
        {
            CompatibleDoors.Add(door);
        }

        public bool OpenDoor(Door door)
        {
            if (CompatibleDoors.Contains(door))
            {
                door.Open();
                return true;
            }

            Console.WriteLine("Key does not fit this door.");
            return false;
        }
    }
}
