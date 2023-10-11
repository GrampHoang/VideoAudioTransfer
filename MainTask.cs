using System.Diagnostics;

namespace VideoAudioTransfer
{
    public class MainTask
    {
        public static void TransferAudio(string inputVideo1Path, string inputVideo2Path)
        {
            if (inputVideo1Path == null){
                Form1.WriteToLog("Please choose video that TAKE audio!");
                return;
            }
            if (inputVideo2Path == null){
                Form1.WriteToLog("Please choose video that GIVE audio!");
                return;
            }
            Form1.WriteToLog("Start transfering");
            // Get the root folder of the application
            string appRootFolder = AppDomain.CurrentDomain.BaseDirectory;
            string outputFileName = Path.GetFileName(inputVideo1Path);
            // Construct the output video file path in the application's root folder
            string outputVideoPath = Path.Combine(appRootFolder, outputFileName);
            
            // Construct the FFmpeg command to merge audio from video 1 into video 2
            string ffmpegCommand = $"-y -i \"{inputVideo1Path}\" -i \"{inputVideo2Path}\" -c copy -map 0:v:0 -map 1:a:0 -shortest \"{outputVideoPath}\"";

            // Run the FFmpeg command and capture the output
            Form1.WriteToLog("Start ffmpeg");
            using (Process ffmpegProcess = new Process())
            {
                ffmpegProcess.StartInfo.FileName = "ffmpeg"; // Make sure 'ffmpeg' is in your system's PATH.
                ffmpegProcess.StartInfo.Arguments = ffmpegCommand;
                ffmpegProcess.StartInfo.RedirectStandardOutput = true;
                ffmpegProcess.StartInfo.RedirectStandardError = true;
                ffmpegProcess.StartInfo.UseShellExecute = false;
                ffmpegProcess.StartInfo.CreateNoWindow = true;
                ffmpegProcess.Start();

                ffmpegProcess.OutputDataReceived += new DataReceivedEventHandler((sender, e) => OutputHandler(e.Data, false));
                ffmpegProcess.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => OutputHandler(e.Data, true));

                // Begin asynchronous read of the standard output and error streams
                ffmpegProcess.BeginOutputReadLine();
                ffmpegProcess.BeginErrorReadLine();
                ffmpegProcess.WaitForExit();
                string logMessage = string.Format("Done, output should be found at: {0}", outputVideoPath);
                Form1.WriteToLog(logMessage);
            }
        }

        private static void OutputHandler(string output, bool isError)
        {
            if (!String.IsNullOrEmpty(output))
            {
                Form1.WriteToLog("[ffmpeg] " + output);
            }
        }

    }
}
