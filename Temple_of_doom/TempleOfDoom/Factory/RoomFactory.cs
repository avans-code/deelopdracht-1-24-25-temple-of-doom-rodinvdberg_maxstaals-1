﻿using TempleOfDoom.data.DTO;
using TempleOfDoom.data.Models.Door;
using TempleOfDoom.data.Models.Items;
using TempleOfDoom.data.Models.Map;

namespace TempleOfDoom.Factory;

public static class RoomFactory
{
    public static List<Room> CreateRooms(List<RoomDto> roomsData, List<ConnectionDto> connectionData)
    {
        var rooms = new List<Room>();

        foreach (var roomData in roomsData)
        {
            var room = new Room(
                id: roomData.Id,
                width: roomData.Width,
                height: roomData.Height,
                items: new List<Item>(),
                doors: new List<Door>()
            );

            rooms.Add(room);
        }

        ConnectionFactory.CreateRoomDoors(rooms, connectionData);

        foreach (var room in rooms)
        {
            room.InitializeRoomLayout();
        }

        return rooms;
    }
}
