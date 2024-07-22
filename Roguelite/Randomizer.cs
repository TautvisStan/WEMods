// use implementation at https://gist.github.com/macklinb/a00be6b616cbf20fa95e4227575fe50b ?


using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Roguelite
{
    public class Randomizer
    {
        public XORShift128 rng { get; set; }
        public uint seed { get; set; }
        public int nums { get { return rng.nums; } set { rng.nums = value; } }
        public Randomizer()
        {
            seed = (uint)DateTime.Now.Ticks & 0xFFFFFFFF;
            rng = new XORShift128(seed);
            Plugin.Log.LogInfo("Using default random seed: " + seed);
        }
        public Randomizer(string customSeed)
        {
            seed = GetSeedFromString(customSeed);
            rng = new XORShift128(seed);
            Plugin.Log.LogInfo("Using custom seed \"" + customSeed+ "\", corresponds to default random seed: " + seed);
        }
        public void CatchUp(int nums)
        {
            for (int i = 0; i < nums; i++)
            {
                rng.XORShift();
            }
        }
     /*   public int Next(int max)
        {
            nums++;
            return rng.Next(max);
        }*/
        public int Range(int min, int max)
        {
            return rng.NextIntRange(min, max);
        }


        public static uint GetSeedFromString(string text)
        {
            uint seed;
            if (uint.TryParse(text, out seed))
            {
                return seed;
            }
            MD5 md5Hasher = MD5.Create();
            var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(text));
            var ivalue = BitConverter.ToUInt32(hashed, 0);
            return ivalue;
        }
        public void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                int k = rng.NextIntRange(0, n--);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }

}
