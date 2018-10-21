﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreambound.Networking.Threading
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static readonly Queue<Action> _executionQueue = new Queue<Action>();

        public void Update()
        {
            lock (_executionQueue)
            {
                while (_executionQueue.Count > 0)
                {
                    _executionQueue.Dequeue().Invoke();
                }
            }
        }

        /// <summary>
        /// Locks the queue and adds the IEnumerator to the queue
        /// </summary>
        /// <param name="action">IEnumerator function that will be executed from the main thread.</param>
        public void Enqueue(IEnumerator action)
        {
            lock (_executionQueue)
            {
                _executionQueue.Enqueue(() => {
                    StartCoroutine(action);
                });
            }
        }

        /// <summary>
        /// Locks the queue and adds the Action to the queue
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        public void Enqueue(Action action)
        {
            Enqueue(ActionWrapper(action));
        }
        IEnumerator ActionWrapper(Action a)
        {
            a();
            yield return null;
        }

        /// <summary>
        /// Locks the queue and adds the Action to the queue
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        public void Enqueue(Action<object> action, object obj)
        {
            Enqueue(ActionWrapper(action, obj));
        }
        IEnumerator ActionWrapper(Action<object> a, object obj)
        {
            a(obj);
            yield return null;
        }

        public void Enqueue(Action<object, object> action, object obj1, object obj2)
        {
            Enqueue(ActionWrapper(action, obj1, obj2));
        }
        IEnumerator ActionWrapper(Action<object, object> a, object obj1, object obj2)
        {
            a(obj1, obj2);
            yield return null;
        }


        private static MainThreadDispatcher _instance = null;

        public static bool Exists()
        {
            return _instance != null;
        }

        public static MainThreadDispatcher Instance()
        {
            if (!Exists())
            {
                throw new Exception("UnityMainThreadDispatcher could not find the UnityMainThreadDispatcher object. Please ensure you have added the MainThreadExecutor Prefab to your scene.");
            }
            return _instance;
        }


        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        void OnDestroy()
        {
            _instance = null;
        }


    }
}