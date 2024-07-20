// use implementation at https://gist.github.com/macklinb/a00be6b616cbf20fa95e4227575fe50b ?


using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Roguelite
{
    public class Randomizer
    {
        public System.Random rng { get; set; }
        public int seed { get; set; }
        public int nums { get; set; }
        public Randomizer()
        {
            seed = (int)DateTime.Now.Ticks & 0xFFFFFFF;
            rng = new System.Random(seed);
            Plugin.Log.LogInfo("Using default random seed: " + seed);
            nums = 0;
        }
        public Randomizer(string customSeed)
        {
            seed = GetSeedFromString(customSeed);
            rng = new System.Random(seed);
            Plugin.Log.LogInfo("Using custom seed \"" + customSeed+ "\", corresponds to default random seed: " + seed);
            nums = 0;
        }
        public void CatchUp(int nums)
        {
            for (int i = 0; i < nums; i++)
            {
                nums++;
                rng.Next(1);
            }
        }
        public int Next(int max)
        {
            nums++;
            return rng.Next(max);
        }
        public int Range(int min, int max)
        {
            nums++;
            return rng.Next(min, max);
        }


        public static int GetSeedFromString(string text)
        {
            int seed;
            if (int.TryParse(text, out seed))
            {
                return seed;
            }
            MD5 md5Hasher = MD5.Create();
            var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(text));
            var ivalue = BitConverter.ToInt32(hashed, 0);
            return ivalue;
        }
        public void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                int k = rng.Next(n--);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }

}
