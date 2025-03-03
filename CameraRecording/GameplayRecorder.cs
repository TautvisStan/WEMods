using UnityEngine;
using System.Diagnostics;
using System.IO;
using System;
using System.Collections;
using UnityEngine.Rendering;
namespace CameraRecording
{
    public class GameplayRecorder : MonoBehaviour
    {
        
        public Camera recordingCamera;
        private RenderTexture renderTexture;
        public bool isRecording = false;
        private Process ffmpegProcess;
        private string saveFolder;
        public int camNumber;
        public int frameDelay;

        public void Setup()
        {
            // Create and setup the recording camera
            GameObject cameraObject = this.gameObject;
            recordingCamera = GetComponent<Camera>();
            if (Plugin.Mode.Value == "FreeCam")
            {
                recordingCamera.targetDisplay = 0;
                recordingCamera.depth = -3;
                renderTexture = new RenderTexture(Plugin.width.Value, Plugin.height.Value, 24);
                recordingCamera.targetTexture = renderTexture;
                cameraObject.tag = "Untagged";
            }
            else
            {
                renderTexture = new RenderTexture(recordingCamera.pixelWidth, recordingCamera.pixelHeight, 24);
            }
            // Set save folder
            saveFolder = Path.Combine(Plugin.PluginPath, "recordings");
            Directory.CreateDirectory(saveFolder);
        }

        public void StartRecording()
        {
            if (isRecording) return;

            string outputPath = Path.Combine(saveFolder, $"GameplayRecording_{DateTime.Now:yyyyMMdd_HHmmss}_{camNumber}.mp4");
            string ffmpegPath = Path.Combine(Plugin.PluginPath, "ffmpeg.exe");
            string V = (Plugin.Mode.Value == "FreeCam") ? $"-vf \"vflip\" " : "";
            string W = (Plugin.Mode.Value == "FreeCam") ? $"{Plugin.width.Value}" : $"{recordingCamera.pixelWidth}";
            string H = (Plugin.Mode.Value == "FreeCam") ? $"{Plugin.height.Value}" : $"{recordingCamera.pixelHeight}";
            string args = $"-y -f rawvideo -vcodec rawvideo -pixel_format rgb24 " +
                 $"-video_size {W}x{H} -framerate {Plugin.frameRate.Value} -i pipe:0 " +
                 $"-c:v libx264 -preset ultrafast " +  // Faster encoding
                 $"-crf {Plugin.crf.Value} " +                        // Constant Rate Factor (18-28 is good range, lower = better quality)
                 $"-tune zerolatency " +              // Reduces encoding latency
                 $"-profile:v high " +                // High profile for better compression
                 $"-level:v 4.2 " +                   // Compatibility level
                 $"-pix_fmt yuv420p " +
                 $"{V}" +
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
            if (Plugin.Mode.Value == "FreeCam" && Plugin.freecamSmoothering.Value == true)
            {
                int delayMult = camNumber == 0 ? 9 : camNumber - 1;
                recordingStartTime += frameInterval / frameDelay * delayMult;
            }
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
                        if(Plugin.maincamVisibleUI.Value == false) //TODO fix
                        {
                            UnityEngine.Debug.LogWarning("TeST");
                            int originalMask = recordingCamera.cullingMask;
                            recordingCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("UI")); // Exclude UI
                            Canvas canvas = LIPNHOMGGHF.JPABICKOAEO.GetComponent<Canvas>();
                            canvas.worldCamera = recordingCamera;
                            canvas.renderMode = RenderMode.ScreenSpaceCamera;
                            recordingCamera.RenderDontRestore();
                            Graphics.Blit(null, renderTexture); // Capture UI-less scene
                            recordingCamera.cullingMask = originalMask; // Restore original mask
                            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                        }    
                        else
                        {
                            Graphics.Blit(null, renderTexture); // Copy to your texture
                        }

                    }
                    else
                    {
                        recordingCamera.Render();
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
                Plugin.Log.LogError("GPU readback error detected.");
                return;
            }

            // Check if ffmpegProcess is still valid
            if (ffmpegProcess == null || ffmpegProcess.HasExited)
            {
                Plugin.Log.LogWarning("FFmpeg process is not available or has exited.");
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
                Plugin.Log.LogError($"Exception writing to FFmpeg process: {e.Message}");
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
            renderTexture.Release();
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