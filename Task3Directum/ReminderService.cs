using System;
using System.Timers;
using System.Linq;

namespace Task3Directum
{
    public class ReminderService : IDisposable
    {
        private readonly MeetingManager _manager;
        private System.Timers.Timer _timer;

        public ReminderService(MeetingManager manager)
        {
            _manager = manager;

            // Проверяем каждые 10 секунд
            _timer = new System.Timers.Timer(10000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;  // Чтобы повторялся
            _timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            var meetings = _manager.GetAllMeetings();

            foreach (var meeting in meetings)
            {
                // Когда пора напомнить?
                var remindTime = meeting.StartTime - meeting.ReminderOffset;

                if (now >= remindTime && now < meeting.StartTime)
                {
                    var minutesLeft = (int)(meeting.StartTime - now).TotalMinutes;
                    Console.WriteLine($"[Напоминание] Через {minutesLeft} мин. встреча \"{meeting.Title}\" в {meeting.StartTime:HH:mm}");
                }
            }
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }
        }
    }
}
