using System;
using System.Collections;
using System.Collections.Generic;
using com.spector.Attributes;
using com.spector.CommandQueue.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace com.spector.CommandQueue
{
    /// <summary>
    /// Subclass the CommandQueue to inherit its ability to Queue any type of command and serialize them to disk or db 
    /// </summary>
    public abstract class CommandQueue : MonoBehaviour
    {
        [SerializeField] 
        [RequireInterface(typeof(ICommandQueueSerializer))]
        private Object _queueSerializer; 
        public ICommandQueueSerializer QueueSerializer
        {
            get => _queueSerializer as ICommandQueueSerializer;
            set => _queueSerializer = (Object) value;
        }

        /// <summary>
        /// If true, the message queue will start executing on instantiation
        /// </summary>
        [SerializeField]
        public bool PlayOnStart = false;

        // queue of commands
        [SerializeField] private Queue<ICommand> _queue = new Queue<ICommand>();

        public Queue<ICommand> Queue => _queue;

        // used by the global config to wait in between messages
        protected float waitForDelay;
        
        // it's not null when a command is running
        private Coroutine _coroutine;
        private bool _isProcessing;
        

        private void OnEnable()
        {
            if (QueueSerializer != null)
            {
                _queue = QueueSerializer.Deserialize();
                _isProcessing = !PlayOnStart; // pause queue if not play on start
                DoNext();
            }
        }

        public CommandQueue()
        {
            _isProcessing = !PlayOnStart; // pause queue if not play on start
        }

        public void Enqueue(ICommand cmd)
        {
            // add a command
            _queue.Enqueue(cmd);
            
            // persist queue to disk
            Save();
            
            // if no command is running, start to execute commands
            if (_coroutine == null)
                DoNext();
        }

        public void Stop()
        {
            _isProcessing = true;
        }

        public void Play()
        {
            _isProcessing = false;
        }

        public void DoNext()
        {
            // if queue is empty, do nothing.
            if (_queue.Count == 0 || _isProcessing)
                return;

            // get a command
            var cmd = _queue.Dequeue();
            
            // listen to the OnFinished event
            cmd.OnFinished += OnCmdFinished;
            
            // execute command
            _isProcessing = true;
            _coroutine = StartCoroutine(ExecuteNextCommand(cmd));
        }

        public void Clear()
        {
            _queue.Clear();
        }

        // yields for command so we can wait frames or seconds
        private IEnumerator ExecuteNextCommand(ICommand command)
        {
            yield return command.Execute();
        }

        private void OnCmdFinished()
        {
            // current command is finished
            _coroutine = null;
            _isProcessing = false;
            
            // when the command is finished we need to save the current state 
            Save();
            
            // wait for next command based on wait delay
            StartCoroutine(WaitForNext());
        }

        private IEnumerator WaitForNext()
        {
            yield return new WaitForSeconds(waitForDelay);
            
            // run the next command
            DoNext();
        }

        /// <summary>
        /// Saves queue commands with queue specific ICommandQueueSerializer
        /// </summary>
        protected virtual void Save()
        {
            var serializer = QueueSerializer as ICommandQueueSerializer;
            serializer?.Serialize(_queue);
        }

        /// <summary>
        /// Restores queue commands with queue specific ICommandQueueSerializer
        /// </summary>
        protected virtual void Restore()
        {
            var serializer = QueueSerializer as ICommandQueueSerializer;
            if (serializer != null)
            {
                _queue = serializer.Deserialize();
            }
        }


    }
}