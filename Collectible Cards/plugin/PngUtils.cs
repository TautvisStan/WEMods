﻿using Hjg.Pngcs;
using Hjg.Pngcs.Chunks;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CollectibleCards2
{
    public static class PngUtils
    {
        public static Dictionary<string, string> GetFullMetadata(string file, Dictionary<string, string> metadataRequest)
        {
            PngReader pngr = FileHelper.CreatePngReader(file);
            //pngr.MaxTotalBytesRead = 1024 * 1024 * 1024L * 3; // 3Gb!
            //pngr.ReadSkippingAllRows();
            Dictionary<string, string> metadata = new();
            PngMetadata pngmeta = pngr.GetMetadata();
            foreach (string key in metadataRequest.Keys)
            {
                metadata[key] = pngmeta.GetTxtForKey(key);
            }
            pngr.End();
            return metadata;
        }
        public static Dictionary<string, string> GetCardMetadata(string file)
        {
            PngReader pngr = FileHelper.CreatePngReader(file);
            PngMetadata pngmeta = pngr.GetMetadata();

            var metadata = new Dictionary<string, string>
            {
                { "CharID", pngmeta.GetTxtForKey("CharID") },
                { "Name", pngmeta.GetTxtForKey("Name") },
                { "FedName", pngmeta.GetTxtForKey("FedName") },
                { "Border", pngmeta.GetTxtForKey("Border") },
                { "Foil", pngmeta.GetTxtForKey("Foil") },
                { "Signature", pngmeta.GetTxtForKey("Signature") },
                { "CustomGenerated", pngmeta.GetTxtForKey("CustomGenerated") },
                { "Popularity", pngmeta.GetTxtForKey("Popularity") },
                { "Strength", pngmeta.GetTxtForKey("Strength") },
                { "Skill", pngmeta.GetTxtForKey("Skill") },
                { "Agility", pngmeta.GetTxtForKey("Agility") },
                { "Stamina", pngmeta.GetTxtForKey("Stamina") },
                { "Attitude", pngmeta.GetTxtForKey("Attitude") },
                { "FrontFinisher", pngmeta.GetTxtForKey("FrontFinisher") },
                { "BackFinisher", pngmeta.GetTxtForKey("BackFinisher") },
                { "CharData", pngmeta.GetTxtForKey("CharData") }
            };

            pngr.End();
            return metadata;
        }
        public static string GetMetadata(string file, string key)
        {
            PngReader pngr = FileHelper.CreatePngReader(file);
            //pngr.MaxTotalBytesRead = 1024 * 1024 * 1024L * 3; // 3Gb!
            //pngr.ReadSkippingAllRows();
            string data = pngr.GetMetadata().GetTxtForKey(key);
            pngr.End();
            return data;
        }
        public static void SaveWithMetadata(string filename, byte[] bytes, Dictionary<string, string> data)
        {
            PngReader pngr = new(new MemoryStream(bytes));
            PngWriter pngw = FileHelper.CreatePngWriter(filename, pngr.ImgInfo, true); // idem
            //Console.WriteLine(pngr.ToString()); // just information
            int chunkBehav = ChunkCopyBehaviour.COPY_ALL_SAFE; // tell to copy all 'safe' chunks
            pngw.CopyChunksFirst(pngr, chunkBehav);          // copy some metadata from reader 
            foreach (string key in data.Keys)
            {
                PngChunk chunk = pngw.GetMetadata().SetText(key, data[key]);
                chunk.Priority = true;
            }

            int channels = pngr.ImgInfo.Channels;
            if (channels < 3)
                throw new Exception("This example works only with RGB/RGBA images");
            for (int row = 0; row < pngr.ImgInfo.Rows; row++)
            {
                ImageLine l1 = pngr.ReadRowInt(row); // format: RGBRGB... or RGBARGBA...
                pngw.WriteRow(l1, row);
            }
            pngw.CopyChunksLast(pngr, chunkBehav); // metadata after the image pixels? can happen
            pngw.End(); // dont forget this
            pngr.End();

        }
        public static void AddMetadata(string origFilename, byte[] bytes, string key, string value)
        {
            Dictionary<string, string> data = new()
            {
                { key, value }
            };
            SaveWithMetadata(origFilename, bytes, data);
        }
        public static Texture2D ResizeTexture(Texture2D original, int width, int height)
        {
            RenderTexture rt = RenderTexture.GetTemporary(width, height);
            RenderTexture.active = rt;
            Graphics.Blit(original, rt);
            Texture2D resized = new(width, height);
            resized.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            resized.Apply();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
            return resized;
        }

    }

}
