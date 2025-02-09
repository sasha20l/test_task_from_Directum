using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Directum
{
    public class Meeting
    {
        public int Id { get; set; }               // Идентификатор встречи
        public string Title { get; set; }         // Тема встречи
        public DateTime StartTime { get; set; }   // Время начала
        public DateTime EndTime { get; set; }     // Время окончания
        public TimeSpan ReminderOffset { get; set; } // За сколько времени напоминать (TimeSpan)

        public Meeting(int id, string title, DateTime start, DateTime end, TimeSpan reminderOffset)
        {
            // Проверка корректности входных параметров
            if (start >= end)
                throw new ArgumentException("Время начала должно быть раньше времени окончания.");

            if (start <= DateTime.Now)
                throw new ArgumentException("Встреча должна планироваться на будущее время.");

            Id = id;
            Title = title;
            StartTime = start;
            EndTime = end;
            ReminderOffset = reminderOffset;
        }

        public override string ToString()
        {
            return $"[{Id}] {Title} | {StartTime:yyyy-MM-dd HH:mm} - {EndTime:HH:mm} " +
                   $"(Напоминание за {ReminderOffset.TotalMinutes} мин.)";
        }
    }
}
