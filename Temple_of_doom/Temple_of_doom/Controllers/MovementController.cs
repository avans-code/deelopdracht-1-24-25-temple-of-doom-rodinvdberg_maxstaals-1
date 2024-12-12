﻿using Temple_of_doom.Models;
public class MovementController
{
    private GameWorld _gameWorld;

    public MovementController(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public void HandleInput(string command)
    {
        if (string.IsNullOrEmpty(command)) return;

        Position currentPosition = _gameWorld.Player.Position;
        Position newPosition = currentPosition;

        switch (command)
        {
            case "up":
                newPosition = new Position(currentPosition.X, currentPosition.Y - 1);
                break;
            case "down":
                newPosition = new Position(currentPosition.X, currentPosition.Y + 1);
                break;
            case "left":
                newPosition = new Position(currentPosition.X - 1, currentPosition.Y);
                break;
            case "right":
                newPosition = new Position(currentPosition.X + 1, currentPosition.Y);
                break;
            case "quit":
                Console.WriteLine("Exiting game.");
                Environment.Exit(0);
                break;
        }

        // Check if the new position is walkable
        if (_gameWorld.CurrentRoom.IsPositionWalkable(newPosition))
        {
            _gameWorld.Player.Position = newPosition;
            Console.WriteLine($"Player moved to: {newPosition.X}, {newPosition.Y}");
        }
        else
        {
            Console.WriteLine("You can't move there!");
        }
    }
}