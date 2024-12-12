﻿namespace Temple_of_doom.Models
{
    public class PressurePlatePuzzle
    {
        public List<PressurePlate> Plates { get; set; } = new List<PressurePlate>();

        public bool IsSolved => Plates.All(p => p.IsActive);
    }
}

