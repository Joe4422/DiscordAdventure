using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiscordZMachine
{
    public class DiscordConsole
    {
        private Queue<string> incomingMessageQueue = new Queue<string>();
        private ISocketMessageChannel channel;
        private string outgoingMessage = "";

        public void EnqueueIncomingMessage(string message)
        {
            incomingMessageQueue.Enqueue(message);
        }

        public DiscordConsole(ISocketMessageChannel channel)
        {
            this.channel = channel;
        }

    }
}
