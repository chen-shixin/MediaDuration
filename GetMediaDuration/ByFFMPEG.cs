using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace MediaDuration
{
    public class ByFFmpeg : Duration
    {
        private StringBuilder result = new StringBuilder(); // Store output text of ffmpeg

        /// <summary>
        /// Get duration(ms) of audio or vedio by FFmpeg.exe
        /// </summary>
        /// <param name="filePath">audio/vedio's path</param>
        /// <returns>Duration in original format, duration in milliseconds</returns>
        /// <remarks>return value from FFmpeg.exe is in format of: "00:00:19.82"</remarks>
        public override Tuple<string, long> GetDuration(string filePath)
        {
            GetMediaInfo(filePath);
            string duration = MatchDuration(result.ToString());

            return Tuple.Create(duration, GetTimeInMillisecond(duration));
        }

        // Call exe async
        private void GetMediaInfo(string filePath)
        {
            result.Clear(); // Clear result to avoid previous value's interference

            Process p = new Process();
            p.StartInfo.FileName = "ffmpeg.exe";
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = string.Concat("-i ", filePath);
            p.ErrorDataReceived += new DataReceivedEventHandler(OutputCallback);

            p.Start();
            p.BeginErrorReadLine();

            p.WaitForExit();
            p.Close();
            p.Dispose();
        }

        // Callback funciton of output stream
        private void OutputCallback(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                result.Append(e.Data);
            }
        }

        // Match the 'Duration' section in "ffmpeg -i filepath" output text
        private string MatchDuration(string text)
        {
            string pattern = @"Duration:\s(\d{2}:\d{2}:\d{2}.\d+)";
            Match m = Regex.Match(text, pattern);

            return m.Groups.Count == 2 ? m.Groups[1].ToString() : string.Empty;
        }
    }
}
