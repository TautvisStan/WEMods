using System.IO;
using UnityEngine;

namespace CollectibleCards2
{
    public static class Background
    {
        public static GameObject BackgroundObj { get; set; } = null;
        public static Shader ShaderPrefab { get; set; }
        public static void GetShaderPrefab()
        {
            var assetBundle = AssetBundle.LoadFromFile(Path.Combine(Plugin.PluginPath, "prefabs", "cardbgshader"));
            ShaderPrefab = assetBundle.LoadAsset<Shader>("CardBGShader");
        }
        public static void SetupBackground(GameObject cameraObject, string preset)
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
                float aspectRatio = Plugin.CardWidth / (float)Plugin.CardHeight;
                Vector3 planeScale = new(aspectRatio * orthographicHeight / 5, 1, orthographicHeight / 5);

                // Apply the calculated scale to the background plane
                BackgroundObj.transform.localScale = planeScale;


            }
            else
            {
                float distance = Mathf.Abs(cameraObject.transform.position.z - BackgroundObj.transform.position.z);

                // Calculate the height in world units based on the camera's FOV
                float height = 2.0f * distance * Mathf.Tan(cameraObject.GetComponent<Camera>().fieldOfView * 0.5f * Mathf.Deg2Rad);

                // Calculate the width in world units to match the texture's aspect ratio
                float width = height * Plugin.CardWidth / Plugin.CardHeight;

                // Scale the plane to match the calculated width and height
                BackgroundObj.transform.localScale = new Vector3(width / 10, 1, height / 10);
                //2.0756 1 2.8868 when the real distance is 20
            }

            string imagePath = Path.Combine(Plugin.PluginPath, preset, "BG.png");
            byte[] fileData = File.ReadAllBytes(imagePath);

            if (fileData != null)
            {
                Texture2D spriteTexture = new(1, 1); // You can adjust the size as needed
                spriteTexture.LoadImage(fileData);
                Sprite selectedBackground = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0.5f, 0.5f));
                Texture2D texture = selectedBackground.texture;
                // Check if background material is not null before setting its properties
                Material material = new(ShaderPrefab)
                {
                    mainTexture = texture
                };
                BackgroundObj.GetComponent<Renderer>().material = material;

                previousMaterial = material;
                previousTexture = spriteTexture;
            }
        }
        private static Material previousMaterial;
        private static Texture2D previousTexture;
        public static void Cleanup()
        {
            GameObject.Destroy(BackgroundObj);
            BackgroundObj = null;
            if (previousMaterial != null)
            {
                GameObject.Destroy(previousMaterial);
                previousMaterial = null;
            }

            if (previousTexture != null)
            {
                GameObject.Destroy(previousTexture);
                previousTexture = null;
            }
        }
    }
}
