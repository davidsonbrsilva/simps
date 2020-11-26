using UnityEngine;

namespace SIMPS
{
    [System.Serializable]
    public class Timer
    {
        [SerializeField][Range(0, 999)] private int hours = 0;
        [SerializeField][Range(0, 59)] private int minutes = 1;
        [SerializeField][Range(0, 59)] private int seconds = 0;

        public int Hours { get { return hours; } set { hours = value; } }
        public int Minutes { get { return minutes; } set { minutes = value; } }
        public int Seconds { get { return seconds; } set { seconds = value; } }

        public Timer(int hours, int minutes, int seconds)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }

        public int GetTimeInSeconds()
        {
            return (Hours * 3600) + (Minutes * 60) + Seconds;
        }

        public float GetTimeInMinutes()
        {
            return (Hours * 60) + Minutes + (Seconds / 60);
        }

        public float GetTimeInHours()
        {
            return Hours + (Minutes / 60) + (Seconds / 3600);
        }
    }
}
