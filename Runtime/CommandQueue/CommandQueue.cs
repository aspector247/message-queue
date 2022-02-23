using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.spector.CommandQueue
{
    public abstract class CommandQueue : MonoBehaviour
    {
        // queue of commands
        protected readonly Queue<ICommand> _queue;
        
        // used by the global config to wait in between messages
        protected float waitForDelay;
        
        // it's not null when a command is running
        private Coroutine _coroutine;
        
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
            if (_queue.Count == 0)
                return;

            // get a command
            var cmd = _queue.Dequeue();
            
            // listen to the OnFinished event
            cmd.OnFinished += OnCmdFinished;
            
            // execute command
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
        /// Saves queue commands with ICommand.Serialize()
        /// </summary>
        protected abstract void Save();
        /// <summary>
        /// Restores queue commands with ICommand.Deserialize()
        /// </summary>
        protected abstract void Restore();
        
        
    }
}