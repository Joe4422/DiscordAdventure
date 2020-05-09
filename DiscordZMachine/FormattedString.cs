using System;
using System.Collections.Generic;
using System.Text;
using ZMachineLib;

namespace DiscordZMachine
{
    public class FormattedString
    {
        public TextStyle Style { get; }
        public string String { get; }

        public FormattedString(string str, TextStyle style = TextStyle.Roman)
        {
            String = str;
            Style = style;
        }
    }
}
