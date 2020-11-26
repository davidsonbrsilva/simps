using System.ComponentModel;

namespace SIMPS
{
    public enum FileFormat
    {
        [Description(".csv")] CSV,
        [Description(".txt")] TXT,
        [Description(".log")] LOG
    }
}
