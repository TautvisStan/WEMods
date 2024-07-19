using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;


namespace Roguelite
{
    public class RogueliteSave
    {
        public bool active;
        public int SelectedCharacter;
        public List<RandomMatch> matches = new();

        public RogueliteSave(int character)
        {
            SelectedCharacter = character;
            active = true;
        }
    }
}
