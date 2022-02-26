using System.Collections;
using System.Collections.Generic;
using com.spector.Attributes;
using com.spector.CommandQueue.Interfaces;
using UnityEngine;

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
        public ICommandQueueSerializer QueueSerializer => _queueSerializer as ICommandQueueSerializer;
        
        // queue of commands
        protected Queue<ICommand> _queue;
        
        // used by the global config to wait in between messages
        protected float waitForDelay;
        
        // it's not null when a command is running
        private Coroutine _coroutine;
        private bool _isPending;

        public CommandQueue()
        {
            // create a queue
            _queue = new Queue<ICommand>();
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

        public void DoNext()
        {
            // if queue is empty, do nothing.
            if (_queue.Count == 0 || _isPending)
                return;

            // get a command
            var cmd = _queue.Dequeue();
            
            // listen to the OnFinished event
            cmd.OnFinished += OnCmdFinished;
            
            // execute command
            _isPending = true;
            _coroutine = StartCoroutine(ExecuteNextCommand(cmd));
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
            _isPending = false;

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