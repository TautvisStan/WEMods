//todo fix scene lightning; costumes based on character role; injured taunt?; figure out adding fed logo


using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace CollectibleCards
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.CollectibleCards";
        public const string PluginName = "CollectibleCards";
        public const string PluginVer = "0.0.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

        public static int CardWidth { get; set; } = 719;
        public static int CardHeight { get; set; } = 1000;

        public static ConfigEntry<float> CharXPos { get; set; }
        public static ConfigEntry<float> CharYPos { get; set; }
        public static ConfigEntry<float> CharSize { get; set; }

        public static ConfigEntry<int> CameraMode { get; set; }

        private void Awake()
        {
            Plugin.Log = base.Logger;
            PluginPath = Path.GetDirectoryName(Info.Location);

            Background.GetShaderPrefab();

            CharXPos = Config.Bind("General",
             "Character x position",
             0f,
             "Character x position");
            CharYPos = Config.Bind("General",
             "Character y position",
             0f,
             "Character y position");
            CharSize = Config.Bind("General",
             "Character scale",
             1f,
             "Character scale");

            CameraMode = Config.Bind("General",
             "Camera mode",
             0,
             "0 - perspective, 1 - orthographic");
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


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.PageDown))
            {
                CreateCharacterCard();
            }
        }
        public void CreateCharacterCard()
        {
            GameObject cam = SetupCamera();
            SetupCharacter();
            SetupPictureLight();
            Background.SetupBackground(cam);
            //  CardOverlay.SetupCanvas();
            //   yield return frameEnd;
            CaptureCameraView camView = cam.AddComponent<CaptureCameraView>();
            camView.StartCoroutine(camView.CaptureScreenshotWithCanvas());
            //CleanupScene();
        }
        public static class CardOverlay
        { 
        }
        public static class Background
        {
            public static GameObject BackgroundObj { get; set; } = null;
            public static Shader ShaderPrefab { get; set; }
            public static void GetShaderPrefab()
            {
                var assetBundle = AssetBundle.LoadFromFile(Path.Combine(PluginPath, "cardbgshader"));
                Debug.LogWarning(assetBundle);
                ShaderPrefab = assetBundle.LoadAsset<UnityEngine.Shader>("CardBGShader");
                Debug.LogWarning(ShaderPrefab);
            }
            public static void SetupBackground(GameObject cameraObject)
            {
                BackgroundObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
                BackgroundObj.transform.position = new Vector3(0f, 8f, -10f);
                BackgroundObj.transform.rotation = Quaternion.Euler(90f, 0f, 0);
                if (cameraObject.GetComponent<Camera>().orthographic)
                {
                    // Set the orthographic size based on the desired resolution
                    float orthographicHeight = 10;//CardHeight / 2.0f;
                    cameraObject.GetComponent<Camera>().orthographicSize = orthographicHeight;

                    // Calculate the scale of the background plane
                    float aspectRatio = (float)CardWidth / (float)CardHeight;
                    Vector3 planeScale = new Vector3(aspectRatio * orthographicHeight/5, 1, orthographicHeight/5);

                    // Apply the calculated scale to the background plane
                    BackgroundObj.transform.localScale = planeScale;

                }
                else
                {
                    float distance = Mathf.Abs(cameraObject.transform.position.z - BackgroundObj.transform.position.z);

                    // Calculate the height in world units based on the camera's FOV
                    float height = 2.0f * distance * Mathf.Tan(cameraObject.GetComponent<Camera>().fieldOfView * 0.5f * Mathf.Deg2Rad);

                    // Calculate the width in world units to match the texture's aspect ratio
                    float width = height * (float)CardWidth / (float)CardHeight;

                    // Scale the plane to match the calculated width and height
                    BackgroundObj.transform.localScale = new Vector3(width / 10, 1, height / 10);
                    //2.0756 1 2.8868 when the real distance is 20
                }

                string imagePath = Path.Combine(PluginPath, "BG.png");
                byte[] fileData = File.ReadAllBytes(imagePath);


                if (fileData != null)
                {
                    Texture2D spriteTexture = new Texture2D(1, 1); // You can adjust the size as needed
                    spriteTexture.LoadImage(fileData);
                    Sprite selectedBackground = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0.5f, 0.5f));
                    Texture2D texture = selectedBackground.texture;
                    // Check if background material is not null before setting its properties
                    Material material = new Material(ShaderPrefab);
                    material.mainTexture = texture;
                    if (BackgroundObj.GetComponent<Renderer>() != null)
                    {
                        BackgroundObj.GetComponent<Renderer>().material = material;
                    }
                    else
                    {
                        Debug.LogError("Renderer component on the background GameObject is null.");
                    }
                }

            }
            public static void DeleteBackground()
            {
                Destroy(BackgroundObj);
                BackgroundObj = null;
            }
        }
        public static GameObject SetupCamera()
        {
            // Setup hard camera
            GameObject cameraObject = new GameObject("CardCamera");

            // Attach the camera component to the new GameObject
            cameraObject.AddComponent<Camera>();

            // Set the position, rotation, or any other properties as needed
            cameraObject.transform.position = new Vector3(0f, 8f, 15f);
            Vector3 targetPosition = new Vector3(0f, 8f, 0f);
            cameraObject.transform.LookAt(targetPosition);
            if (CameraMode.Value == 1)
            {
                cameraObject.GetComponent<Camera>().orthographic = true;
                cameraObject.GetComponent<Camera>().orthographicSize = 10;
            }
            return cameraObject;
        }
        public static Texture2D ResizeTexture(Texture2D texture, int width, int height)
        {
            Texture2D resizedTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Color[] pixels = texture.GetPixels(0, 0, texture.width, texture.height);
            resizedTexture.SetPixels(0, 0, width, height, pixels);
            resizedTexture.Apply();
            return resizedTexture;
        }
        public static Texture2D OverlayTextures(Texture2D baseTexture, Texture2D overlay)
        {
            // Ensure the overlay is the same size as the base texture
            if (overlay.width != baseTexture.width || overlay.height != baseTexture.height)
            {
                overlay = ResizeTexture(overlay, baseTexture.width, baseTexture.height);
            }

            // Create a new texture to store the final image
            Texture2D result = new Texture2D(baseTexture.width, baseTexture.height, TextureFormat.RGBA32, false);

            // Loop through each pixel and combine the base and overlay textures
            for (int y = 0; y < result.height; y++)
            {
                for (int x = 0; x < result.width; x++)
                {
                    Color baseColor = baseTexture.GetPixel(x, y);
                    Color overlayColor = overlay.GetPixel(x, y);

                    // Alpha blend the overlay onto the base texture
                    Color finalColor = Color.Lerp(baseColor, overlayColor, overlayColor.a);
                    result.SetPixel(x, y, finalColor);
                }
            }

            result.Apply();
            return result;
        }

        public static GameObject SetupPictureLight()
        {
            GameObject LightObj = new GameObject("Light");
            LightObj.transform.position = new Vector3(13.73f, 16.33f, 17.79f);
            LightObj.transform.eulerAngles = new Vector3(39.4763f, 209.4147f, 227.5626f);
            Light LightComp = LightObj.AddComponent<Light>();
           // LightComp.color = new Color(1f, 0.9748f, 0.816f, 1f);
            LightComp.intensity = 1.125f;
            LightComp.shadowBias = 0.5f;
            LightComp.shadowResolution = UnityEngine.Rendering.LightShadowResolution.High;
            LightComp.type = LightType.Directional;
            return LightObj;
        }    
        public static void SetupCharacter()
        {
            int BDHHBIIKMLP = 1;
            int GOOKPABIPBC = UnityEngine.Random.Range(1, Characters.no_chars+1);
            if (NJBJIIIACEP.OAAMGFLINOB == null) NJBJIIIACEP.PIMGMPBCODM();
            DFOGOCNBECG dfogocnbecg = NJBJIIIACEP.OAAMGFLINOB[0];
            dfogocnbecg.GOOKPABIPBC = GOOKPABIPBC;
            dfogocnbecg.EMDMDLNJFKP = Characters.c[dfogocnbecg.GOOKPABIPBC];
            dfogocnbecg.OEGJEBDBGJA = dfogocnbecg.EMDMDLNJFKP.costume[BDHHBIIKMLP];
            if (dfogocnbecg.PCNHIIPBNEK[0] != null)
            {
                UnityEngine.Object.Destroy(dfogocnbecg.PCNHIIPBNEK[0]);
            }
            dfogocnbecg.DDKAGOBJGBC(0);
            dfogocnbecg.ABHDOPBDDPB();
            dfogocnbecg.PCNHIIPBNEK[0].transform.eulerAngles = new Vector3(0f, 0f, 0f);
            dfogocnbecg.PCNHIIPBNEK[0].transform.position = new Vector3(CharXPos.Value, CharYPos.Value, 0);
            dfogocnbecg.PCNHIIPBNEK[0].transform.localScale = new Vector3(CharSize.Value, CharSize.Value, CharSize.Value);
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
                       if (dfogocnbecg.EMDMDLNJFKP.gender > 0 && NAEEIFNFBBO.PMEEFNOLAGF(0, 1, 0) == 0)
                       {
                           num6 = 0;
                           num7 = NAEEIFNFBBO.PMEEFNOLAGF(82, 83, 0);
                       }
                       if (dfogocnbecg.EMDMDLNJFKP.injury != 0 && NAEEIFNFBBO.PMEEFNOLAGF(0, 1, 0) == 0)
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
            //dfogocnbecg.FJHHJGONAFO(num6, (float)num7);
            dfogocnbecg.KOLHFFPPCEE((float)(50 - 100 * dfogocnbecg.EMDMDLNJFKP.heel));
            int num = NAEEIFNFBBO.OMOADEKHHHO(MBLIOKEDHHB.ABJFEMNCIMI);
            dfogocnbecg.FEOFDJFFNMN = MBLIOKEDHHB.LHFJJPOPIAA[num].PFDGHMKKHOF;
            dfogocnbecg.LMALJJFEHGH = MBLIOKEDHHB.LHFJJPOPIAA[num].EJPKJOFMIAI[NAEEIFNFBBO.PMEEFNOLAGF(0, MBLIOKEDHHB.LHFJJPOPIAA[num].EJPKJOFMIAI.Length - 1)];
            dfogocnbecg.FJHHJGONAFO(dfogocnbecg.FEOFDJFFNMN, dfogocnbecg.LMALJJFEHGH);


        }
        public class CaptureCameraView : MonoBehaviour
        {
       /*     private void RenderCanvasToTexture(Canvas overlayCanvas, Camera captureCamera, RenderTexture rt)
            {
                // Set the overlay canvas to render into the RenderTexture
                overlayCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                overlayCanvas.worldCamera = captureCamera;
                overlayCanvas.targetDisplay = 0;
                overlayCanvas.planeDistance = 1;

                // Render the overlay onto the RenderTexture
                Canvas.ForceUpdateCanvases(); // Update the canvas with the latest changes
                Graphics.Blit(null, rt); // Blit the canvas content onto the RenderTexture
            }*/

            public IEnumerator CaptureScreenshotWithCanvas()
            {
                Camera targetCamera = this.GetComponent<Camera>();
                List<Texture2D> overlayTextures = new();
                //      var overlayText = "HELLO WORLD";
                //       var textFontSize = 80;
                //        var textColor = new Color(1, 1, 1);

                // Create the canvas and add the overlay elements
                GameObject canvasObj = new GameObject("CanvasObj");
                Canvas overlayCanvas = canvasObj.AddComponent<Canvas>();
                overlayCanvas.renderMode = RenderMode.ScreenSpaceCamera; // Set to Screen Space - Camera
                overlayCanvas.worldCamera = targetCamera; // Attach the camera to the canvas
                overlayCanvas.sortingOrder = 100; // Ensure it's rendered on top
                overlayCanvas.planeDistance = 1;





                // string overlayfile = Path.Combine(PluginPath, "Pictures", "FoilOverlay.png");

                DirectoryInfo dir = new DirectoryInfo(PluginPath);
                FileInfo[] files = dir.GetFiles("*");

                foreach (FileInfo file in files)
                {
                    if (file.Name.StartsWith("overlay"))
                    {
                        byte[] array2 = File.ReadAllBytes(file.FullName);
                        if (array2 != null)
                        {
                            Texture2D texture2D = new Texture2D(1, 1);
                            ImageConversion.LoadImage(texture2D, array2);
                            Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
                            overlayTextures.Add (sprite.texture);
                            //     screenShot = OverlayTextures(screenShot, texture);
                            //  AddOverlayTextureToCanvas(texture, overlayCanvas);
                        }
                    }
                }


                // Add the overlay texture and text to the canvas

             //   AddTextToCanvas(overlayCanvas, overlayText, new Vector2(-300, 0), textFontSize, textColor);
             //   AddOverlayTextureToCanvas(overlayTexture, overlayCanvas);

                foreach(Texture2D texture2D in overlayTextures)
                {
                    AddOverlayTextureToCanvas(texture2D, overlayCanvas);
                }
                // Wait for the end of the frame to ensure everything is rendered
                yield return new WaitForEndOfFrame();

                // Capture the screenshot here
                CaptureScreenshot(targetCamera);

                // Optionally, clean up the canvas after capturing
               // Destroy(canvasObj);
            }

            void CaptureScreenshot(Camera camera)
            {
                int captureHeight = CardHeight;
                int captureWidth = CardWidth;
                RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24);
                camera.targetTexture = rt;
                Texture2D screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
                camera.Render();
                RenderTexture.active = rt;
                screenShot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
                screenShot.Apply();
                camera.targetTexture = null;
                RenderTexture.active = null;
                Destroy(rt);

                // Encode the Texture2D into PNG format with transparency
                byte[] bytes = screenShot.EncodeToPNG();
                string filename = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH'-'mm'-'ss'-'fff") + ".png";
                string filePath = Path.Combine(PluginPath, filename);
                // Save the PNG file
                File.WriteAllBytes(filePath, bytes);
                Debug.LogWarning($"Saved image to {filePath}");
            }

            void AddOverlayTextureToCanvas(Texture2D texture, Canvas canvas)
            {
                GameObject rawImageObject = new GameObject(texture.name);
                rawImageObject.transform.SetParent(canvas.transform, false);
                RawImage rawImage = rawImageObject.AddComponent<RawImage>();
                rawImage.texture = texture;
                RectTransform rectTransform = rawImage.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(texture.width, texture.height);
                rectTransform.anchoredPosition = Vector2.zero;
                rawImage.transform.SetAsLastSibling();
            }

            void AddTextToCanvas(Canvas canvas, string text, Vector2 position, int fontSize, Color color)
            {
                GameObject textObject = new GameObject(text);
                textObject.transform.SetParent(canvas.transform, false);
                Text uiText = textObject.AddComponent<Text>();
                uiText.text = text;
                uiText.horizontalOverflow = HorizontalWrapMode.Overflow;
                uiText.fontSize = fontSize;
                uiText.color = color;
                uiText.alignment = TextAnchor.UpperLeft;
                uiText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                RectTransform rectTransform = uiText.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = position;
                rectTransform.sizeDelta = new Vector2(200, 100);
                uiText.transform.SetAsLastSibling();
            }

        }
    }
}