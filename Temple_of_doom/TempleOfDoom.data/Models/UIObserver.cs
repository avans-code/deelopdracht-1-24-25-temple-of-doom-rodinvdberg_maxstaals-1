﻿namespace TempleOfDoom.data.Models;

public class UIObserver
{
    public void Update(string message)
    {
        Console.WriteLine($"UI Update: {message}");
    }
}