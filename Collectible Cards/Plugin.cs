//todo fix scene lightning


using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UIElements;

namespace CollectibleCards
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.CollectibleCards";
        public const string PluginName = "CollectibleCards";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

        static Plugin thisPlugin;

        private void Awake()
        {
            Plugin.Log = base.Logger;
            thisPlugin = this;
            PluginPath = Path.GetDirectoryName(Info.Location);
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
                StartCoroutine(this.aaaa());
               // GetCharacterPicture();
            }
        }
        public IEnumerator aaaa()
        {
            SetupCharacter();
            SetupPictureLight();
            yield return frameEnd;
            //  if (LIPNHOMGGHF.CMOMBJMMOBK > 0f)
            GetCharacterPicture();

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
            LightComp.color = new Color(1f, 0.9748f, 0.816f, 1f);
            LightComp.intensity = 1f;
            LightComp.shadowBias = 0.5f;
            LightComp.shadowResolution = UnityEngine.Rendering.LightShadowResolution.High;
            LightComp.type = LightType.Directional;
            return LightObj;
        }    
        public static void SetupCharacter()
        {
            int BDHHBIIKMLP = 1; int FKEMEFPKBBL = 1; float JNLAJNFCDHA = 0.75f;
            int GOOKPABIPBC = 1;
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



            foreach (Transform transform in dfogocnbecg.PCNHIIPBNEK[0].GetComponentsInChildren<Transform>())
            {
                if (transform.gameObject.name == "Hair")
                {
              //      transform.gameObject.layer = 10;
                }
            }
        }
        public static void GetCharacterPicture()
        {
            // Setup hard camera
            GameObject cameraObject = new GameObject("CardCamera");
            
            // Attach the camera component to the new GameObject
            Camera cameracomp = cameraObject.AddComponent<Camera>();

            // Set the position, rotation, or any other properties as needed
            cameraObject.transform.position = new Vector3(0f, 8f, 15f);
            Vector3 targetPosition = new Vector3(0f, 8f, 0f);
            cameraObject.transform.LookAt(targetPosition);




          //  float screenAspectRatio = (float)( 719 / 1000);
         //   cameracomp.aspect = screenAspectRatio;


            var background = GameObject.CreatePrimitive(PrimitiveType.Plane);
            background.transform.position = new Vector3(0f, 8f, -10f);
            background.transform.rotation = Quaternion.Euler(90f, 0f, 0);
           float distance = Mathf.Abs(cameraObject.transform.position.z - background.transform.position.z);

            // Calculate the height in world units based on the camera's FOV
            float height = 2.0f * distance * Mathf.Tan(cameracomp.fieldOfView * 0.5f * Mathf.Deg2Rad);

            // Calculate the width in world units to match the texture's aspect ratio
            float width = height * (float)719 / (float)1000;

            // Scale the plane to match the calculated width and height
            background.transform.localScale = new Vector3(width/10, 1, height/10);
            //2.0756 1 2.8868 when the real distance is 20



           



            /*     float halfFOV = cameracomp.fieldOfView * 0.5f;
                 float distance = 2f;

                 float height = Mathf.Tan(Mathf.Deg2Rad * halfFOV) * distance * 2f;
                 float width = height * cameracomp.aspect;
               //  background.transform.rotation = Quaternion.Euler(90f, 180f, 0f);

                 background.transform.localScale = new Vector3(width, 1f, height);*/


            string imagePath = Path.Combine(PluginPath, "Pictures", "BG.png");
            byte[] fileData = File.ReadAllBytes(imagePath);

            if (fileData != null)
            {
                Texture2D spriteTexture = new Texture2D(1, 1); // You can adjust the size as needed
                spriteTexture.LoadImage(fileData);
                Sprite selectedBackground = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0.5f, 0.5f));
                Texture2D texture = selectedBackground.texture;
                // Check if background material is not null before setting its properties
                Material material = new Material(Shader.Find("Standard"));


                var materialsAB = AssetBundle.LoadFromFile(Path.Combine(PluginPath, "PureTextureShader"));
                Debug.LogWarning(materialsAB);
                var prefab = materialsAB.LoadAsset<UnityEngine.Shader>("Mobile-Lightmap-Unlit");
                Debug.LogWarning(prefab);
                

                material.mainTexture = texture;
                if (background.GetComponent<Renderer>() != null)
                {
                    background.GetComponent<Renderer>().material = material;
                    background.GetComponent<MeshRenderer>().material.shader = prefab;
                }
                else
                {
                    Debug.LogError("Renderer component on the background GameObject is null.");
                }
            }





            cameraObject.AddComponent<CaptureCameraView>().RenderAndSave();

       //     Destroy(cameraObject);
       //     Destroy(NJBJIIIACEP.OAAMGFLINOB[0].PCNHIIPBNEK[0]);
        }


        public class CaptureCameraView : MonoBehaviour
        {
            public Camera opaqueCamera; // Assign this in the inspector or dynamically
            public Camera transparentCamera;




            public string fileName = "CameraCapture";

            /*  void Start()
              {
                  if (captureCamera == null)
                  {
                      Debug.LogError("Capture Camera is not assigned.");
                      return;
                  }

                  // Capture the camera's view
                  RenderAndSave();
              }*/

            public void RenderAndSave()
            {
                int captureHeight = 1000;
                int captureWidth = (int)(captureHeight * (6.4 / 8.9));
                if (opaqueCamera == null) opaqueCamera = this.GetComponent<Camera>();
                if (transparentCamera == null)
                {
                    transparentCamera = GameObject.Instantiate(this.gameObject).GetComponent<Camera>();
                    
                }
                int transparentLayer = 10;

                // Set the opaque camera to render everything except the transparent layer
                opaqueCamera.cullingMask = ~(1 << transparentLayer);

                // Set the transparent camera to render only the transparent layer
                transparentCamera.cullingMask = (1 << transparentLayer);

                // Set up the opaque camera to clear with a transparent background
                opaqueCamera.clearFlags = CameraClearFlags.SolidColor;
                opaqueCamera.backgroundColor = new Color(0, 0, 0, 0);  // Transparent black

                // Set up the transparent camera to not clear anything (to blend over opaque camera)
                transparentCamera.clearFlags = CameraClearFlags.Nothing;

                // Create a RenderTexture for capturing the image
                RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24, RenderTextureFormat.ARGB32);
                opaqueCamera.targetTexture = rt;
                transparentCamera.targetTexture = rt;

                // Ensure transparency is preserved by using RGBA32 format
                Texture2D screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGBA32, false);




                // Render the transparent objects on top
          //      transparentCamera.Render();
                // Render the opaque objects first
                opaqueCamera.Render();



                // Read the pixels into a Texture2D
                RenderTexture.active = rt;
                screenShot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
                screenShot.Apply();

                // Clean up
                opaqueCamera.targetTexture = null;
                transparentCamera.targetTexture = null;
                RenderTexture.active = null;
                Destroy(rt);



       /*         string text = Path.Combine(PluginPath, "Pictures", "BG.png");
                if (File.Exists(text))
                {
                    byte[] array = File.ReadAllBytes(text);
                    if (array != null)
                    {
                        Texture2D texture2D = new Texture2D(1, 1);
                        ImageConversion.LoadImage(texture2D, array);
                        Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
                        Texture2D texture = sprite.texture;
                        screenShot = OverlayTextures(texture, screenShot);
                    }
                }*/


                string text2 = Path.Combine(PluginPath, "Pictures", "FoilOverlay.png");
                if (File.Exists(text2))
                {
                    byte[] array2 = File.ReadAllBytes(text2);
                    if (array2 != null)
                    {
                        Texture2D texture2D = new Texture2D(1, 1);
                        ImageConversion.LoadImage(texture2D, array2);
                        Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
                        Texture2D texture = sprite.texture;
                        screenShot = OverlayTextures(screenShot, texture);
                    }
                }



                // Encode the Texture2D into PNG format with transparency
                byte[] bytes = screenShot.EncodeToPNG();
                string filename = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH'-'mm'-'ss'-'fff") + ".jpg";
                string filePath = Path.Combine(PluginPath, "Pictures", filename);

                

                // Save the PNG file
                File.WriteAllBytes(filePath, bytes);
                Debug.LogWarning($"Saved image to {filePath}");
            }


        }
    }
}