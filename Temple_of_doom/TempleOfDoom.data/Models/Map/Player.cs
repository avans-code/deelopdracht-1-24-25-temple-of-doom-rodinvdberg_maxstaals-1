﻿namespace TempleOfDoom.data.Models.Map;

public class Player
{
    public int Lives { get; set; }
    public bool HasWon { get; set; }
    public Position Position { get; set; }
    public Inventory Inventory { get; set; }
    public int StartingRoomId { get; set; }
    public Room CurrentRoom { get; set; }

    public Player(int lives, Position position)
    {
        Lives = lives;
        HasWon = false;
        Inventory = new Inventory();
        Position = position;
    }

    public bool HasKey(string keyColor)
    {
        return Inventory.HasItem(keyColor);
    }

    public Position GetPlayerStartPosition()
    {
        return Position;
    }

    public void Move(string command, Room currentRoom, List<Room> rooms)
    {
        if (currentRoom == null || rooms == null)
        {
            Console.WriteLine("Error: Room or room list is not initialized.");
            return;
        }

        // Controleer op geldige input
        if (command != "up" && command != "down" && command != "left" && command != "right")
        {
            Console.WriteLine("Invalid command! Use 'up', 'down', 'left', or 'right'.");
            return;
        }

        // Bereken de nieuwe positie
        var newPosition = new Position
        (
            command switch
            {
                "up" => Position.X,
                "down" => Position.X,
                "left" => Position.X - 1,
                "right" => Position.X + 1,
                _ => Position.X
            },
            command switch
            {
                "up" => Position.Y - 1,
                "down" => Position.Y + 1,
                "left" => Position.Y,
                "right" => Position.Y,
                _ => Position.Y
            }
        );

        // Controleer of de nieuwe positie binnen de kamergrenzen ligt
        if (currentRoom.IsPositionWalkable(newPosition))
        {
            Position = newPosition;
        }


        foreach (var door in currentRoom.Doors)
        {
            if (door.Position.X == Position.X && door.Position.Y == Position.Y)
            {
                Console.WriteLine($"You used the door to Room ID={door.TargetRoomId}");

                // Teleporteer naar de verbonden kamer
                var targetRoom = rooms.FirstOrDefault(r => r.Id == door.TargetRoomId);
                if (targetRoom == null)
                {
                    Console.WriteLine($"Error: TargetRoomId={door.TargetRoomId} not found!");
                    return;
                }

                CurrentRoom = targetRoom;

                // Stel de nieuwe positie in
                Position = door.Direction switch
                {
                    Direction.NORTH => new Position(door.Position.X, CurrentRoom.Height - 1),
                    Direction.SOUTH => new Position(door.Position.X, 0),
                    Direction.WEST => new Position(CurrentRoom.Width - 1, door.Position.Y),
                    Direction.EAST => new Position(0, door.Position.Y),
                    _ => throw new Exception("Invalid door direction")
                };

                Console.WriteLine($"Teleported to Room ID={CurrentRoom.Id} at Position=({Position.X}, {Position.Y})");

                // Controleer of de nieuwe positie toegankelijk is
                if (!CurrentRoom.IsPositionWalkable(Position))
                {
                    Console.WriteLine($"current room: {CurrentRoom.Width}, {CurrentRoom.Height}");
                    Console.WriteLine($"wanted position: {Position.X}, {Position.Y}");
                    Console.WriteLine("Teleportation position is not walkable, fallback required.");
                    Position = new Position(CurrentRoom.Width / 2, CurrentRoom.Height / 2);
                    Console.WriteLine($"Fallback to position: {Position.X}, {Position.Y}");
                }
                break;
            }
        }
    }
}