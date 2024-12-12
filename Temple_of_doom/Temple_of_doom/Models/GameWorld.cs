﻿using Temple_of_doom.Controllers;

namespace Temple_of_doom.Models
{
    public class GameWorld
    {
        public Player Player { get; set; }
        public List<Room> Rooms { get; set; }
        public Room CurrentRoom { get; set; }
        public bool IsGameOver => Player?.Lives <= 0 || Player?.HasWon == true;

        public void MovePlayer(string direction)
        {
            Position currentPosition = Player.Position;
            Position newPosition = direction switch
            {
                "up" => new Position(currentPosition.X, currentPosition.Y - 1),
                "down" => new Position(currentPosition.X, currentPosition.Y + 1),
                "left" => new Position(currentPosition.X - 1, currentPosition.Y),
                "right" => new Position(currentPosition.X + 1, currentPosition.Y),
                _ => currentPosition // Geen beweging
            };

            if (CurrentRoom.IsPositionWalkable(newPosition))
            {
                Player.Position = newPosition;
                Console.WriteLine($"Player moved to: {newPosition.X}, {newPosition.Y}");
            }
            else
            {
                Console.WriteLine("You can't move there!");
            }
        }
    }
}