﻿using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiscordZMachine
{
    public class GameInstanceManager
    {
        public enum CreateInstanceResult
        {
            Success,
            Fail_Exists,
            Fail_InstanceLimit
        }
        public enum DestroyInstanceResult
        {
            Success,
            Fail_NoSuchInstance
        }

        private Dictionary<ISocketMessageChannel, GameInstance> instances;

        public int MaxInstances { get; set; }
        public int InstanceCount => instances.Count;

        public GameInstanceManager(int maxInstances = -1)
        {
            instances = new Dictionary<ISocketMessageChannel, GameInstance>();
            MaxInstances = maxInstances;
        }

        public CreateInstanceResult CreateInstance(ISocketMessageChannel messageChannel, FileStream gameFile)
        {
            if (instances.ContainsKey(messageChannel))
            {
                return CreateInstanceResult.Fail_Exists;
            }
            else if (InstanceCount >= MaxInstances && MaxInstances >= 0)
            {
                return CreateInstanceResult.Fail_InstanceLimit;
            }
            else
            {
                instances.Add(messageChannel, new GameInstance(messageChannel));
                instances[messageChannel].Start(gameFile);
                return CreateInstanceResult.Success;
            }
        }

        public DestroyInstanceResult DestroyInstance(ISocketMessageChannel messageChannel)
        {
            bool instanceExists = instances.TryGetValue(messageChannel, out GameInstance instance);

            if (!instanceExists)
            {
                return DestroyInstanceResult.Fail_NoSuchInstance;
            }
            else
            {
                instance.Stop();
                instances.Remove(messageChannel);
                return DestroyInstanceResult.Success;
            }
        }

        public bool InstanceExists(ISocketMessageChannel messageChannel) => instances.ContainsKey(messageChannel);

        public void EnqueueIncomingMessage(ISocketMessageChannel messageChannel, string message)
        {
            bool instanceExists = instances.TryGetValue(messageChannel, out GameInstance instance);

            if (instanceExists)
            {
                instance.EnqueueIncomingMessage(message);
            }
        }
    }
}
