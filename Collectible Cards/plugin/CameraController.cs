using System.IO;
using System;
using UnityEngine;

namespace CollectibleCards2
{
    public static class CameraController
    {
        public static GameObject CameraObj { get; set; } = null;
        public static void SetupCamera(string[] setup)
        {
            // Setup hard camera
            CameraObj = new GameObject("CardCamera");

            // Attach the camera component to the new GameObject
            CameraObj.AddComponent<Camera>();

            // Set the position, rotation, or any other properties as needed
            CameraObj.transform.position = new Vector3(0f, 8f, 30f);
            Vector3 targetPosition = new(0f, 8f, 0f);
            CameraObj.transform.LookAt(targetPosition);
            CameraObj.GetComponent<Camera>().orthographic = true;
            CameraObj.GetComponent<Camera>().orthographicSize = 10;
            foreach (string line in setup)
            {
                if (line.Trim().Length == 0)
                {
                    continue;
                }
                if (line.ToLower() == "perspective")
                {
                    CameraObj.GetComponent<Camera>().orthographic = false;
                }
            }
        }
        public static void Cleanup()
        {
            UnityEngine.Object.Destroy(CameraObj);
            CameraObj = null;
        }
        public static byte[] CaptureScreenshot()
        {
            Camera camera = CameraObj.GetComponent<Camera>();
            int captureHeight = Plugin.CardHeight;
            int captureWidth = Plugin.CardWidth;
            RenderTexture rt = new(captureWidth, captureHeight, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new(captureWidth, captureHeight, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
            screenShot.Apply();
            camera.targetTexture = null;
            RenderTexture.active = null;
            UnityEngine.Object.Destroy(rt);
            // Encode the Texture2D into PNG format with transparency
            byte[] bytes = screenShot.EncodeToPNG();
            UnityEngine.Object.Destroy(screenShot);
            return bytes;
        }
    }

}
