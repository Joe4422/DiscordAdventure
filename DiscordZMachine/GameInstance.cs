using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordZMachine
{
    public class GameInstance
    {
        private ZMachine zMachine;
        private DiscordConsole console;
        private Thread zMachineThread;
        private ISocketMessageChannel channel;

        public GameInstance(Stream gameStream, ISocketMessageChannel channel)
        {
            this.channel = channel;
            console = new DiscordConsole(channel);
            zMachine = new ZMachine(gameStream, console);
        }

        public bool Start()
        {
            try
            {
                zMachineThread = new Thread(() => zMachine.Run());
                zMachineThread.Name = channel.Id.ToString();
                zMachineThread.Start();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void EnqueueIncomingMessage(string message) => console.EnqueueIncomingMessage(message);
    }
}
