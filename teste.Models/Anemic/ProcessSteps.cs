using System.Text;

namespace teste.Models.Anemic
{
    public class ProcessSteps
    {
        private StringBuilder StringBuilder { get; set; }

        public ProcessSteps()
        {
            StringBuilder = new StringBuilder();
        }

        public void AddLogMessageContent(string mesageContent)
        {
            StringBuilder.AppendLine(mesageContent);
        }

        public string GetLogMessageContent()
        {
            return StringBuilder.ToString();
        }
    }
}
