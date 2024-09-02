using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Clock.Time
{
    public class Time
    {
        private TimeSynchronizer _timeSync;
        public event Action<DateTime> UpdateTime;
        public DateTime alarmClockTime;
        private bool _presentAlarmIsClock;

        public Time()
        {
            _timeSync.StartSyncTime();
            InitSyncTime();

        }
        
        public void SetPresentAlamInClock(bool presentAlarmIsClock)
        {
            _presentAlarmIsClock = presentAlarmIsClock;
        }

        private async void InitSyncTime()
        {
            Debug.Log("Синхронизация времени с серверами");
            DateTime currentSyncTime = await _timeSync.SynchronizeTime();
            TimeСounting(currentSyncTime);
        }

        private async void TimeСounting(DateTime currentSyncTime)
        {
            DateTime now = DateTime.UtcNow;
            while (currentSyncTime.Minute != 0)
            {
                await Task.Yield();
                TimeSpan elapsed = DateTime.UtcNow - now;
                currentSyncTime = currentSyncTime.Add(elapsed);
                UpdateTime?.Invoke(currentSyncTime);
                now = DateTime.UtcNow;
                CheckingAlarmTime(currentSyncTime);
            }
            InitSyncTime();
        }

        private void CheckingAlarmTime(DateTime currentSyncTime)
        {
            if (!_presentAlarmIsClock) return;
            if (alarmClockTime.Hour == currentSyncTime.Hour
                && alarmClockTime.Minute == currentSyncTime.Minute
                && alarmClockTime.Second == currentSyncTime.Second)
                Debug.Log("Сработал будильник");
        }
    }
}