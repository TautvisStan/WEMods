using UnityEngine;
using System.Diagnostics;
using System.IO;
using System;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; //Unity.RenderPipelines.Universal.Runtime.dll
namespace CameraRecording
{
    public class GameplayRecorder : MonoBehaviour
    {
        public Camera recordingCamera;
        private RenderTexture renderTexture;
        public bool isRecording = false;
        private Process ffmpegProcess;
        private string saveFolder;

       // public GameplayRecorder test = null;

        public void Setup()
        {
            // Create and setup the recording camera
            UnityEngine.Debug.LogWarning("0");
            GameObject cameraObject = this.gameObject;
            UnityEngine.Debug.LogWarning("1");
            
            UnityEngine.Debug.LogWarning("TESTING CAM");
            recordingCamera = GetComponent<Camera>();

            if (Plugin.Mode.Value == "FreeCam")
            {
                UnityEngine.Debug.LogWarning("USING FREE CAMERA");

                UniversalAdditionalCameraData mainCamData = Camera.main.GetUniversalAdditionalCameraData();

                mainCamData.cameraStack.Add(recordingCamera);
                recordingCamera.targetDisplay = 0;
                recordingCamera.depth = -3;

                renderTexture = new RenderTexture(Plugin.width.Value, Plugin.height.Value, 24);

                recordingCamera.targetTexture = renderTexture;
                cameraObject.tag = "Untagged";
                UnityEngine.Debug.LogWarning("CAMERA ADDED");
            }
            else
            {
                UnityEngine.Debug.LogWarning("CAM IS EXISTING");
                // UnityEngine.Debug.LogWarning(recordingCamera.targetTexture == null);
                // renderTexture = recordingCamera.targetTexture;
                UnityEngine.Debug.LogWarning(recordingCamera.pixelWidth);
                UnityEngine.Debug.LogWarning(recordingCamera.pixelHeight);
                renderTexture = new RenderTexture(Plugin.width.Value, Plugin.height.Value, 24);

                UnityEngine.Debug.LogWarning($"{renderTexture.width} x {renderTexture.height}");
                Plugin.width.Value = renderTexture.width;
                Plugin.height.Value = renderTexture.height;
            }

            // Setup camera properties


            // Create render texture


            // Set save folder
            saveFolder = Path.Combine(Plugin.PluginPath, "recordings");
            UnityEngine.Debug.LogWarning(Plugin.PluginPath);
            Directory.CreateDirectory(saveFolder);
            UnityEngine.Debug.LogWarning(saveFolder);

        }

        public void StartRecording()
        {
            if (isRecording) return;

            string outputPath = Path.Combine(saveFolder, $"GameplayRecording_{DateTime.Now:yyyyMMdd_HHmmss}_{UnityEngine.Random.Range(0,100)}.mp4");
            string ffmpegPath = Path.Combine(Plugin.PluginPath, "ffmpeg.exe");

            string args = $"-y -f rawvideo -vcodec rawvideo -pixel_format rgb24 " +
                 $"-video_size {Plugin.width.Value}x{Plugin.height.Value} -framerate {Plugin.frameRate.Value} -i pipe:0 " +
                 $"-c:v libx264 -preset ultrafast " +  // Faster encoding
                 $"-crf {Plugin.crf.Value} " +                        // Constant Rate Factor (18-28 is good range, lower = better quality)
                 $"-tune zerolatency " +              // Reduces encoding latency
                 $"-profile:v high " +                // High profile for better compression
                 $"-level:v 4.2 " +                   // Compatibility level
                 $"-pix_fmt yuv420p " +
                 $"-vf \"vflip\" " +
                 $"-threads 0 " +                     // Use optimal number of threads
                 $"-vsync cfr " +
                 $"-r {Plugin.frameRate.Value} " +          // Add this line
                 $"\"{outputPath}\"";

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
            float frameInterval = 1f / Plugin.frameRate.Value; // Time between frames
            float recordingStartTime = Time.realtimeSinceStartup;
            int frameCount = 0;

            while (isRecording)
            {
                yield return new WaitForEndOfFrame();

                float currentTime = Time.realtimeSinceStartup;

                // Calculate the expected time for the current frame
                float expectedTime = recordingStartTime + frameCount * frameInterval;

                if (currentTime >= expectedTime)
                {
                    // Request an asynchronous readback
                    if (Plugin.Mode.Value == "MainCam")
                    {
                        Graphics.Blit(null, renderTexture); // Copy to your texture
                    }
                    AsyncGPUReadback.Request(renderTexture, 0, TextureFormat.RGB24, OnCompleteReadback);

                    // Increment frame count
                    frameCount++;
                }
            }
        }

        private void OnCompleteReadback(AsyncGPUReadbackRequest request)
        {
            if (!isRecording)
            {
                // Recording has stopped; ignore this callback.
                return;
            }

            if (request.hasError)
            {
                UnityEngine.Debug.LogError("GPU readback error detected.");
                return;
            }

            // Check if ffmpegProcess is still valid
            if (ffmpegProcess == null || ffmpegProcess.HasExited)
            {
                UnityEngine.Debug.LogWarning("FFmpeg process is not available or has exited.");
                return;
            }

            // Get the data
            byte[] frameData = request.GetData<byte>().ToArray();

            // Write frame data to FFmpeg
            try
            {
                ffmpegProcess.StandardInput.BaseStream.Write(frameData, 0, frameData.Length);
                ffmpegProcess.StandardInput.BaseStream.Flush();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Exception writing to FFmpeg process: {e.Message}");
            }
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