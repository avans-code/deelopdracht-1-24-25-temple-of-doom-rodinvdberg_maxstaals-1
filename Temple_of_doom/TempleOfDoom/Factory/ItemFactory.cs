﻿using TempleOfDoom.data.DTO;
using TempleOfDoom.data.Models.Items;

namespace TempleOfDoom.Factory;

public static class ItemFactory
{
    public static Item CreateItem(ItemDTO itemDTO)
    {
        return itemDTO.Type switch
        {
            "key" => new Key(itemDTO.Name),
            "SankaraStone" => new SankaraStone(itemDTO.Name),
            _ => throw new ArgumentException($"Unknown item type: {itemDTO.Type}")
        };
    }
}