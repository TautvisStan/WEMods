﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CollectibleCards2
{
    public static class CollectibleCardGenerator
    {
        public static WaitForEndOfFrame frameEnd = new();
        public static IEnumerator GenerateCollectibleCard(Action<string> action, int CharID, int costume, string preset, int borderRarity, int foilRarity, int signatureRarity, bool customGenerated)
        {
            ProportionalRandomSelector<int> randomSelector;
            if (CharID == -1) CharID = UnityEngine.Random.Range(1, Characters.no_chars + 1);
            Character character = Characters.c[CharID];
            if (preset == "") preset = Path.Combine(Plugin.DefaultDesignDirectory, "Base_Fed_" + character.fed.ToString());
            if (costume == 0)
            {
                int wrCostume = 5;
                int caCostume = 5;
                int reCostume = 5;
                if (Characters.c[CharID].role == 1)
                    wrCostume += 85;
                else if (Characters.c[CharID].role == 3)
                    reCostume += 85;
                else
                    caCostume += 85;
                randomSelector = new();
                randomSelector.AddPercentageItem(1, wrCostume);
                randomSelector.AddPercentageItem(2, caCostume);
                randomSelector.AddPercentageItem(3, reCostume);
                costume = randomSelector.SelectItem();
            }
            if (borderRarity == -1)
            {
                randomSelector = new();
                randomSelector.AddPercentageItem(0, 60);
                randomSelector.AddPercentageItem(1, 25);
                randomSelector.AddPercentageItem(2, 10);
                randomSelector.AddPercentageItem(3, 5);
                borderRarity = randomSelector.SelectItem();
            }
            if (foilRarity == -1)
            {
                randomSelector = new();
                randomSelector.AddPercentageItem(0, 75);
                randomSelector.AddPercentageItem(1, 25);
                foilRarity = randomSelector.SelectItem();
            }
            if (signatureRarity == -1)
            {
                randomSelector = new();
                randomSelector.AddPercentageItem(0, 95);
                randomSelector.AddPercentageItem(1, 5);
                signatureRarity = randomSelector.SelectItem();
            }

            Dictionary<string, string> CardMetaData = new()
            {
                { "CharID", CharID.ToString() },
                { "Name", Characters.c[CharID].name },
                { "FedName", Characters.fedData[Characters.c[CharID].fed].name },
                { "Border", borderRarity.ToString() },
                { "Foil", foilRarity.ToString() },
                { "Signature", signatureRarity.ToString() },
                { "CustomGenerated", customGenerated.ToString() },
                { "Popularity", Characters.c[CharID].stat[1].ToString("0") },
                { "Strength", Characters.c[CharID].stat[2].ToString("0") },
                { "Skill", Characters.c[CharID].stat[3].ToString("0") },
                { "Agility", Characters.c[CharID].stat[4].ToString("0") },
                { "Stamina", Characters.c[CharID].stat[5].ToString("0") },
                { "Attitude", Characters.c[CharID].stat[6].ToString("0") },
                { "FrontFinisher", MBLIOKEDHHB.DDIJBPJLEBF(Characters.c[Characters.foc].moveFront[0]) },
                { "BackFinisher", MBLIOKEDHHB.DDIJBPJLEBF(Characters.c[Characters.foc].moveBack[0]) },
                { "CharData", "" },
            };
            try
            {
                (string[] scene, string[] canvas) = MetaTxtSplitter(Path.Combine(preset, "CardDesign.txt"));
                CameraController.SetupCamera(scene);
                LightController.SetupPictureLight();
                Background.SetupBackground(CameraController.CameraObj, preset);
                CharacterController.SetupCharacter(CharID, costume, scene);
                CanvasController.SetupCanvas(CameraController.CameraObj, preset, CardMetaData, canvas);
            }
            catch (Exception e)
            {
                Plugin.Log.LogError("There was an error generating custom card!");
                Plugin.Log.LogError(e);

                CameraController.Cleanup();
                LightController.Cleanup();
                Background.Cleanup();
                CharacterController.Cleanup();
                CanvasController.Cleanup();
                GC.Collect();
            }

            yield return frameEnd;
            try
            {
                CharacterController.SetupBelts();
                byte[] bytes = CameraController.CaptureScreenshot();
                string filename = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH'-'mm'-'ss'-'fff") + ".png";
                string foldername = Plugin.CardsDirectory;
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }
                if(WECCLHandler.WECCLLoaded)
                {
                    string charData = WECCLHandler.GetCharacterDataJson(CharID);
                    if (!charData.Contains("Custom/"))
                    {
                        CardMetaData["CharData"] = charData;
                    }
                }
                string filePath = Path.Combine(foldername, filename);
                PngUtils.SaveWithMetadata(filePath, bytes, CardMetaData);
                
                Plugin.Log.LogInfo($"Saved card image to {filePath}");
                action(filePath);
            }
            catch (Exception e)
            {
                Plugin.Log.LogError("There was an error generating custom card!");
                Plugin.Log.LogError(e);

            }
            finally
            {
                CameraController.Cleanup();
                LightController.Cleanup();
                Background.Cleanup();
                CharacterController.Cleanup();
                CanvasController.Cleanup();
                GC.Collect();
            }
        }
        public static (string[], string[]) MetaTxtSplitter(string file)
        {
            List<string> pre = new();
            bool layers = false;
            List<string> post = new();
            foreach (string line in File.ReadAllLines(file))
            {
                if(line != "layers")
                {
                    if (!layers)
                    {
                        pre.Add(line);
                    }
                    else
                    {
                        post.Add(line);
                    }
                }
                else
                {
                    layers = true;
                }
            }
            return (pre.ToArray(), post.ToArray());
        }
    }
    public class ProportionalRandomSelector<T>
    {
        private readonly Dictionary<T, int> percentageItemsDict;
        public ProportionalRandomSelector()
        {
            percentageItemsDict = new();
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
            int randomNumber = UnityEngine.Random.Range(1, poolSize);
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
