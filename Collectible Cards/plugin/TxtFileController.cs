using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace CollectibleCards2
{
    public class OverlayFedLogo
    {
        public float PosX { get; set; } = 0;
        public float PosY { get; set; } = 0;
        public float RotX { get; set; } = 0;
        public float RotY { get; set; } = 180;
        public float RotZ { get; set; } = 0;
        public float Size { get; set; } = 1;

        public void Parse(string fullFilePath)
        {

            string[] lines = File.ReadAllLines(fullFilePath);
            foreach (string line in lines)
            {
                if (line.Trim().Length == 0)
                {
                    continue;
                }
                if (line.ToLower().StartsWith("size:"))
                {
                    Size = float.Parse(line.Substring(5).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("posx:"))
                {
                    PosX = float.Parse(line.Substring(5).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("posy:"))
                {
                    PosY = float.Parse(line.Substring(5).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("rotx:"))
                {
                    RotX = float.Parse(line.Substring(5).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("roty:"))
                {
                    RotY = float.Parse(line.Substring(5).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("rotz:"))
                {
                    RotZ = float.Parse(line.Substring(5).Trim());
                    continue;
                }
            }
        }
        public Vector2 GetCanvasPos()
        {
            return new Vector2(PosX - (Plugin.CardWidth / 2), (Plugin.CardHeight / 2) - PosY);
        }
    }
    public class OverlayText
    {
        public Text TextComponent { get; set; } = new();
        public float PosX { get; set; } = 0;
        public float PosY { get; set; } = 0;
        public float RotX { get; set; } = 0;
        public float RotY { get; set; } = 180;
        public float RotZ { get; set; } = 0;
        public float ColR { get; set; } = 0;
        public float ColG { get; set; } = 0;
        public float ColB { get; set; } = 0;
        public float BoxX { get; set; } = 200;
        public float BoxY { get; set; } = 100;
        public bool DrawBox { get; set; } = false;
        public OverlayText(Text component)
        {
            TextComponent = component;
        }
        public Vector2 GetCanvasPos()
        {
            return new Vector2(PosX - (Plugin.CardWidth / 2), (Plugin.CardHeight / 2) - PosY);
        }
    }
    public static class OverlaytxtFileParser
    {
        public static Font SignatureFont = new();
        public static Font CardFont = new();
        public static void LoadSigFont()
        {
            var assetBundle = AssetBundle.LoadFromFile(Path.Combine(Plugin.PluginPath, "prefabs", "signaturefont"));
            SignatureFont = assetBundle.LoadAsset<Font>("Signaturefont");
        }
        public static void LoadCardFont()
        {
            var assetBundle = AssetBundle.LoadFromFile(Path.Combine(Plugin.PluginPath, "prefabs", "cardfont"));
            CardFont = assetBundle.LoadAsset<Font>("CardFont");
        }
        public static OverlayText ParseCharName(string file, Text component, int charID)
        {
            OverlayText overlayText = Parser(file, component);
            overlayText.TextComponent.text = Characters.c[charID].name;
            return overlayText;
        }
        public static OverlayText ParseFedName(string file, Text component, int charID)
        {
            OverlayText overlayText = Parser(file, component);
            overlayText.TextComponent.text = Characters.fedData[Characters.c[charID].fed].name;
            return overlayText;
        }
        public static OverlayText ParseCharSig(string file, Text component, int charID)
        {
            OverlayText overlayText = Parser(file, component);
            overlayText.TextComponent.text = Characters.c[charID].name;
            overlayText.TextComponent.font = SignatureFont;
            return overlayText;
        }
        public static OverlayText ParseCharNumber(string file, Text component, int charID)
        {
            OverlayText overlayText = Parser(file, component);
            overlayText.TextComponent.text = charID.ToString();
            return overlayText;
        }
        public static OverlayText Parser(string fullFilePath, Text component)
        {
            OverlayText overlayText = new(component);
            overlayText.TextComponent.font = CardFont;
            string[] lines = File.ReadAllLines(fullFilePath);
            foreach (string line in lines)
            {
                if (line.Trim().Length == 0)
                {
                    continue;
                }
                if (line.ToLower().StartsWith("text:"))
                {
                    overlayText.TextComponent.text = line.Substring(5).Trim();
                    continue;
                }
                if (line.ToLower().StartsWith("size:"))
                {
                    overlayText.TextComponent.fontSize = int.Parse(line.Substring(5).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("colorr:"))
                {
                    overlayText.ColR = float.Parse(line.Substring(7).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("colorg:"))
                {
                    overlayText.ColG = float.Parse(line.Substring(7).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("colorb:"))
                {
                    overlayText.ColB = float.Parse(line.Substring(7).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("posx:"))
                {
                    overlayText.PosX = float.Parse(line.Substring(5).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("posy:"))
                {
                    overlayText.PosY = float.Parse(line.Substring(5).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("rotx:"))
                {
                    overlayText.RotX = float.Parse(line.Substring(5).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("roty:"))
                {
                    overlayText.RotY = float.Parse(line.Substring(5).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("rotz:"))
                {
                    overlayText.RotZ = float.Parse(line.Substring(5).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("boxx:"))
                {
                    overlayText.BoxX = float.Parse(line.Substring(5).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("boxy:"))
                {
                    overlayText.BoxY = float.Parse(line.Substring(5).Trim());
                    continue;
                }
                if (line.ToLower() == "drawbox")
                {
                    overlayText.DrawBox = true;
                    continue;
                }
                if (line.ToLower().StartsWith("alignment:"))
                {
                    if (Enum.TryParse<TextAnchor>(line.Substring(10).Trim(), true, out TextAnchor alignment))
                    {
                        overlayText.TextComponent.alignment = alignment;
                    }
                    continue;
                }
            }
            return overlayText;
        }
    }
}
