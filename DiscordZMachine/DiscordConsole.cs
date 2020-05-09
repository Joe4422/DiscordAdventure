using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZMachineLib;

namespace DiscordZMachine
{
    public class DiscordConsole : IZMachineIO
    {
        private Queue<string> incomingMessageQueue = new Queue<string>();
        private TextStyle currentStyle = TextStyle.Roman;
        private ISocketMessageChannel channel;
        private string outgoingMessage = "";

        public DiscordConsole(ISocketMessageChannel channel)
        {
            this.channel = channel;
        }

        public void EnqueueIncomingMessage(string message)
        {
            incomingMessageQueue.Enqueue(message);
        }

        public void BufferMode(bool buffer)
        {
            return;
        }

        public void EraseWindow(ushort window)
        {
            return;
        }

        public void Print(string s)
        {
            if (s == ">")
            {
                channel.SendMessageAsync(outgoingMessage[0..^1]);
                outgoingMessage = "";
            }
            else
            {
                outgoingMessage += currentStyle switch
                {
                    TextStyle.Bold => Discord.Format.Bold(s),
                    TextStyle.Italic => Discord.Format.Italics(s),
                    TextStyle.Reverse => s.Reverse(),
                    _ => s,
                };
            }
        }

        public void Quit()
        {
            throw new NotImplementedException();
        }

        public string Read(int max)
        {
            while (incomingMessageQueue.Count == 0) ;
            return incomingMessageQueue.Dequeue();
        }

        public char ReadChar()
        {
            throw new NotImplementedException();
        }

        public Stream Restore()
        {
            try
            {
                FileStream fs = File.OpenRead(channel.Id.ToString());
                return fs;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Save(Stream stream)
        {
            try
            {
                FileStream fs = File.Create(channel.Id.ToString());
                stream.CopyTo(fs);
                fs.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SetColor(ZColor foreground, ZColor background)
        {
            return;
        }

        public void SetCursor(ushort line, ushort column, ushort window)
        {
            return;
        }

        public void SetTextStyle(TextStyle textStyle)
        {
            currentStyle = textStyle;
        }

        public void SetWindow(ushort window)
        {
            return;
        }

        public void ShowStatus()
        {
        }

        public void SoundEffect(ushort number)
        {
            return;
        }

        public void SplitWindow(ushort lines)
        {
            return;
        }
    }
}
