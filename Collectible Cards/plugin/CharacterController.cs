using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CollectibleCards2
{
    internal class CharacterController
    {
        public static DFOGOCNBECG Wrestler { get; set; } = null;
        public static void SetupCharacter(int id)
        {
            int BDHHBIIKMLP = 1;
            int GOOKPABIPBC = id;
            if (NJBJIIIACEP.OAAMGFLINOB == null) NJBJIIIACEP.PIMGMPBCODM(1);
            NJBJIIIACEP.NBBBLJDBLNM = 1;
            Wrestler = NJBJIIIACEP.OAAMGFLINOB[1];
            Wrestler.GOOKPABIPBC = GOOKPABIPBC;
            Wrestler.EMDMDLNJFKP = Characters.c[Wrestler.GOOKPABIPBC];
            Wrestler.OEGJEBDBGJA = Wrestler.EMDMDLNJFKP.costume[BDHHBIIKMLP];
            Wrestler.PLFGKLGCOMD = 1;
            if (Wrestler.PCNHIIPBNEK[0] != null)
            {
                UnityEngine.Object.Destroy(Wrestler.PCNHIIPBNEK[0]);
            }
            Wrestler.DDKAGOBJGBC(0);
            Wrestler.ABHDOPBDDPB();
            Wrestler.PCNHIIPBNEK[0].transform.eulerAngles = new Vector3(0f, 0f, 0f);
            Wrestler.PCNHIIPBNEK[0].transform.position = new Vector3(Plugin.CharXPos.Value, Plugin.CharYPos.Value, 0);
            Wrestler.PCNHIIPBNEK[0].transform.localScale = new Vector3(Plugin.CharSize.Value, Plugin.CharSize.Value, Plugin.CharSize.Value);



            /**/
            /*       int num6;
                   int num7;
                   int num8;
                   do
                   {
                       int num5 = NAEEIFNFBBO.OMOADEKHHHO(MBLIOKEDHHB.AKICIDBAGOC, 0);
                       num6 = MBLIOKEDHHB.LHFJJPOPIAA[num5].PFDGHMKKHOF;
                       num7 = MBLIOKEDHHB.LHFJJPOPIAA[num5].EJPKJOFMIAI[NAEEIFNFBBO.PMEEFNOLAGF(0, MBLIOKEDHHB.LHFJJPOPIAA[num5].EJPKJOFMIAI.Length - 1, 0)];
                       if (NAEEIFNFBBO.PMEEFNOLAGF(0, 3, 0) == 0)
                       {
                           num6 = 0;
                           num7 = NAEEIFNFBBO.PMEEFNOLAGF(3, 24, 0);
                       }
                       if (Wrestler.EMDMDLNJFKP.gender > 0 && NAEEIFNFBBO.PMEEFNOLAGF(0, 1, 0) == 0)
                       {
                           num6 = 0;
                           num7 = NAEEIFNFBBO.PMEEFNOLAGF(82, 83, 0);
                       }
                       if (Wrestler.EMDMDLNJFKP.injury != 0 && NAEEIFNFBBO.PMEEFNOLAGF(0, 1, 0) == 0)
                       {
                           num6 = 4;
                           num7 = NAEEIFNFBBO.PMEEFNOLAGF(1, 6, 0);
                       }
                       num8 = 1;
                       if (num6 == 3 && (num7 == 36 || num7 == 37 || num7 == 42))
                       {
                           num8 = 0;
                       }
                       if (num6 == 4 && num7 == 39)
                       {
                           num8 = 0;
                       }
                   }
                   while (num8 == 0);*/
            //Wrestler.FJHHJGONAFO(num6, (float)num7);
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
            //  Wrestler.MKFMMIPFKKC();

            if (JFLEBEBCGFA.HLLBCKILNNG != null)
            {

                JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[0]]?.OFPBEHEIBBD(Wrestler.PLFGKLGCOMD, 13);
                JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[1]]?.OFPBEHEIBBD(Wrestler.PLFGKLGCOMD, 10);
                JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[2]]?.OFPBEHEIBBD(Wrestler.PLFGKLGCOMD, JFLEBEBCGFA.HLLBCKILNNG[Wrestler.JIFMEHIKLDI[2]].KDFCBHGKOKE);

            }
        }
        public static void Cleanup()
        {
            UnityEngine.Object.Destroy(Wrestler.PCNHIIPBNEK[0]);

        }
    }
}
