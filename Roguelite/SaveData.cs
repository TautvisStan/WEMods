using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace Roguelite
{
    public class RogueliteSave
    {
        public bool active { get; set; }
        public int SelectedCharacter { get; set; }
        public List<RandomMatch> matches { get; set; }
        public uint seed { get; set; }
        public int nums { get; set; }
        public int matchesCompleted { get; set; }

        public RogueliteSave(int character)
        {
            SelectedCharacter = character;
            active = true;
            matches = new();
            matchesCompleted = 0;
        }
    }
}
