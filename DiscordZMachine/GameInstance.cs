using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZMachineLib;

namespace DiscordZMachine
{
    public class GameInstance
    {
        private ZMachine zMachine;
        private DiscordConsole console;
        private Thread zMachineThread;
        private ISocketMessageChannel channel;

        public GameInstance(ISocketMessageChannel channel)
        {
            this.channel = channel;
            console = new DiscordConsole(channel);
            zMachine = new ZMachine(console);
        }

        public void Start(FileStream file)
        {
            zMachine.LoadFile(file);
            zMachineThread = new Thread(() => zMachine.Run());
            zMachineThread.Name = channel.Id.ToString();
            zMachineThread.Start();
        }

        public void EnqueueIncomingMessage(string message) => console.EnqueueIncomingMessage(message);

        public void Save() => zMachine.SaveState();

        public void Stop()
        {
            zMachineThread.Interrupt();
        }
    }
}
