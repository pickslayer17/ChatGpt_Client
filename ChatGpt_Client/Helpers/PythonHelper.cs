using System.Diagnostics;

namespace ChatGpt_Client.Helpers
{
    public class PythonHelper
    {
        public static async Task<int> RunPythonScript(string text)
        {
            // Python код, который должен быть выполнен
            text = text.Replace("\'", "\\\"").Replace("\"", "\\\"").Replace("\r", "").Replace("\n", ";");
            string pythonCode = "import sys\n"
                  + "import tiktoken\n"
                  + "encoding = tiktoken.encoding_for_model('gpt-4-turbo')\n"
                  + "print(len(encoding.encode(\"" + text + "\")))";

            // Создание временного файла с Python кодом
            string tempFilePath = System.IO.Path.GetTempFileName() + ".py";
            System.IO.File.WriteAllText(tempFilePath, pythonCode);

            // Настройка процесса для выполнения Python скрипта
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "python";
            start.Arguments = tempFilePath;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.CreateNoWindow = true;

            // Запуск процесса
            using (Process process = Process.Start(start))
            {
                // Чтение вывода из стандартного потока
                string result = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                // Ожидание завершения процесса
                process.WaitForExit();
                System.IO.File.Delete(tempFilePath);

                // Вывод результата и ошибок
                return int.Parse(result.Trim());
            }
        }
    }
}
