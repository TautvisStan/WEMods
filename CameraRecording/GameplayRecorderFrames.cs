using UnityEngine;
using System.Diagnostics;
using System.IO;
using System;
using System.Collections;
using UnityEngine.Rendering;
using System.Collections.Concurrent;
using System.Threading;
using Unity.Collections;

namespace CameraRecording
{
    public class GameplayRecorderFrames : MonoBehaviour
    {
        public Camera recordingCamera;
        private RenderTexture renderTexture;
        private bool isRecording = false;
        private Process ffmpegProcess;
        private string saveFolder;

        public GameplayRecorderFrames test = null;
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
                test = testobj.AddComponent<CameraRecording.GameplayRecorderFrames>();
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

            // Initialize recording variables if necessary
            frameQueue = new ConcurrentQueue<byte[]>();
            StartFrameWriter();

            isRecording = true;
            StartCoroutine(CaptureFrames());
        }

        public void StopRecording()
        {
            if (!isRecording) return;

            isRecording = false;
            StopCoroutine(CaptureFrames());
            StopFrameWriter();

            // Start the conversion process
            ConvertImageSequenceToVideo();
        }

        private IEnumerator CaptureFrames()
        {
            int frameCount = 0;
            float frameInterval = 1f / Plugin.frameRate.Value; // Time between frames
            float nextFrameTime = Time.time;

            while (isRecording)
            {
                yield return new WaitForEndOfFrame();

                if (Time.time >= nextFrameTime)
                {
                    // Request an asynchronous readback
                    AsyncGPUReadback.Request(renderTexture, 0, TextureFormat.RGB24, request => OnCompleteReadback(request, frameCount));
                    frameCount++;

                    // Calculate next frame time
                    nextFrameTime += frameInterval;

                    // If we're falling behind, catch up to prevent slow-motion
                    if (nextFrameTime < Time.time)
                    {
                        nextFrameTime = Time.time + frameInterval;
                    }
                }
            }
        }

        private void OnCompleteReadback(AsyncGPUReadbackRequest request, int frameCount)
        {
            if (request.hasError)
            {
                UnityEngine.Debug.LogError("GPU readback error detected.");
                return;
            }

            // Get the data
            NativeArray<byte> data = request.GetData<byte>();

            // Create Texture2D from NativeArray
            Texture2D frameTexture = new Texture2D(Plugin.width.Value, Plugin.height.Value, TextureFormat.RGB24, false);
            frameTexture.LoadRawTextureData(data);
            frameTexture.Apply();

            // Encode to PNG/JPG
            byte[] bytes = frameTexture.EncodeToPNG(); // Or EncodeToJPG()
            Destroy(frameTexture);

            // Enqueue for writing
            frameQueue.Enqueue(bytes);
        }

        private ConcurrentQueue<byte[]> frameQueue = new ConcurrentQueue<byte[]>();
        private bool isWritingFrames = false;

        private void StartFrameWriter()
        {
            isWritingFrames = true;
            ThreadPool.QueueUserWorkItem(FrameWriterThread);
        }

        private void StopFrameWriter()
        {
            isWritingFrames = false;
        }

        private void FrameWriterThread(object state)
        {
            int frameCount = 0;
            while (isWritingFrames || !frameQueue.IsEmpty)
            {
                if (frameQueue.TryDequeue(out byte[] frameData))
                {
                    string frameFileName = Path.Combine(saveFolder, $"frame_{frameCount:D06}.png");
                    File.WriteAllBytes(frameFileName, frameData);
                    frameCount++;
                }
                else
                {
                    Thread.Sleep(1); // Avoid tight loop if queue is temporarily empty
                }
            }
        }

        private void CaptureFrameToFile()
        {
            // Read the render texture
            RenderTexture.active = renderTexture;
            Texture2D frameTexture = new Texture2D(Plugin.width.Value, Plugin.height.Value, TextureFormat.RGB24, false);

            frameTexture.ReadPixels(new Rect(0, 0, Plugin.width.Value, Plugin.height.Value), 0, 0);
            frameTexture.Apply();

            // Encode texture into PNG/JPG
            byte[] bytes = frameTexture.EncodeToPNG(); // Or EncodeToJPG()
            Destroy(frameTexture);

            // Enqueue the frame data
            frameQueue.Enqueue(bytes);
        }
        private void ConvertImageSequenceToVideo()
        {
            string outputPath = Path.Combine(saveFolder, $"GameplayRecording_{DateTime.Now:yyyyMMdd_HHmmss}.mp4");
            string ffmpegPath = Path.Combine(Plugin.PluginPath, "ffmpeg.exe");

            // Adjust the pattern to match your filename format
            string imagePattern = Path.Combine(saveFolder, "frame_%06d.png");

            string args = $"-y -framerate {Plugin.frameRate.Value} -i \"{imagePattern}\" " +
                          $"-c:v libx264 -preset veryfast -crf {Plugin.crf.Value} " +
                          $"-pix_fmt yuv420p \"{outputPath}\"";

            Process ffmpegProcess = new Process();
            ffmpegProcess.StartInfo.FileName = ffmpegPath;
            ffmpegProcess.StartInfo.Arguments = args;
            ffmpegProcess.StartInfo.UseShellExecute = false;
            ffmpegProcess.StartInfo.CreateNoWindow = true;

            ffmpegProcess.Start();
            ffmpegProcess.WaitForExit();

            // Optionally, delete the image files after encoding
            DeleteImageSequence();
        }

        private void DeleteImageSequence()
        {
            string[] files = Directory.GetFiles(saveFolder, "frame_*.png");
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        private void OnCompleteReadback(AsyncGPUReadbackRequest request)
        {
            if (request.hasError)
            {
                UnityEngine.Debug.LogError("GPU readback error detected.");
                return;
            }

            // Get the data
            byte[] frameData = request.GetData<byte>().ToArray();

            // Write frame data to FFmpeg
            ffmpegProcess.StandardInput.BaseStream.Write(frameData, 0, frameData.Length);
            ffmpegProcess.StandardInput.BaseStream.Flush();
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