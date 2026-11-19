using System;
using System.Globalization;
using System.Windows.Forms;

namespace KeyEventLogger
{
    internal sealed class KeyEntry
    {
        public DateTime Time { get; }
        public string Key { get; }

        public KeyEntry(DateTime time, Keys key)
        {
            Time = time;
            Key = key.ToString();
        }

        public string ToFileLine()
        {
            return Time.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + "\t" + Key;
        }

        public override string ToString()
        {
            return Time.ToString("HH:mm:ss.fff") + "     " + Key;
        }
    }
}
