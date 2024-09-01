using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace CollectibleCards2
{
    public static class CanvasController
    {
        public static GameObject CanvasObj { get; set; } = null;
        private static List<Texture2D> oldTextures = new();
        public static void SetupCanvas(GameObject CameraObj, string preset, Dictionary<string, string> CardMeta, string[] layers)
        {
            Camera targetCamera = CameraObj.GetComponent<Camera>();
            CanvasObj = new GameObject("CanvasObj");
            Canvas overlayCanvas = CanvasObj.AddComponent<Canvas>();
            overlayCanvas.renderMode = RenderMode.ScreenSpaceCamera; // Set to Screen Space - Camera
            overlayCanvas.worldCamera = targetCamera; // Attach the camera to the canvas
            overlayCanvas.sortingOrder = 100; // Ensure it's rendered on top
            overlayCanvas.planeDistance = 1;

            foreach (string line in layers)
            {
                string[] chunks = line.Split(new char[] { ' ' }, 2);
                if(chunks.Length == 2)
                {
                    string file = Path.Combine(Plugin.PluginPath, preset, chunks[1]);
                    if (chunks[0] == "border")
                    {
                        switch (int.Parse(CardMeta["Border"]))
                        {
                            case 0: 
                                AddOverlayTextureToCanvas(overlayCanvas, file);
                                break;
                            case 1: 
                                if (File.Exists(Path.Combine(Plugin.PluginPath, preset, "bronze.png")))
                                {
                                    AddOverlayTextureToCanvas(overlayCanvas, Path.Combine(Plugin.PluginPath, preset, "bronze.png"));
                                }
                                break;
                            case 2:
                                if (File.Exists(Path.Combine(Plugin.PluginPath, preset, "silver.png")))
                                {
                                    AddOverlayTextureToCanvas(overlayCanvas, Path.Combine(Plugin.PluginPath, preset, "silver.png"));
                                }
                                break;
                            case 3:
                                if (File.Exists(Path.Combine(Plugin.PluginPath, preset, "gold.png")))
                                {
                                    AddOverlayTextureToCanvas(overlayCanvas, Path.Combine(Plugin.PluginPath, preset, "gold.png"));
                                }
                                break;
                        }
                    }
                    if (chunks[0] == "overlay")
                    {
                        AddOverlayTextureToCanvas(overlayCanvas, file);
                    }
                    if (chunks[0] == "name")
                    {
                        AddCharNameTextToCanvas(overlayCanvas, file, int.Parse(CardMeta["CharID"]));
                    }
                    if (chunks[0] == "number")
                    {
                        AddNumberTextToCanvas(overlayCanvas, file, int.Parse(CardMeta["CharID"]));
                    }
                    if (chunks[0] == "fed")
                    {
                        AddFedNameTextToCanvas(overlayCanvas, file, int.Parse(CardMeta["CharID"]));
                    }
                    if (chunks[0] == "logo")
                    {
                        AddFedLogoToCanvas(overlayCanvas, file, int.Parse(CardMeta["CharID"]));
                    }
                    if (chunks[0] == "foil" && CardMeta["Foil"] != "0")
                    {
                        AddOverlayTextureToCanvas(overlayCanvas, file);
                    }
                    if (chunks[0] == "signature" && CardMeta["Signature"] != "0")
                    {
                        AddCharSigTextToCanvas(overlayCanvas, file, int.Parse(CardMeta["CharID"]));
                    }
                }
            }
        }
        
        public static void AddOverlayTextureToCanvas(Canvas canvas, string file)
        {
            byte[] array2 = File.ReadAllBytes(file);
            if (array2 != null)
            {
                Texture2D texture2D = new Texture2D(1, 1);
                ImageConversion.LoadImage(texture2D, array2);
                GameObject rawImageObject = new(texture2D.name);
                rawImageObject.transform.SetParent(canvas.transform, false);
                RawImage rawImage = rawImageObject.AddComponent<RawImage>();
                rawImage.texture = texture2D;
                RectTransform rectTransform = rawImage.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(texture2D.width, texture2D.height);
                rectTransform.anchoredPosition = Vector2.zero;
                rawImage.transform.SetAsLastSibling();
                oldTextures.Add(texture2D);
            }
        }

        public static void AddFedLogoToCanvas(Canvas canvas, string file, int id)
        {
            GameObject imageObject = new GameObject("FedLogo");
            imageObject.transform.SetParent(canvas.transform, false);
            Image image = imageObject.AddComponent<Image>();
            OverlayFedLogo overlayLogo = new();
            overlayLogo.Parse(file);
            image.sprite = MCDCDEBALPI.HJMMBCFGCKA[Characters.c[id].fed];
            RectTransform rectTransform = image.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(image.sprite.texture.width, image.sprite.texture.height);
            rectTransform.localScale = new Vector3(overlayLogo.Size, overlayLogo.Size, 1);
            rectTransform.anchoredPosition = overlayLogo.GetCanvasPos();
            image.transform.SetAsLastSibling();
        }

        public static void AddNumberTextToCanvas(Canvas canvas, string file, int id)
        {
            GameObject textObject = new("Number");
            textObject.transform.SetParent(canvas.transform, false);
            Text uiText = textObject.AddComponent<Text>();
            OverlayText ovrText = OverlaytxtFileParser.ParseCharNumber(file, uiText, id);
            AddText(ovrText, uiText);
            uiText.transform.SetAsLastSibling();
        }
        public static void AddCharNameTextToCanvas(Canvas canvas, string file, int id)
        {
            GameObject textObject = new("Name");
            textObject.transform.SetParent(canvas.transform, false);
            Text uiText = textObject.AddComponent<Text>();
            OverlayText ovrText = OverlaytxtFileParser.ParseCharName(file, uiText, id);
            AddText(ovrText, uiText);

            uiText.transform.SetAsLastSibling();
        }
        public static void AddFedNameTextToCanvas(Canvas canvas, string file, int id)
        {
            GameObject textObject = new("FedName");
            textObject.transform.SetParent(canvas.transform, false);
            Text uiText = textObject.AddComponent<Text>();
            OverlayText ovrText = OverlaytxtFileParser.ParseFedName(file, uiText, id);
            AddText(ovrText, uiText);

            uiText.transform.SetAsLastSibling();
        }
        public static void AddCharSigTextToCanvas(Canvas canvas, string file, int id)
        {
            GameObject textObject = new GameObject("Sig");
            textObject.transform.SetParent(canvas.transform, false);
            Text uiText = textObject.AddComponent<Text>();
            OverlayText ovrText = OverlaytxtFileParser.ParseCharSig(file, uiText, id);
            AddText(ovrText, uiText);

            uiText.transform.SetAsLastSibling();
        }
        public static void AddText(OverlayText ovrText, Text uiText)
        {
            uiText.horizontalOverflow = HorizontalWrapMode.Wrap;
            uiText.verticalOverflow = VerticalWrapMode.Overflow;
            uiText.color = new Color(ovrText.ColR, ovrText.ColG, ovrText.ColB);
            RectTransform rectTransform = uiText.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = ovrText.GetCanvasPos();
            rectTransform.eulerAngles = new Vector3(ovrText.RotX, ovrText.RotY, ovrText.RotZ);
            rectTransform.sizeDelta = new Vector2(ovrText.BoxX, ovrText.BoxY);
            if (ovrText.DrawBox == true) DrawBox(uiText);
        }
        public static void DrawBox(Text uiText)
        {
            // Create a new GameObject for the background image
            GameObject backgroundObject = new GameObject("TextBackground");

            // Add the Image component to the new GameObject
            UnityEngine.UI.Image backgroundImage = backgroundObject.AddComponent<UnityEngine.UI.Image>();

            // Set the background color (e.g., red with some transparency)
            backgroundImage.color = new Color(1, 0, 0, 0.5f); // Red color with 50% transparency

            // Get the RectTransform components
            RectTransform textRectTransform = uiText.GetComponent<RectTransform>();
            RectTransform backgroundRectTransform = backgroundObject.GetComponent<RectTransform>();

            // Set the parent of the background to be the same as the text's parent
            backgroundRectTransform.SetParent(uiText.transform, false);

            // Match the size of the background to the text box
            backgroundRectTransform.anchorMin = textRectTransform.anchorMin;
            backgroundRectTransform.anchorMax = textRectTransform.anchorMax;
            // backgroundRectTransform.anchoredPosition = textRectTransform.anchoredPosition;
            backgroundRectTransform.sizeDelta = textRectTransform.sizeDelta;

            // Optionally, adjust the background to be behind the text
            backgroundObject.transform.SetSiblingIndex(uiText.transform.GetSiblingIndex());
        }
        public static void AddTextToCanvas(Canvas canvas, string text, Vector2 position, int fontSize, Color color)
        {
            GameObject textObject = new(text);
            textObject.transform.SetParent(canvas.transform, false);
            Text uiText = textObject.AddComponent<Text>();
            uiText.text = text;
            uiText.horizontalOverflow = HorizontalWrapMode.Overflow;
            uiText.fontSize = fontSize;
            uiText.color = color;
            uiText.alignment = TextAnchor.UpperLeft;

            RectTransform rectTransform = uiText.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = position;
            rectTransform.sizeDelta = new Vector2(200, 100);
            uiText.transform.SetAsLastSibling();
        }
        public static void Cleanup()
        {
            Object.Destroy(CanvasObj);
            CanvasObj = null;
            if (oldTextures != null)
            {
                for (int i = 0; i < oldTextures.Count; i++)
                {
                    if (oldTextures[i] != null)
                    {
                        GameObject.Destroy(oldTextures[i]);
                        oldTextures[i] = null;
                    }
                }
                oldTextures = new();
            }
        }
    }
}
