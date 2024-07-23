using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Roguelite
{
    public static class MatchRules
    {
        public static (int, int)[] format =
        {
           (0, 0), //individuals
           (1, 0), //teams
           (2, 0) //tag teams
        };
        public static (int, int)[] rules = 
        { 
           (2, 40), //strict
           (1, 40), //leninet
           (0, 20) //hardcore
        };

        public static (int, int)[] falls =
        {
    //       (0, 10), //none
           (1, 15), //pins 
           (2, 60), //p & s
           (3, 15) //submissions
        };

        public static (int, int)[] aim =
        {
           (1, 75), //first fall wins 
           (2, 25), //best of 3
           (5, 0) //elimination
        };

        public static (int, int)[] stoppage =
        {
           (-1, 10), //smashes
           (0, 40), //none 
           (1, 20), //no health
           (2, 15), //10 count
           (3, 15) //blood
        };

        public static (int, int)[] countouts =
        {
           (0, 40), //none
           (1, 20), //fast 
           (2, 20), //slow
           (3, 20), //elimination
           (4, 0) //victory
        };

        public static (int, int)[] ring =
        {
           (0, 5), //none
           (1, 70), //square 
           (2, 25) //hexagon
        };

        public static (int, int)[] cage =
        {
           (0, 80), //none
           (1, 20) //yes 
        };

        public static (int, int)[] ropes =
        {
           (0, 20), //none
           (1, 60), //white 
           (-1, 15), //barbed 
           (-2, 5), //exploding 

        };

        public static (int, int)[] venue =
        {
           (0, 80), //default arenas
           (1, 20) //other locations
        };

        public static int[] arenas =
            { -2, -1, 0, 1 };

        public static int[] otherlocs =
            { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 21, 27};


        //public static RandomMatch
    }
    public class RandomMatch
    {
        public int venue { get; set; }   
        public int format { get; set; }
        public int rules { get; set; }
        public int aim { get; set; }
        public int falls { get; set; }
        public int stoppage { get; set; }
        public int countouts { get; set; }
        public int ringshape { get; set; }
        public int cage { get; set; }
        public int ropes { get; set; }
        public List<int> opponents { get; set; }
        public List<int> teammate { get; set; }
        public RandomMatch()
        {
            opponents = new();
            teammate = new();
        }
        public override string ToString()
        {
            string opps = "";
            string team = "";
            foreach (int i in opponents) opps = string.Join(" ", opps, i);
            foreach (int i in teammate) team = string.Join(" ", team, i);

            return string.Join(" ", venue, format, rules, aim, falls, stoppage, countouts, ringshape, cage, ropes, opps, team);
        }
    }
    public static class MatchGenerator
    {
        public static void RemoveCharacter(int id)
        {
            Scene_Match_Setup code = UnityEngine.GameObject.Find("Code").GetComponent<Scene_Match_Setup>();
            Debug.Log("Attempting to remove P" + id.ToString());
            if (id == 0)
            {
                for (int i = 1; i <= NJBJIIIACEP.NBBBLJDBLNM; i++)
                {
                    if (NJBJIIIACEP.OAAMGFLINOB[i].AHBNKMMMGFI > 0f)
                    {
                        id = i;
                    }
                }
            }
            if (id > 0)
            {
                Debug.Log("Deactivating P" + id.ToString());
                //    CHLPMKEGJBJ.DNNPEAOCDOG(CHLPMKEGJBJ.AKLKPBJBEBK, 1f, 1f);
                DFOGOCNBECG dfogocnbecg = NJBJIIIACEP.OAAMGFLINOB[id];
                dfogocnbecg.AHBNKMMMGFI = -1f;
                dfogocnbecg.PCNHIIPBNEK[0].SetActive(false);
                if (dfogocnbecg.IFOCOECLBAF != null)
                {
                    dfogocnbecg.IFOCOECLBAF.SetActive(false);
                }
                FFCEGMEAIBP.NMMABDGIJNC[id] = -id;
                if (code.castFoc == id)
                {
                    code.castFoc = 0;
                }
                if (code.moveFoc == id)
                {
                    code.moveFoc = 0;
                }
                for (int j = 0; j <= HKJOAJOKOIJ.NGCNKGDDKGF; j++)
                {
                    if (HKJOAJOKOIJ.NAADDLFFIHG[j].AHBNKMMMGFI > 0 && HKJOAJOKOIJ.NAADDLFFIHG[j].FOAPDJMIFGP == id)
                    {
                        HKJOAJOKOIJ.NAADDLFFIHG[j].FOAPDJMIFGP = 0;
                    }
                }
            }
        }
        public static int AddCharacter(int id, int team)
        {
            Scene_Match_Setup code = UnityEngine.GameObject.Find("Code").GetComponent<Scene_Match_Setup>();
            code.n = 0;
            code.v = 1;
            while (code.v <= NJBJIIIACEP.NBBBLJDBLNM)
            {
                if (NJBJIIIACEP.OAAMGFLINOB[code.v].AHBNKMMMGFI <= 0f)
                {
                    Debug.Log("Overwriting slot " + code.v.ToString() + " to add new");
                    code.n = code.v;
                    break;
                }
                code.v++;
            }
            if (code.n == 0 && FFCEGMEAIBP.EHIDHAPMAKG < NAEEIFNFBBO.ILLMCDIFFON && FFCEGMEAIBP.EHIDHAPMAKG < NJBJIIIACEP.KLDJKHPCDHM)
            {
                code.n = FFCEGMEAIBP.EHIDHAPMAKG + 1;
                Debug.Log("Increasing cast to " + FFCEGMEAIBP.EHIDHAPMAKG.ToString() + " to add new");
            }
            //    CHLPMKEGJBJ.DNNPEAOCDOG(CHLPMKEGJBJ.DOIEFBGOCPA, 1f, 1f);
            //   CHLPMKEGJBJ.DNNPEAOCDOG(CHLPMKEGJBJ.JMBMELDCIDA[1], 1f, 0.75f);
            if (FFCEGMEAIBP.EHIDHAPMAKG < 1)
            {
                FFCEGMEAIBP.EHIDHAPMAKG = 1;
                Debug.Log("Increasing castSize from 0 to 1");
            }
            if (code.n > FFCEGMEAIBP.EHIDHAPMAKG)
            {
                FFCEGMEAIBP.EHIDHAPMAKG = code.n;
                Debug.Log("Increasing castSize to " + code.n.ToString());
            }
            int num = id;
            int num2 = 1;
            int num3 = team;
            FFCEGMEAIBP.NMMABDGIJNC[code.n] = num;
            FFCEGMEAIBP.DJCDPNPLICD[code.n] = num2;
            FFCEGMEAIBP.EKKIPMFPMEE[code.n] = num3;
            int num4 = 0;
            int num5;
            do
            {
                FFCEGMEAIBP.AJMAFHIBCGJ[code.n] = (float)UnityEngine.Random.Range(-20, 20) * World.ringSize;
                if (num3 == 1)
                {
                    FFCEGMEAIBP.AJMAFHIBCGJ[code.n] = (float)UnityEngine.Random.Range(-20, -5) * World.ringSize;
                }
                if (num3 == 2)
                {
                    FFCEGMEAIBP.AJMAFHIBCGJ[code.n] = (float)UnityEngine.Random.Range(5, 20) * World.ringSize;
                }
                FFCEGMEAIBP.MHHLHMDOFBP[code.n] = (float)UnityEngine.Random.Range(-20, 20) * World.ringSize;
                num4++;
                num5 = 1;
                if (Mathf.Abs(FFCEGMEAIBP.AJMAFHIBCGJ[code.n]) < 5f && FFCEGMEAIBP.MHHLHMDOFBP[code.n] > 0f && NAEEIFNFBBO.BNCCMMLOIML > 0)
                {
                    if (num2 != 3)
                    {
                        num5 = 0;
                    }
                }
                else if (num2 == 3)
                {
                    num5 = 0;
                }
                for (int i = 1; i <= NJBJIIIACEP.NBBBLJDBLNM; i++)
                {
                    if (NJBJIIIACEP.OAAMGFLINOB[i].AHBNKMMMGFI > 0f && NAEEIFNFBBO.FHPCDHIGILG(FFCEGMEAIBP.AJMAFHIBCGJ[code.n], FFCEGMEAIBP.MHHLHMDOFBP[code.n], NJBJIIIACEP.OAAMGFLINOB[i].NJDGEELLAKG, NJBJIIIACEP.OAAMGFLINOB[i].BMFDFFLPBOJ) < 5f)
                    {
                        num5 = 0;
                    }
                }
            }
            while (num4 < 100 && num5 == 0);
            Debug.Log("Assigning character " + FFCEGMEAIBP.NMMABDGIJNC[code.n].ToString() + " to slot " + code.n.ToString());
            if (code.n <= NJBJIIIACEP.NBBBLJDBLNM)
            {
                NJBJIIIACEP.IIACAIINEKD(code.n, id, 1);
            }
            else
            {
                NJBJIIIACEP.DFLLBNMHHIH(id, 1);
            }
            return code.n;

        }

        public static void RemoveOpponents()
        {
            for (int i = NJBJIIIACEP.NBBBLJDBLNM; i > 0; i--)
            {
                RemoveCharacter(i);
            }
        }

        public static void SetupMatchRules(RandomMatch match)
        {
            FFCEGMEAIBP.JELMGJMKKEK(2);
            FFCEGMEAIBP.NLPNEAAANMO(0);
            World.libraryFoc = Array.IndexOf(World.library, match.venue);
            World.location = match.venue;
            Scene_Match_Setup code = UnityEngine.GameObject.Find("Code").GetComponent<Scene_Match_Setup>();
            int num = 0;
            if (World.location <= 1 && code.oldArena <= 1)
            {
                num = 1;
            }
            if (World.location == -2 || code.oldArena == -2)
            {
                num = 0;
            }
            if (num > 0)
            {
                World.KELMLPGMAPC(1);
            }
            else
            {
                World.ICGNAJFLAHL(1);
            }
            code.oldArena = match.venue;
            FFCEGMEAIBP.OLJFOJOLLOM = match.format;
            FFCEGMEAIBP.CADLONHABMC = match.rules;
            FFCEGMEAIBP.BPJFLJPKKJK = match.aim;
            FFCEGMEAIBP.LGHMLHICAFL = match.falls;
            FFCEGMEAIBP.DOLNEDHNKMM = match.stoppage;
            FFCEGMEAIBP.GDKCEGBINCM = match.countouts;
            World.ringShape = match.ringshape;
            World.arenaCage = match.cage;
            World.ringRopes = match.ropes;
            World.ICGNAJFLAHL(1); //location
            World.DBKOAJKLBIF(1); //ring
            World.crowdSize = 1;
            if (World.ringShape > 0)
            {
                if (World.MNCIAPLCFDM() == 0f)
                {
                    World.ringShape = -World.ringShape;
                    GIMNNPMAKNJ.NALPMNNGKAE();
                }
                else
                {
                    World.ringSize = World.MNCIAPLCFDM();
                }
            }
            FFCEGMEAIBP.NBAFIEALMHN = 0;
            FFCEGMEAIBP.JMBGHDFADHN = -1;
            //fog
            World.EFNENMKELIA();
            World.CJMBGNBDOFI(1);

            if(match.rules <= 1)
            {
                JFLEBEBCGFA.CCFGHKNBOEL *= 2;
                JFLEBEBCGFA.LGPEMCGEEPN = 0;
            }

            if (match.stoppage == -1)
            {
                HAPFAOIMGOL.CCFGHKNBOEL *= 2;
            }

        }
        public static void SetupParticipants(int player, RandomMatch match)
        {
            RemoveOpponents();
            AddCharacter(player, 1);
            foreach (int opponent in match.opponents)
            {
                if (opponent > Characters.no_chars)
                {
                    Plugin.Log.LogWarning($"WARNING! Opponent id {opponent} is out of range! They will be replaced by a random character!");
                    int newopp = UnityEngine.Random.Range(1, Characters.no_chars+1);
                    AddCharacter(newopp, 2);
                }
                else
                {
                    AddCharacter(opponent, 2);
                }
            }
            foreach (int teammate in match.teammate)
            {
                if (teammate > Characters.no_chars)
                {
                    Plugin.Log.LogWarning($"WARNING! Teammate id {teammate} is out of range! They will be replaced by a random character!");
                    int newteam = UnityEngine.Random.Range(1, Characters.no_chars+1);
                    AddCharacter(newteam, 1);
                }
                else
                {
                    AddCharacter(teammate, 1);
                }
            }
        }
        public static List<int> RandomizeOpponents(int playerchar, Randomizer rng)
        {
            List<int> Opponents = new();
            for (int i = 1; i <= Characters.no_chars; i++)
            {
                if (i != playerchar) Opponents.Add(i);
            }
            rng.Shuffle(Opponents);
            return Opponents;
        }
        public static List<RandomMatch> GenerateRandomMatches(List<int> opponentPool, Randomizer rng)
        {
            List<RandomMatch> matches = new();
            List<int> teammatePool = new();
            int i = 1;
            while (opponentPool.Count > 0) 
            {
                RandomMatch match = new();
                match.venue = GenerateVenue(rng);
                match.format = GenerateFormat(rng, i);
                match.ringshape = GenerateRingshape(rng, match.venue);
                match.cage = GenerateCage(rng, match.ringshape);
                match.rules = GenerateRules(rng, match.ringshape);
                match.aim = GenerateAim(rng, i);
                match.ropes = GenerateRopes(rng, match.ringshape);
                match.falls = GenerateFalls(rng);
                match.stoppage = GenerateStoppage(rng, match.rules, match.cage);
                match.countouts = GenerateCountouts(rng, match.rules, match.cage, match.ringshape);

                if(i % 5 == 0 && i % 10 != 0)
                {
                    int index = rng.Range(1, teammatePool.Count);
                    match.teammate.Add(teammatePool[index]);
                    teammatePool.RemoveAt(index);
                }
                match.opponents.Add(opponentPool.First());
                teammatePool.Add(opponentPool.First());
                opponentPool.Remove(opponentPool.First());

                if(i%5 == 0 && opponentPool.Count != 0)
                {
                    match.opponents.Add(opponentPool.First());
                    teammatePool.Add(opponentPool.First());
                    opponentPool.Remove(opponentPool.First());
                }
                matches.Add(match);
                i++;
            }


            return matches;
        }
        public static int GenerateVenue(Randomizer rng)
        {
            ProportionalRandomSelector<int> randomSelectorType = new(rng);
            foreach ((int i, int proc) in MatchRules.venue)
            {
                randomSelectorType.AddPercentageItem(i, proc);
            }
            int venueType = randomSelectorType.SelectItem();
            if(venueType == 0) 
            {
                ProportionalRandomSelector<int> randomSelectorArena = new(rng);
                foreach (int i in MatchRules.arenas)
                {
                    randomSelectorArena.AddPercentageItem(i, 1);
                }
                return randomSelectorArena.SelectItem();
            }
            else
            {
                ProportionalRandomSelector<int> randomSelectorOtherLocs = new(rng);
                foreach (int i in MatchRules.otherlocs)
                {
                    randomSelectorOtherLocs.AddPercentageItem(i, 1);
                }
                return randomSelectorOtherLocs.SelectItem();
            }
        }
        public static int GenerateFormat(Randomizer rng, int i)
        {
            if (i % 20 == 0)
            {
                return 1; // teams
            }
            if(i % 10 == 0)
            {
                return 2; //tag teams
            }
            if(i % 5 == 0)
            {
                ProportionalRandomSelector<int> randomSelectorFormat = new(rng);
                randomSelectorFormat.AddPercentageItem(1, 50);
                randomSelectorFormat.AddPercentageItem(2, 50);
                return randomSelectorFormat.SelectItem();
            }
            return 0; //individuals
        }
        public static int GenerateAim(Randomizer rng, int i)
        {
            ProportionalRandomSelector<int> randomSelectorAim = new(rng);
            randomSelectorAim.AddPercentageItem(1, 75);
            randomSelectorAim.AddPercentageItem(2, 25);

            if (i % 5 == 0)
            {
                randomSelectorAim.AddPercentageItem(5, 50);
            }
            return randomSelectorAim.SelectItem();
        }
        public static int GenerateRingshape(Randomizer rng, int venue)
        {
            int[] noRingVenues = { 4, 6, 7, 9, 11, 13, 15, 21, 27 };
            if (Array.Exists(noRingVenues, element => element == venue))
            {
                return 0; //no ring
            }
            else
            {
                ProportionalRandomSelector<int> randomSelectorRing = new(rng);
                foreach ((int i, int proc) in MatchRules.ring)
                {
                    randomSelectorRing.AddPercentageItem(i, proc);
                }
                int result = randomSelectorRing.SelectItem();
                return result;
            }
        }
        public static int GenerateCage(Randomizer rng, int ring)
        {
            if (ring == 0)
            {
                return 0; //no cage
            }
            else
            {
                ProportionalRandomSelector<int> randomSelectorCage = new(rng);
                foreach ((int i, int proc) in MatchRules.cage)
                {
                    randomSelectorCage.AddPercentageItem(i, proc);
                }
                return randomSelectorCage.SelectItem();
            }
        }
        public static int GenerateRopes(Randomizer rng, int ring)
        {
            if (ring == 0)
            {
                return 0; //no ropes
            }
            else
            {
                ProportionalRandomSelector<int> randomSelectorRope = new(rng);
                foreach ((int i, int proc) in MatchRules.ropes)
                {
                    randomSelectorRope.AddPercentageItem(i, proc);
                }
                return randomSelectorRope.SelectItem();
            }
        }
        public static int GenerateRules(Randomizer rng, int ring)
        {
            if (ring == 0)
            {
                return 0; //hardcore
            }
            else
            {
                ProportionalRandomSelector<int> randomSelectorRules = new(rng);
                foreach ((int i, int proc) in MatchRules.rules)
                {
                    randomSelectorRules.AddPercentageItem(i, proc);
                }
                return randomSelectorRules.SelectItem();
            }
        }
        public static int GenerateFalls(Randomizer rng)
        {
            ProportionalRandomSelector<int> randomSelectorFalls = new(rng);
            foreach ((int i, int proc) in MatchRules.falls)
            {
                randomSelectorFalls.AddPercentageItem(i, proc);
            }
            return randomSelectorFalls.SelectItem();
        }
        public static int GenerateStoppage(Randomizer rng, int rules, int cage)
        {
            ProportionalRandomSelector<int> randomSelectorStoppage = new(rng);
            randomSelectorStoppage.AddPercentageItem(0, 50); // none
            randomSelectorStoppage.AddPercentageItem(1, 20); // no health
            randomSelectorStoppage.AddPercentageItem(2, 20); // 10 count
            if(rules == 0) // hardcore
            {
                randomSelectorStoppage.AddPercentageItem(3, 10); // none
            }
            if(cage == 0)  //no cage
            {
                randomSelectorStoppage.AddPercentageItem(-1, 10); // smashes
            }
            return randomSelectorStoppage.SelectItem();

        }
        public static int GenerateCountouts(Randomizer rng, int rules, int cage, int ringshape)
        {
            ProportionalRandomSelector<int> randomSelectorCountouts = new(rng);
            randomSelectorCountouts.AddPercentageItem(0, 40); // none
            if (ringshape != 0 && rules != 0)
            {
                randomSelectorCountouts.AddPercentageItem(1, 20); // slow
                randomSelectorCountouts.AddPercentageItem(2, 20); // fast
            }
            if (ringshape != 0)
            {
                randomSelectorCountouts.AddPercentageItem(3, 20); // elimination
            }
            if (cage != 0)
            {
                randomSelectorCountouts.AddPercentageItem(4, 75); // victory
            }
            return randomSelectorCountouts.SelectItem();

        }


    }
    public class ProportionalRandomSelector<T>
    {

        private readonly Dictionary<T, int> percentageItemsDict;
        private Randomizer random;
        public ProportionalRandomSelector(Randomizer rnd)
        {
            percentageItemsDict = new();
            random = rnd;
        }

        public void AddPercentageItem(T item, int percentage) => percentageItemsDict.Add(item, percentage);

        public T SelectItem()
        {

            // Calculate the summa of all portions.
            int poolSize = 0;
            foreach (int i in percentageItemsDict.Values)
            {
                poolSize += i;
            }

            // Get a random integer from 1 to PoolSize.
            int randomNumber = random.Range(1, poolSize);

            // Detect the item, which corresponds to current random number.
            int accumulatedProbability = 0;
            foreach (KeyValuePair<T, int> pair in percentageItemsDict)
            {
                accumulatedProbability += pair.Value;
                if (randomNumber <= accumulatedProbability)
                    return pair.Key;
            }

            return default;  // this code will never come while you use this programm right :)

        }

    }
}


//strict = no blood stoppage
//no ring = no cage
//cage = countout victory instead of elim
//every 5th match = team format;
//every 10 match = handicap tag;
//every 20 match = handicap team;