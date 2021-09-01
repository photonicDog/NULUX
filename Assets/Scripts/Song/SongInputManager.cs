using Assets.Scripts.Song.Enums;
using Assets.Scripts.Song.Extensions;
using Song.Types;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Song
{
    public class SongInputManager : MonoBehaviour
    {
        private static SongInputManager _instance;
        public static SongInputManager Instance { get { return _instance; } }

        public Queue<InputCommand> commandQueue;

        // Singleton instancing
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (commandQueue.Count > 0)
            {
                NoteManager.Instance.OnUpdate(commandQueue);
            }
            commandQueue.Clear();
        }

        public void ProcessL1(InputAction.CallbackContext callbackContext)
        {
            var pressType = GetPressType(callbackContext.started, callbackContext.canceled);
            var queueTime  = TranslateInputTime(callbackContext.startTime);
            commandQueue.Enqueue(new InputCommand(queueTime, KeyType.L1, pressType));
        }

        public void ProcessL2(InputAction.CallbackContext callbackContext)
        {
            var pressType = GetPressType(callbackContext.started, callbackContext.canceled);
            var queueTime = TranslateInputTime(callbackContext.startTime);
            commandQueue.Enqueue(new InputCommand(queueTime, KeyType.L2, pressType));
        }

        public void ProcessL3(InputAction.CallbackContext callbackContext)
        {
            var pressType = GetPressType(callbackContext.started, callbackContext.canceled);
            var queueTime = TranslateInputTime(callbackContext.startTime);
            commandQueue.Enqueue(new InputCommand(queueTime, KeyType.L3, pressType));
        }

        public void ProcessR1(InputAction.CallbackContext callbackContext)
        {
            var pressType = GetPressType(callbackContext.started, callbackContext.canceled);
            var queueTime  = TranslateInputTime(callbackContext.startTime);
            commandQueue.Enqueue(new InputCommand(queueTime, KeyType.R1, pressType));
        }

        public void ProcessR2(InputAction.CallbackContext callbackContext)
        {
            var pressType = GetPressType(callbackContext.started, callbackContext.canceled);
            var queueTime  = TranslateInputTime(callbackContext.startTime);
            commandQueue.Enqueue(new InputCommand(queueTime, KeyType.R2, pressType));
        }

        public void ProcessR3(InputAction.CallbackContext callbackContext)
        {
            var pressType = GetPressType(callbackContext.started, callbackContext.canceled);
            var queueTime = TranslateInputTime(callbackContext.startTime);
            commandQueue.Enqueue(new InputCommand(queueTime, KeyType.R3, pressType));
        }

        public void ProcessBar(InputAction.CallbackContext callbackContext)
        {
            var pressType = GetPressType(callbackContext.started, callbackContext.canceled);
            var queueTime  = TranslateInputTime(callbackContext.startTime);
            commandQueue.Enqueue(new InputCommand(queueTime, KeyType.Bar, pressType));
        }

        public void ProcessPause(InputAction.CallbackContext callbackContext)
        {
            //TODO: Tie to not-yet-added pause functionality
            throw new System.NotImplementedException("Pause functionality not implemented");
        }

        public void ProcessDebugKey(InputAction.CallbackContext callbackContext)
        {
            //TODO: Tie to not-yet-added debug functionality
            throw new System.NotImplementedException("Debug functionality not implemented");
        }

        private double TranslateInputTime(double time)
        {
            return time - Conductor.Instance.GetRealStartTimeOffset();
        }

        private PressType GetPressType(bool started, bool canceled)
        {
            if (started)
            {
                return PressType.PRESS;
            }
            else if (canceled)
            {
                return PressType.RELEASE;
            }
            else
            {
                throw new Exception("Input came in that was neither started nor canceled!");
            }
        }
    }
}
