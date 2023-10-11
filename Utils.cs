using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace VideoAudioTransfer{
    public class Utils{
        public async static Task<Image> GenerateThumbnailAsync(string videoPath)
        {
            var cts = new CancellationTokenSource();
            var task = Task.Run(() => GenerateThumbnail(videoPath));
            
            // Wait for either the task to complete or the timeout
            var completedTask = await Task.WhenAny(task, Task.Delay(3000, cts.Token));

            // If the task completed successfully, return the result
            if (completedTask == task && !cts.IsCancellationRequested)
            {
                cts.Cancel();
                return task.Result;
            }
            else
            {
                // Task didn't complete within the timeout
                cts.Cancel();
                Form1.WriteToLog("[HARMLESS] Thumbnail generation timed out. ");
                return null; // or return a default image, or throw an exception, as needed
            }
        }
        public static Image GenerateThumbnail(string videoPath)
        {
            // Construct the FFmpeg command to generate a thumbnail from the video
            string ffmpegCommand = $"-ss 1 -i \"{videoPath}\" -vf \"thumbnail,scale=320:-1\" -frames:v 1 -f image2pipe -vcodec bmp -\"";
            Form1.WriteToLog("Start getting thumbnail");
            using (MemoryStream thumbnailStream = new MemoryStream())
            {
                using (Process ffmpegProcess = new Process())
                {
                    ffmpegProcess.StartInfo.FileName = "ffmpeg"; // Make sure 'ffmpeg' is in your system's PATH.
                    ffmpegProcess.StartInfo.Arguments = ffmpegCommand;
                    ffmpegProcess.StartInfo.RedirectStandardOutput = true;
                    ffmpegProcess.StartInfo.RedirectStandardError = true;
                    ffmpegProcess.StartInfo.UseShellExecute =  false;
                    ffmpegProcess.StartInfo.CreateNoWindow = true;

                    Form1.WriteToLog("Start ffmpeg");
                    ffmpegProcess.Start();
                    using (Stream ffmpegOutput = ffmpegProcess.StandardOutput.BaseStream)
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        while ((bytesRead = ffmpegOutput.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            thumbnailStream.Write(buffer, 0, bytesRead);
                        }
                    }

                    ffmpegProcess.WaitForExit();
                    Form1.WriteToLog("Finish ffmpeg");
                }

                // Load the thumbnail from the memory stream and return it
                return Image.FromStream(thumbnailStream);
            }
        }
    }
}
