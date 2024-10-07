using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CollectibleCards2
{
    public class WECCLHandler
    {
        internal static bool WECCLLoaded { get; set; } = false;
        public static void CheckWECCLStatus()
        {
            WECCLLoaded = IsWECCLLoaded();
        }
        private static bool IsWECCLLoaded()
        {
            try
            {
                // Try to load WECCL.dll assembly
                Assembly.Load("WECCL");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static string GetCharacterDataJson(int CharID)
        {
            string json = "";
            try
            {
                Type BetterCharDataType = Type.GetType("WECCL.Saves.BetterCharacterData, WECCL");
                var chardataobj = new System.Object();


                MethodInfo methodInfo = BetterCharDataType.GetMethod("FromRegularCharacter", BindingFlags.Static | BindingFlags.Public);
                object[] parameters = new object[] { Characters.c[CharID], Characters.c };
                chardataobj = methodInfo.Invoke(null, parameters);

                Type BetterCharDataFileType = Type.GetType("WECCL.Saves.BetterCharacterDataFile, WECCL");

                var constructor = TypeHelper.CreateDefaultConstructor(BetterCharDataFileType);
                var chardatafileobj = constructor.Invoke();
                FieldInfo CharacterDataField = chardatafileobj.GetType().GetField("characterData");
                FieldInfo OverrideModeField = chardatafileobj.GetType().GetField("overrideMode");
                Debug.LogWarning(CharacterDataField != null);
                Debug.LogWarning(OverrideModeField != null);

                CharacterDataField.SetValue(chardatafileobj, chardataobj);
                OverrideModeField.SetValue(chardatafileobj, "append");
                json = JsonConvert.SerializeObject(chardatafileobj, Newtonsoft.Json.Formatting.Indented);
            }
            catch (Exception e)
            {
                Plugin.Log.LogError("Failed to add full character data! ");
                Plugin.Log.LogError(e);
            }
            return json;
        }
    }
}
