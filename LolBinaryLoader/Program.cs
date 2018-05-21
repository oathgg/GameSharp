// Credits to Rockzz: https://www.unknowncheats.me/forum/2014518-post1606.html.

using System;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Diagnostics;

namespace LoLBinaryLoader
{
    class Program
    {
        const string strBaseURL = "http://l3cdn.riotgames.com/releases/live/projects/lol_game_client/releases/";
        const string strListFileName = @"releaselisting_EUW";
        static string targetPathBase = @"D:\Dissassembly\League of Legends\";

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Load!");
            WebClient wc = new WebClient();
            try
            {
                wc.DownloadFile(new Uri(strBaseURL + strListFileName), strListFileName);
            }
            catch (WebException ex)
            {
                Console.WriteLine("ListNotFound:" + strBaseURL + strListFileName);
                return;
            }

            string line;
            StreamReader file = new StreamReader(strListFileName);
            while ((line = file.ReadLine()) != null)
            {
                // Skip any blanc lines
                if (line == "")
                    continue;

                // version URL for compressed league file
                string strUrl = strBaseURL + line + "/files/League of Legends.exe.compressed";
                string targetPath = $@"{targetPathBase}{line}";
                Directory.CreateDirectory(targetPath);
                string compressedFileName = $@"{targetPath}\League of Legends.exe.compressed";

                Console.WriteLine("Downloading file:" + strUrl + "->" + compressedFileName);
                try
                {
                    wc.DownloadFile(new Uri(strUrl), compressedFileName);
                }
                catch (WebException ex)
                {
                    Console.WriteLine("No File In Release:" + strUrl);
                    // Delete the directory if we created it and any files in the directory.
                    if (Directory.Exists(targetPath))
                        Directory.Delete(targetPath, true);
                    continue;
                }

                if (!File.Exists(compressedFileName))
                {
                    Console.WriteLine("FileNotFound:" + compressedFileName);
                    continue;
                }
                byte[] fileInBytes = File.ReadAllBytes(compressedFileName);
                if (fileInBytes[0] != 0x78 || fileInBytes[1] != 0x9C)
                {
                    Console.WriteLine("File:" + compressedFileName + "-HeaderInvalid:" + fileInBytes[0].ToString() + "," + fileInBytes[1].ToString());
                    continue;
                }
                // We skip the first 2 bytes as these are only needed for the header.
                Stream byteStreamOriginal = new MemoryStream(fileInBytes, 2, fileInBytes.Length - 2);
                using (DeflateStream decompressionStream = new DeflateStream(byteStreamOriginal, CompressionMode.Decompress))
                {
                    string currentFileName = compressedFileName;
                    string newFileName = currentFileName.Remove(currentFileName.Length - ".compressed".Length);
                    using (FileStream decompressedFileStream = File.Create(newFileName))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", compressedFileName);
                    }

                    //Delete Compressed File!
                    File.Delete(compressedFileName);
                    FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(newFileName);
                    string newPathName = targetPath.Replace(line, fileVersionInfo.ProductVersion);
                    // If we have an exe file we rename the folder to that version exe, otherwise delete the folder.
                    if (File.Exists(newFileName))
                    {
                        // Check if we haven't already downloaded and renamed this folder before
                        if (!Directory.Exists(newPathName))
                            Directory.Move(targetPath, newPathName);
                        else
                        {
                            // We remove it if we already have a folder with that name as this will mean we have 
                            //  downloaded it before and we don't want duplicates.
                            Directory.Delete(targetPath, true);
                            // We also want to stop the program.
                            Environment.Exit(0);
                        }
                    }
                }
            }
            file.Close();
        }
    }
}