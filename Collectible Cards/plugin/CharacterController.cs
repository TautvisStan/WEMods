﻿using System;
using UnityEngine;

namespace CollectibleCards2
{
    internal class CharacterController
    {
        public static DFOGOCNBECG Wrestler { get; set; } = null;
        public static GameObject ParentObj { get; set; } = null;
        public static float Size { get; set; } = 1f;
        public static void SetupCharacter(int id, int costume, string[] setup)
        {
            ParentObj = new("Parent");
            int GOOKPABIPBC = id;
            if (NJBJIIIACEP.OAAMGFLINOB == null) NJBJIIIACEP.PIMGMPBCODM(1);
            NJBJIIIACEP.NBBBLJDBLNM = 1;
            Wrestler = NJBJIIIACEP.OAAMGFLINOB[1];
            Wrestler.GOOKPABIPBC = GOOKPABIPBC;
            Wrestler.EMDMDLNJFKP = Characters.c[Wrestler.GOOKPABIPBC];
            Wrestler.OEGJEBDBGJA = Wrestler.EMDMDLNJFKP.costume[costume];
            Wrestler.PLFGKLGCOMD = 1;
            if (Wrestler.PCNHIIPBNEK[0] != null)
            {
                UnityEngine.Object.Destroy(Wrestler.PCNHIIPBNEK[0]);
            }
            Wrestler.DDKAGOBJGBC(0);
            Wrestler.ABHDOPBDDPB();
            Wrestler.PCNHIIPBNEK[0].transform.eulerAngles = new Vector3(0f, 0f, 0f);
            Wrestler.PCNHIIPBNEK[0].transform.position = new Vector3(0, 0, 0);
            Wrestler.PCNHIIPBNEK[0].transform.localScale = new Vector3(1, 1, 1);
            Size = 1;
            foreach (string line in setup)
            {
                if (line.Trim().Length == 0)
                {
                    continue;
                }
                if (line.ToLower().StartsWith("charsize:"))
                {
                    Size = float.Parse(line.Substring(9).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("charposx:"))
                {
                    float PosX = float.Parse(line.Substring(9).Trim());
                    Wrestler.PCNHIIPBNEK[0].transform.position = new Vector3(PosX, Wrestler.PCNHIIPBNEK[0].transform.position.y, 0);
                    continue;
                }
                if (line.ToLower().StartsWith("charposy:"))
                {
                    float PosY = float.Parse(line.Substring(9).Trim());
                    Wrestler.PCNHIIPBNEK[0].transform.position = new Vector3(Wrestler.PCNHIIPBNEK[0].transform.position.x, PosY, 0);
                    continue;
                }
                if (line.ToLower().StartsWith("charrotx:"))
                {
                    float RotX = float.Parse(line.Substring(9).Trim());
                    Wrestler.PCNHIIPBNEK[0].transform.eulerAngles = new Vector3(RotX, Wrestler.PCNHIIPBNEK[0].transform.eulerAngles.y, Wrestler.PCNHIIPBNEK[0].transform.eulerAngles.z);
                    continue;
                }
                if (line.ToLower().StartsWith("charroty:"))
                {
                    float RotY = float.Parse(line.Substring(9).Trim());
                    Wrestler.PCNHIIPBNEK[0].transform.eulerAngles = new Vector3(Wrestler.PCNHIIPBNEK[0].transform.eulerAngles.x, RotY, Wrestler.PCNHIIPBNEK[0].transform.eulerAngles.z);
                    continue;
                }
                if (line.ToLower().StartsWith("charrotz:"))
                {
                    float RotZ = float.Parse(line.Substring(9).Trim());
                    Wrestler.PCNHIIPBNEK[0].transform.eulerAngles = new Vector3(Wrestler.PCNHIIPBNEK[0].transform.eulerAngles.x, Wrestler.PCNHIIPBNEK[0].transform.eulerAngles.y, RotZ);
                    continue;
                }
            }
            Wrestler.KOLHFFPPCEE((float)(50 - 100 * Wrestler.EMDMDLNJFKP.heel));
            int num = NAEEIFNFBBO.OMOADEKHHHO(MBLIOKEDHHB.ABJFEMNCIMI);
            Wrestler.FEOFDJFFNMN = MBLIOKEDHHB.LHFJJPOPIAA[num].PFDGHMKKHOF;
            Wrestler.LMALJJFEHGH = MBLIOKEDHHB.LHFJJPOPIAA[num].EJPKJOFMIAI[NAEEIFNFBBO.PMEEFNOLAGF(0, MBLIOKEDHHB.LHFJJPOPIAA[num].EJPKJOFMIAI.Length - 1)];
            Wrestler.FJHHJGONAFO(Wrestler.FEOFDJFFNMN, Wrestler.LMALJJFEHGH);
            Wrestler.FEACEIIIAHK();

        }
        public static void SetupBelts()
        {
            //belts
            float realHeight = Characters.c[Wrestler.GOOKPABIPBC].height;
            try
            {
                Characters.c[Wrestler.GOOKPABIPBC].height = 1;
                Wrestler.JIFMEHIKLDI[0] = 0;
                Wrestler.JIFMEHIKLDI[1] = 0;
                Wrestler.JIFMEHIKLDI[2] = 0;
                for (int j = 1; j <= JFLEBEBCGFA.LLODPDKEEJG; j++)
                {
                    Debug.Log("Removing existing prop " + j.ToString() + " / " + JFLEBEBCGFA.LLODPDKEEJG.ToString());
                    if (JFLEBEBCGFA.HLLBCKILNNG[j].BHKGKKLDDBC != null)
                    {
                        UnityEngine.Object.Destroy(JFLEBEBCGFA.HLLBCKILNNG[j].BHKGKKLDDBC);
                    }
                }
                JFLEBEBCGFA.LLODPDKEEJG = 0;
                JFLEBEBCGFA.HLLBCKILNNG = new GDFKEAMIOAG[JFLEBEBCGFA.LLODPDKEEJG + 1];
                JFLEBEBCGFA.HLLBCKILNNG[0] = new GDFKEAMIOAG();
                if (Wrestler.PCNHIIPBNEK[0].activeSelf)
                {
                    Wrestler.AMPHLBAOCKC();
                }
                Wrestler.PCNHIIPBNEK[0].transform.SetParent(ParentObj.transform, true);
                if (JFLEBEBCGFA.HLLBCKILNNG != null)
                {
                    if (JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[0]] != null && JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[0]].PCNHIIPBNEK != null)
                    {
                        JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[0]].OFPBEHEIBBD(Wrestler.PLFGKLGCOMD, 13);
                        JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[0]].PCNHIIPBNEK.transform.SetParent(ParentObj.transform, true);
                    }
                    if (JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[1]] != null && JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[1]].PCNHIIPBNEK != null)
                    {

                        JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[1]].OFPBEHEIBBD(Wrestler.PLFGKLGCOMD, 10);
                        JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[1]].PCNHIIPBNEK.transform.SetParent(ParentObj.transform, true);
                    }
                    if (JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[2]] != null && JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[2]].PCNHIIPBNEK != null)
                    {
                        JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[2]].OFPBEHEIBBD(Wrestler.PLFGKLGCOMD, JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[2]].KDFCBHGKOKE);
                        JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[2]].PCNHIIPBNEK.transform.SetParent(ParentObj.transform, true);
                    }
                }

                ParentObj.transform.localScale = new Vector3(Size, Size, Size);
                Characters.c[Wrestler.GOOKPABIPBC].height = realHeight;
            }
            catch (Exception e)
            {
                Characters.c[Wrestler.GOOKPABIPBC].height = realHeight;
                throw e;
            }
        }
        public static void Cleanup()
        {
            UnityEngine.Object.Destroy(Wrestler.PCNHIIPBNEK[0]);
            for (int j = 1; j <= JFLEBEBCGFA.LLODPDKEEJG; j++)
            {
                Debug.Log("Removing existing prop " + j.ToString() + " / " + JFLEBEBCGFA.LLODPDKEEJG.ToString());
                if (JFLEBEBCGFA.HLLBCKILNNG[j].BHKGKKLDDBC != null)
                {
                    UnityEngine.Object.Destroy(JFLEBEBCGFA.HLLBCKILNNG[j].BHKGKKLDDBC);
                }
            }
            UnityEngine.Object.Destroy(ParentObj);

        }
    }
}