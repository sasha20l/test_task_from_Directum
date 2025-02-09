using System.Globalization;

namespace Task3Directum
{
    public class Program
    {
        static void Main(string[] args)
        {
            var manager = new MeetingManager();

            // Запускаем сервис напоминаний
            using var reminderService = new ReminderService(manager);

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nДоступные команды: add, edit, delete, list, export, exit");
                Console.Write("Введите команду: ");
                var command = Console.ReadLine()?.Trim().ToLower();

                try
                {
                    switch (command)
                    {
                        case "add":
                            HandleAdd(manager);
                            break;
                        case "edit":
                            HandleEdit(manager);
                            break;
                        case "delete":
                            HandleDelete(manager);
                            break;
                        case "list":
                            HandleList(manager);
                            break;
                        case "export":
                            HandleExport(manager);
                            break;
                        case "exit":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Неизвестная команда!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }

            Console.WriteLine("Программа завершена.");
        }

        private static void HandleAdd(MeetingManager manager)
        {
            Console.WriteLine("Добавление встречи.");
            Console.Write("Название встречи: ");
            var title = Console.ReadLine();

            Console.Write("Дата и время начала (формат yyyy-MM-dd HH:mm): ");
            var startStr = Console.ReadLine();
            DateTime start = DateTime.ParseExact(startStr, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            Console.Write("Дата и время окончания (формат yyyy-MM-dd HH:mm): ");
            var endStr = Console.ReadLine();
            DateTime end = DateTime.ParseExact(endStr, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            Console.Write("За сколько минут до встречи напоминать? ");
            var offsetStr = Console.ReadLine();
            int offsetMinutes = int.Parse(offsetStr);
            TimeSpan reminder = TimeSpan.FromMinutes(offsetMinutes);

            manager.AddMeeting(title, start, end, reminder);
            Console.WriteLine("Встреча добавлена.");
        }

        private static void HandleEdit(MeetingManager manager)
        {
            Console.Write("Введите Id встречи для редактирования: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Новое название: ");
            var title = Console.ReadLine();

            Console.Write("Дата и время начала (yyyy-MM-dd HH:mm): ");
            var startStr = Console.ReadLine();
            DateTime start = DateTime.ParseExact(startStr, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            Console.Write("Дата и время окончания (yyyy-MM-dd HH:mm): ");
            var endStr = Console.ReadLine();
            DateTime end = DateTime.ParseExact(endStr, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            Console.Write("За сколько минут до встречи напоминать? ");
            var offsetStr = Console.ReadLine();
            int offsetMinutes = int.Parse(offsetStr);
            TimeSpan reminder = TimeSpan.FromMinutes(offsetMinutes);

            manager.EditMeeting(id, title, start, end, reminder);
            Console.WriteLine("Встреча отредактирована.");
        }

        private static void HandleDelete(MeetingManager manager)
        {
            Console.Write("Введите Id встречи для удаления: ");
            int id = int.Parse(Console.ReadLine());
            manager.RemoveMeeting(id);
            Console.WriteLine($"Встреча с Id={id} удалена (если существовала).");
        }

        private static void HandleList(MeetingManager manager)
        {
            Console.Write("Введите дату для просмотра встреч (yyyy-MM-dd): ");
            string dateStr = Console.ReadLine();
            DateTime date = DateTime.ParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var meetings = manager.GetMeetingsByDay(date);
            if (meetings.Count == 0)
            {
                Console.WriteLine("На эту дату встреч нет.");
            }
            else
            {
                Console.WriteLine($"Встречи на {date:yyyy-MM-dd}:");
                foreach (var m in meetings)
                {
                    Console.WriteLine(m);
                }
            }
        }

        private static void HandleExport(MeetingManager manager)
        {
            Console.Write("Введите дату для экспорта (yyyy-MM-dd): ");
            string dateStr = Console.ReadLine();
            DateTime date = DateTime.ParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var meetings = manager.GetMeetingsByDay(date);
            if (meetings.Count == 0)
            {
                Console.WriteLine("На эту дату встреч нет. Экспорт не выполнен.");
                return;
            }

            Console.Write("Введите путь к файлу для экспорта (например, C:\\temp\\export.txt): ");
            string filePath = Console.ReadLine();

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Встречи на {date:yyyy-MM-dd}:");
                foreach (var m in meetings)
                {
                    writer.WriteLine(m);
                }
            }

            Console.WriteLine("Экспорт завершен.");
        }
    }
}
