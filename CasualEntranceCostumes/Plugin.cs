using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using UnityEngine;
using BepInEx.Configuration;
using System.Collections;
using UnityEngine.TextCore;

namespace CasualEntranceCostumes
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.CasualEntranceCostumes";
        public const string PluginName = "CasualEntranceCostumes";
        public const string PluginVer = "1.0.3";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static ConfigEntry<KeyCode> ChangeClothesButton;
        WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

        public static System.Func<DFOGOCNBECG, IEnumerator> ChangeClothesFuncKeybind;
        public static Plugin plugin;

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);

            ChangeClothesButton = Config.Bind("General", "Costume change keybind", KeyCode.None, "Changes the current entrants' costume from casual to wrestling");

            ChangeClothesFuncKeybind = ChangeClothesOnKeybind;
            plugin = this;
        }

        private void OnEnable()
        {
            Harmony.PatchAll();
            Logger.LogInfo($"Loaded {PluginName}!");
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            Logger.LogInfo($"Unloaded {PluginName}!");
        }
        [HarmonyPatch(typeof(Scene_Game), nameof(Scene_Game.Start))]
        [HarmonyPostfix]
        public static void Scene_Game_Start()
        {
            if (FFCEGMEAIBP.LOBDMDPMFLK == 1 && FFCEGMEAIBP.CBIPLGLDCAG != 1)
            {
                for (int i = 1; i <= NJBJIIIACEP.NBBBLJDBLNM; i++)
                {
                    DFOGOCNBECG dfogocnbecg = NJBJIIIACEP.OAAMGFLINOB[i];
                    Character character = dfogocnbecg.EMDMDLNJFKP;
                    if (dfogocnbecg.FIEMGOLBHIO == 1 && dfogocnbecg.PCNHIIPBNEK[0] != null)
                    {
                        ChangeCostume(dfogocnbecg, 2);
                    }

                }
            }
        }
        public static void ChangeCostume(DFOGOCNBECG dfogocnbecg, int costume)
        {
            Character character = dfogocnbecg.EMDMDLNJFKP;
            var a = dfogocnbecg.OHBOIDGNIOE[0];
            var b = dfogocnbecg.EJPKJOFMIAI[0];
            dfogocnbecg.IIAHHOIOBMF(character.id, costume);
            dfogocnbecg.FKICFLEIGEA(a, b, 0);

            for (int i = 0; i <= 3; i++)
            {
                dfogocnbecg.MPMGGCCFCOP.Play(MBLIOKEDHHB.NNEMALOMALN(a), i, b / MBLIOKEDHHB.NIMHPNKOPAE[a]);
            }
            dfogocnbecg.FEACEIIIAHK();

            if (dfogocnbecg.AHBNKMMMGFI == 0)
            {
                dfogocnbecg.PCNHIIPBNEK[0].SetActive(false);
            }
        }
        public IEnumerator ChangeClothesOnKeybind(DFOGOCNBECG dfogocnbecg)
        {
            yield return frameEnd;
            ChangeCostume(dfogocnbecg, 1);
            yield break;

        }
        [HarmonyPatch(typeof(Scene_Game), nameof(Scene_Game.Update))]
        [HarmonyPostfix]
        public static void Scene_Game_Update() 
        {
            if (Input.GetKeyDown(ChangeClothesButton.Value))
            {
                if (FFCEGMEAIBP.LOBDMDPMFLK == 1)
                {
                    for (int i = 1; i <= NJBJIIIACEP.NBBBLJDBLNM; i++)
                    {
                        DFOGOCNBECG dfogocnbecg = NJBJIIIACEP.OAAMGFLINOB[i];
                        Character character = dfogocnbecg.EMDMDLNJFKP;
                        if (dfogocnbecg.FIEMGOLBHIO == 1 && dfogocnbecg.NEMJMNEGAAH(FFCEGMEAIBP.LPBCEGPJNMF) > 0 && dfogocnbecg.PCNHIIPBNEK[0] != null)
                        {
                            plugin.StartCoroutine(ChangeClothesFuncKeybind(dfogocnbecg));
                        }
                    }
                }
            }
        }
        //change it back to wrestling on match start
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.DMJFCHKLEFH))]
        [HarmonyPostfix]
        public static void FFCEGMEAIBP_DMJFCHKLEFH_Postfix()
        {
            if (FFCEGMEAIBP.CBIPLGLDCAG != 1 && FFCEGMEAIBP.LOBDMDPMFLK == 2)
            {
                for (int i = 1; i <= NJBJIIIACEP.NBBBLJDBLNM; i++)
                {
                    DFOGOCNBECG dfogocnbecg = NJBJIIIACEP.OAAMGFLINOB[i];
                    Character character = dfogocnbecg.EMDMDLNJFKP;
                    if (dfogocnbecg.FIEMGOLBHIO == 1 && dfogocnbecg.PCNHIIPBNEK[0] != null)
                    {
                        ChangeCostume(dfogocnbecg, 1);
                    }
                }
            }
        }
        //rename the wrestling costume button
        [HarmonyPatch(typeof(AKFIIKOMPLL), nameof(AKFIIKOMPLL.ICGNAJFLAHL))]
        [HarmonyPrefix]
        public static void AKFIIKOMPLL_ICGNAJFLAHL_Prefix(AKFIIKOMPLL __instance, int CHMHJJNEMKB, ref string NMKKHDOGOGA, float DPBNKMPJJOJ, float NKEMECHAEEJ, float BGPLCHIKEAK, float JOIPMMGOLFI)
        {
            if (LIPNHOMGGHF.FAKHAFKOBPB == 60 && LIPNHOMGGHF.CHLJMEPFJOK == 2 && LIPNHOMGGHF.ODOAPLMOJPD == 0 && NMKKHDOGOGA == "Casual")
            {
                NMKKHDOGOGA = "Casual/Entrance";
            }
        }
    }
}