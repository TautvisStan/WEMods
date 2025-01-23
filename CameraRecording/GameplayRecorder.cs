using UnityEngine;
using System.Diagnostics;
using System.IO;
using System;
using System.Collections;

namespace CameraRecording
{
    public class GameplayRecorder : MonoBehaviour
    {
        public Camera recordingCamera;
        private RenderTexture renderTexture;
        private bool isRecording = false;
        private Process ffmpegProcess;
        private string saveFolder;

        public GameplayRecorder test = null;
        public void ToggleRecording()
        {
            if (!isRecording)
            {
                GameObject testobj = GameObject.Instantiate(Camera.main.gameObject);

                GameObject.Destroy(testobj.GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessLayer>());
                GameObject.Destroy(testobj.GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessVolume>());
                GameObject.Destroy(testobj.GetComponent<AudioListener>());
             /*   Camera testcam;
                UnityEngine.Debug.LogWarning(testobj.TryGetComponent<Camera>(out testcam));
                GameObject.Destroy(testcam);
                UnityEngine.Debug.LogWarning(testobj.TryGetComponent<Camera>(out testcam));*/
                test = testobj.AddComponent<CameraRecording.GameplayRecorder>();
                test.Setup();
                test.StartRecording();
                isRecording = true;
            }
            else
            {
                test.StopRecording();
                Destroy(test.gameObject);
                Destroy(this);
            }
        }
        public void Setup()
        {
            // Create and setup the recording camera
             GameObject cameraObject = this.gameObject;
            cameraObject.tag = "Untagged";
            UnityEngine.Debug.LogWarning("TESTING CAM");
            if (!cameraObject.TryGetComponent<Camera>(out recordingCamera))
            {
                UnityEngine.Debug.LogWarning("ADDING CAMERA");
                recordingCamera = cameraObject.AddComponent<Camera>();
            }
            else
            {
                UnityEngine.Debug.LogWarning("CAM IS EXISTING");
            }
        //    recordingCamera.pixelRect = new Rect(0, 0, Plugin.width.Value, Plugin.height.Value);
                
             //recordingCamera = (!cameraObject.TryGetComponent<Camera>(out recordingCamera)) ? cameraObject.AddComponent<Camera>() : recordingCamera;




            /*
             //Log(CameraRecording.GameplayRecorder);
//CameraRecording.GameplayRecorder test = Camera.main.gameObject.AddComponent<CameraRecording.GameplayRecorder>();
            
test.StartRecording();
             */


            //
            //GameObject test = GameObject.Instantiate(Camera.main.gameObject);
            // GameObject.Destroy(test.GetComponent<Camera>());
            //CameraRecording.GameplayRecorder test = testobj.AddComponent<CameraRecording.GameplayRecorder>();
            //
            //  test.StartRecording();



            // Setup camera properties
            recordingCamera.targetDisplay = 0;
            recordingCamera.depth = -1;

            // Create render texture
            renderTexture = new RenderTexture(Plugin.width.Value, Plugin.height.Value, 24);

            recordingCamera.targetTexture = renderTexture;

            // Set save folder
            saveFolder = Path.Combine(Plugin.PluginPath, "recordings");
            UnityEngine.Debug.LogWarning(Plugin.PluginPath);
            Directory.CreateDirectory(saveFolder);
            UnityEngine.Debug.LogWarning(saveFolder);

        }

        public void StartRecording()
        {
            if (isRecording) return;

            
            string outputPath = Path.Combine(saveFolder, $"GameplayRecording_{DateTime.Now:yyyyMMdd_HHmmss}_{UnityEngine.Random.Range(0, 99)}.mp4");
            string ffmpegPath = Path.Combine(Plugin.PluginPath, "ffmpeg.exe");

            // FFmpeg command to receive raw frames and encode them
           //     string args = $"-y -f rawvideo -vcodec rawvideo -pixel_format rgb24 -video_size {width}x{height} " +
           //                  $"-framerate {frameRate} -i pipe:0 -c:v libx264 -preset ultrafast -vf \"vflip\" -pix_fmt yuv420p \"{outputPath}\"";
            
            string args = $"-y -f rawvideo -vcodec rawvideo -pixel_format rgb24 " +
                 $"-video_size {Plugin.width.Value}x{Plugin.height.Value} -framerate {Plugin.frameRate.Value} -i pipe:0 " +
                 $"-c:v libx264 -preset ultrafast " +  // Faster encoding
                 $"-crf {Plugin.crf.Value} " +                        // Constant Rate Factor (18-28 is good range, lower = better quality)
                 //   $"-b:v 5M " +

                 $"-tune zerolatency " +              // Reduces encoding latency
                 $"-profile:v high " +                // High profile for better compression
                 $"-level:v 4.2 " +                   // Compatibility level
                 $"-pix_fmt yuv420p " +
                 $"-vf \"vflip\" " +
                 $"-threads 0 " +                     // Use optimal number of threads
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
            Texture2D frameTexture = new Texture2D(Plugin.width.Value, Plugin.height.Value, TextureFormat.RGB24, false);
            byte[] frameData = new byte[Plugin.width.Value * Plugin.height.Value * 3];

            float frameInterval = 1f / Plugin.frameRate.Value; // Time between frames
            float nextFrameTime = Time.time;

            while (isRecording)
            {
                yield return new WaitForEndOfFrame();

                if (Time.time >= nextFrameTime)
                {
                    // Read the render texture
                    RenderTexture.active = renderTexture;
                    frameTexture.ReadPixels(new Rect(0, 0, Plugin.width.Value, Plugin.height.Value), 0, 0);
                    frameTexture.Apply();

                    // Get raw pixel data
                    frameData = frameTexture.GetRawTextureData();

                    // Write frame data to FFmpeg
                    ffmpegProcess.StandardInput.BaseStream.Write(frameData, 0, frameData.Length);

                    // Calculate next frame time
                    nextFrameTime += frameInterval;

                    // If we're falling behind, catch up to prevent slow-motion
                    if (nextFrameTime < Time.time)
                    {
                        nextFrameTime = Time.time + frameInterval;
                    }
                }
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