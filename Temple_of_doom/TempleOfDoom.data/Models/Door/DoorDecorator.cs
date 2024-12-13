﻿namespace TempleOfDoom.data.Models.Door;

public abstract class DoorDecorator : Door
{
    protected Door door;

    protected DoorDecorator(Door door)
    {
        this.door = door;
    }
}