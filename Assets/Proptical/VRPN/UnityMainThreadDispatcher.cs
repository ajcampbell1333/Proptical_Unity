using System;
using System.Collections.Generic;
using UnityEngine;

namespace Proptical.VRPN
{
    /// <summary>
    /// Dispatcher for executing actions on Unity's main thread
    /// </summary>
    public class UnityMainThreadDispatcher : MonoBehaviour
    {
        private static UnityMainThreadDispatcher instance;
        private static readonly Queue<Action> actionQueue = new Queue<Action>();
        private static readonly object queueLock = new object();

        public static UnityMainThreadDispatcher Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("UnityMainThreadDispatcher");
                    instance = go.AddComponent<UnityMainThreadDispatcher>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        private void Update()
        {
            lock (queueLock)
            {
                while (actionQueue.Count > 0)
                    actionQueue.Dequeue()?.Invoke();
            }
        }

        public void Enqueue(Action action)
        {
            if (action == null) return;

            lock (queueLock)
                actionQueue.Enqueue(action);
        }
    }
}

