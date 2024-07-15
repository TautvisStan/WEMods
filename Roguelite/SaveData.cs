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
        public List<int> Opponents = new List<int>();
        public List<int> Defeated = new List<int>();

        public RogueliteSave(int character)
        {
            SelectedCharacter = character;
            active = true;
            for (int i = 1; i <= Characters.no_chars; i++)
            {
                if (i != character) Opponents.Add(i);
            }
            var rng = new System.Random();
            rng.Shuffle(Opponents);

        }
    }
}
