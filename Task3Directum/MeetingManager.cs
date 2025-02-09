using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3Directum
{
    public class MeetingManager
    {
        private List<Meeting> _meetings = new List<Meeting>();
        private int _nextId = 1;  // Счётчик для автоинкремента Id

        public void AddMeeting(string title, DateTime start, DateTime end, TimeSpan reminderOffset)
        {
            // Создаём временный объект, чтобы проверить корректность
            var newMeeting = new Meeting(_nextId, title, start, end, reminderOffset);

            // Проверка пересечений
            if (IsOverlapping(newMeeting))
                throw new InvalidOperationException("Данная встреча пересекается с уже запланированными.");

            _meetings.Add(newMeeting);
            _nextId++;
        }

        public void EditMeeting(int id, string title, DateTime start, DateTime end, TimeSpan reminderOffset)
        {
            var existing = _meetings.FirstOrDefault(m => m.Id == id);
            if (existing == null)
                throw new ArgumentException($"Встреча с Id={id} не найдена");

            // Создаём временный объект для проверки
            var tempMeeting = new Meeting(id, title, start, end, reminderOffset);

            // Исключаем из списка текущую встречу, чтобы проверить пересечения
            var oldList = _meetings.Where(m => m.Id != id).ToList();
            if (IsOverlapping(tempMeeting, oldList))
                throw new InvalidOperationException("Данная встреча пересекается с уже запланированными.");

            // Обновляем поля
            existing.Title = title;
            existing.StartTime = start;
            existing.EndTime = end;
            existing.ReminderOffset = reminderOffset;
        }

        public void RemoveMeeting(int id)
        {
            var toRemove = _meetings.FirstOrDefault(m => m.Id == id);
            if (toRemove != null)
            {
                _meetings.Remove(toRemove);
            }
        }

        public List<Meeting> GetMeetingsByDay(DateTime date)
        {
            // Сравниваем только дату
            return _meetings
                .Where(m => m.StartTime.Date == date.Date)
                .OrderBy(m => m.StartTime)
                .ToList();
        }

        private bool IsOverlapping(Meeting newMeeting, List<Meeting> listToCheck = null)
        {
            var checkList = listToCheck ?? _meetings;

            foreach (var m in checkList)
            {
              
                if ((newMeeting.StartTime < m.EndTime) && (m.StartTime < newMeeting.EndTime))
                {
                    return true;
                }
            }
            return false;
        }

        // Может понадобиться метод для получения всех встреч
        public List<Meeting> GetAllMeetings() => _meetings;
    }
}

