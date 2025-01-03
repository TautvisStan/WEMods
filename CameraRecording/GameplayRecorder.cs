using UnityEngine;
using System.Diagnostics;
using System.IO;
using System;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;

namespace CameraRecording
{
    public class GameplayRecorder : MonoBehaviour
    {
        private Camera recordingCamera;
        private RenderTexture renderTexture;
        private bool isRecording = false;
        private Process ffmpegProcess;
        private string saveFolder;

        // Adjust these settings as needed
        private readonly int width = 1920/4;
        private readonly int height = 1080/4;
        private readonly int frameRate = 30;

        void Awake()
        {
            // Create and setup the recording camera
             GameObject cameraObject = this.gameObject;
             recordingCamera = (!cameraObject.TryGetComponent<Camera>(out recordingCamera)) ? cameraObject.AddComponent<Camera>() : recordingCamera;




            /*
             //Log(CameraRecording.GameplayRecorder);
//CameraRecording.GameplayRecorder test = Camera.main.gameObject.AddComponent<CameraRecording.GameplayRecorder>();
            
test.StartRecording();
             */


            //
            //GameObject testobj = GameObject.Instantiate(Camera.main.gameObject);
            // Destroy(test.GetComponent<Camera>());
            //CameraRecording.GameplayRecorder test = testobj.AddComponent<CameraRecording.GameplayRecorder>();
            //
            //  test.StartRecording();



            // Setup camera properties
            recordingCamera.targetDisplay = 0;
            recordingCamera.depth = 0;

            // Create render texture
            renderTexture = new RenderTexture(width, height, 24);
            recordingCamera.targetTexture = renderTexture;

            // Set save folder
            saveFolder = Path.Combine(Plugin.PluginPath, "Recordings");
            UnityEngine.Debug.LogWarning(Plugin.PluginPath);
            UnityEngine.Debug.LogWarning(Plugin.PluginPath);
            UnityEngine.Debug.LogWarning(saveFolder);
            Directory.CreateDirectory(saveFolder);
            UnityEngine.Debug.LogWarning(saveFolder);
        }

        public void StartRecording()
        {
            if (isRecording) return;

            string outputPath = Path.Combine(saveFolder, $"GameplayRecording_{DateTime.Now:yyyyMMdd_HHmmss}.mp4");
            string ffmpegPath = Path.Combine(Plugin.PluginPath, "ffmpeg.exe");

            // FFmpeg command to receive raw frames and encode them
            string args = $"-y -f rawvideo -vcodec rawvideo -pixel_format rgb24 -video_size {width}x{height} " +
                         $"-framerate {frameRate} -i pipe:0 -c:v libx264 -preset ultrafast -vf \"vflip\" -pix_fmt yuv420p \"{outputPath}\"";

            ffmpegProcess = new Process();
            ffmpegProcess.StartInfo.FileName = ffmpegPath;
            ffmpegProcess.StartInfo.Arguments = args;
            ffmpegProcess.StartInfo.UseShellExecute = false;
            ffmpegProcess.StartInfo.RedirectStandardInput = true;
            ffmpegProcess.StartInfo.CreateNoWindow = true;

            ffmpegProcess.Start();

            isRecording = true;
            StartCoroutine(CaptureFrames());
        }

        private IEnumerator CaptureFrames()
        {
            Texture2D frameTexture = new Texture2D(width, height, TextureFormat.RGB24, false);
            byte[] frameData = new byte[width * height * 3];

            while (isRecording)
            {
                yield return new WaitForEndOfFrame();

                // Read the render texture
                RenderTexture.active = renderTexture;
                frameTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                frameTexture.Apply();

                // Get raw pixel data
                frameData = frameTexture.GetRawTextureData();

                // Write frame data to FFmpeg
                ffmpegProcess.StandardInput.BaseStream.Write(frameData, 0, frameData.Length);
            }

            Destroy(frameTexture);
        }

        public void StopRecording()
        {
            if (!isRecording) return;

            isRecording = false;

            // Close FFmpeg process
            ffmpegProcess.StandardInput.Close();
            ffmpegProcess.WaitForExit();
            ffmpegProcess.Close();
        }

        void OnDestroy()
        {
            if (isRecording)
                StopRecording();

            if (renderTexture != null)
                renderTexture.Release();
        }
    }
}