﻿using TempleOfDoom.data.Enums;
using TempleOfDoom.data.Models.Door;
using TempleOfDoom.data.Models.Items;

namespace TempleOfDoom.data.Models.Map;

public class Room(int id, int width, int height, List<Item> items, List<Door.Door> doors)
{
    public int Id { get; } = id;
    public List<Item> Items { get; set; } = items;
    public List<Door.Door> Doors { get; } = doors;
    public int Width { get; } = width;
    public int Height { get; } = height;
    public char[,] Layout { get; } = new char[height, width];

    public bool IsPositionWalkable(Position position)
    {
        if (Layout == null)
            throw new NullReferenceException("Room layout is not initialized.");

        if (position.Y < 0 || position.Y >= Layout.GetLength(0) ||
            position.X < 0 || position.X >= Layout.GetLength(1))
            return false;

        return Layout[position.Y, position.X] != '#';
    }

    public void InitializeRoomLayout()
    {
        // Stap 1: Vul alle posities met lege ruimte
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                Layout[y, x] = ' ';
            }
        }

        // Stap 2: Voeg muren toe langs de randen
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (y == 0 || y == Height - 1 || x == 0 || x == Width - 1)
                {
                    Layout[y, x] = (Layout[y, x] == ' ' || Layout[y, x] == '\0') ? '#' : Layout[y, x];
                }
            }
        }

        // Stap 3: Voeg deuren toe aan de lay-out
        foreach (var door in Doors)
        {
            if (door.Position.X >= 0 && door.Position.X < Width &&
                door.Position.Y >= 0 && door.Position.Y < Height)
            {
                var doorSymbol = door switch
                {
                    SimpleDoor => '|',
                    ColoredDoor => '=',
                    ToggleDoor => '_',
                    ClosingGate => '∩',
                    OpenOnOddDoor => '?',
                    OpenOnStonesDoor => '*',
                    _ => '?'
                };

                Layout[door.Position.Y, door.Position.X] = doorSymbol;
            }
        }
        foreach (var item in Items)
        {
            if (item.X >= 0 && item.X < Width && item.Y >= 0 && item.Y < Height)
            {
                var itemSymbol = item switch
                {
                    Item keyItem when keyItem.Type == "key" => 'K',
                    Item sankaraStoneItem when sankaraStoneItem.Type == "sankara stone" => 'S',
                    Item boobytrapItem when boobytrapItem.Type == "boobytrap" => 'B',
                    Item disappearingBoobytrapItem when disappearingBoobytrapItem.Type == "disappearing boobytrap" => 'D',
                    Item pressurePlateItem when pressurePlateItem.Type == "pressure plate" => 'T',
                    _ => ' '  // Default symbol for unknown item types
                };

                Layout[item.Y, item.X] = itemSymbol;
            }
        }
        //Console.WriteLine($"Room ID={Id} Layout:");
        //for (var y = 0; y < Height; y++)
        //{
        //    for (var x = 0; x < Width; x++)
        //    {
        //        Console.Write(Layout[y, x]);
        //    }
        //    Console.WriteLine();
        //}
    }

    public void HandlePlayerInteraction(Player player)
    {
        Console.WriteLine("test");

        if (Layout == null)
            throw new NullReferenceException("Room layout is not initialized.");

        // Controleer of de speler op een geldige positie is in de huidige kamer
        if (!player.CurrentRoom.IsPositionWalkable(player.Position))
        {
            Console.WriteLine("Player position is not walkable, adjusting...");
            // Pas de positie aan (bijvoorbeeld terug naar het midden van de kamer)
            player.Position = new Position(player.CurrentRoom.Width / 2, player.CurrentRoom.Height / 2);
            Console.WriteLine($"New position: ({player.Position.X}, {player.Position.Y})");
        }

        var currentTile = Layout[player.Position.Y, player.Position.X];

        switch (currentTile)
        {
            case 'K': // Key
                player.Inventory.AddItem("Key");
                Layout[player.Position.Y, player.Position.X] = '.'; // Verwijder de key uit de kamer

                // Verwijder het item uit de Items-lijst van de kamer
                var keyItem = player.CurrentRoom.Items.FirstOrDefault(i => i.X == player.Position.X && i.Y == player.Position.Y);
                if (keyItem != null)
                {
                    player.CurrentRoom.Items.Remove(keyItem);
                    Console.WriteLine("Key removed from the room.");
                }
                else
                {
                    Console.WriteLine("Key item not found in the room.");
                }

                Console.WriteLine("You picked up a Key!");
                break;

            case 'S': // Sankara Stone
                Console.WriteLine("You found a Sankara Stone!");
                player.Inventory.AddItem("sankara stone"); // Voeg toe aan inventaris
                Layout[player.Position.Y, player.Position.X] = '.'; // Verwijder de steen uit de kamer

                // Verwijder het item uit de Items-lijst van de kamer
                var sankaraStoneItem = player.CurrentRoom.Items.FirstOrDefault(i => i.X == player.Position.X && i.Y == player.Position.Y);
                if (sankaraStoneItem != null)
                {
                    player.CurrentRoom.Items.Remove(sankaraStoneItem);
                    Console.WriteLine("Sankara Stone removed from the room.");
                }
                else
                {
                    Console.WriteLine("Sankara Stone item not found in the room.");
                }

                Console.WriteLine("You picked up a Sankara Stone!");

                // Toon de huidige hoeveelheid Sankara Stones in de inventaris
                int sankaraStoneCount = player.Inventory.GetItemCount("Sankara Stone");
                Console.WriteLine("You now have " + sankaraStoneCount + " Sankara Stones.");

                // Controleer of de speler heeft gewonnen (bijvoorbeeld: 5 Sankara Stones verzameld)
                if (sankaraStoneCount == 5)
                {
                    player.HasWon = true;
                    Console.WriteLine("You have collected all 5 Sankara Stones! You win!");
                }
                player.CheckWinCondition();
                break;

            case 'B': // Boobytrap
                if (currentTile == 'B') // Alleen verwijderen als het een "disappearing" boobytrap is
                {
                    // Controleer of het een "disappearing" boobytrap is en verwijder het alleen dan
                    var boobytrapItem = player.CurrentRoom.Items.FirstOrDefault(i => i.X == player.Position.X && i.Y == player.Position.Y);
                    if (boobytrapItem != null && boobytrapItem is DisappearingBoobytrap)
                    {
                        player.Lives--;
                        player.CurrentRoom.Items.Remove(boobytrapItem); // Verwijder de boobytrap
                        Console.WriteLine("Disappearing boobytrap triggered! Player loses 1 life.");
                    }
                    else if (boobytrapItem != null)
                    {
                        // Als het een normale boobytrap is, wordt deze niet verwijderd
                        player.Lives--;
                        Console.WriteLine("Boobytrap triggered! Player loses 1 life.");
                    }
                }
                break;

            default:
                // Console.WriteLine("Nothing to interact with here.");
                break;
        }

        // Debugging: Toon de inhoud van de kamer na de interactie
        Console.Clear();
        InitializeRoomLayout();

        Console.WriteLine("Current items in the room after interaction:");
        foreach (var item in player.CurrentRoom.Items)
        {
            Console.WriteLine($"Item: {item.Type} at ({item.X}, {item.Y})");
        }
    }
}