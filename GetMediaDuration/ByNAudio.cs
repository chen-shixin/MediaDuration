using NAudio.Wave;
using System;

namespace MediaDuration
{
    public class ByNAudio : Duration
    {
        /// <summary>
        /// Get duration(ms) of audio or vedio by NAudio.dll
        /// </summary>
        /// <param name="filePath">audio/vedio's path</param>
        /// <returns>Duration in original format, duration in milliseconds</returns>
        /// <remarks>return value from NAudio.dll is in format of: "00:00:19.820"</remarks>
        public override Tuple<string, long> GetDuration(string filePath)
        {
            TimeSpan ts;
            try
            {
                using (AudioFileReader audioFileReader = new AudioFileReader(filePath))
                {
                    ts = audioFileReader.TotalTime;
                }
            }
            catch (Exception)
            {
                /* As NAudio is mainly used for processing audio, so some formats may not surport,
                 * just use 00:00:00 instead for these cases.
                 */
                ts = new TimeSpan();
                //throw ex;
            }

            return Tuple.Create(ts.ToString(), GetTimeInMillisecond(ts.ToString()));
        }
    }
}
