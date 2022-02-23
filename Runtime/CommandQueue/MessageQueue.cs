using com.spector.CommandQueue.Commands;
using com.spector.CommandQueue.Messages;
using com.spector.CommandQueue.Messages.Config;
using UnityEngine;

namespace com.spector.CommandQueue
{
    /// <summary>
    /// The message queue is used by attaching the component to a game object
    /// It can process any type of Message that is created.
    /// In this example we are creating a Toast Message but you could also create a Modal Message too
    /// Example usage:
    /// create a toast message
    /// ToastMessage toastMessage = new ToastMessage(title, etc);
    /// toastMessage.show(); or MessageQeue.Show(toastMessage); 
    /// </summary>
    public class MessageQueue : CommandQueue
    {
        public MessageQueueConfig messageQueueConfig;
        public MessageViewConfig[] messageViewConfigs;
        
        public void ShowMessage(MessageBase messageBase)
        {
            // find appropriate message view config attached to the MessageQueue
            MessageViewConfig messageViewConfig = GetMessageViewConfig(messageBase.Name);
            
            if (messageViewConfig == null)
            {
                Debug.LogError($"MessageQueue is missing a MessageViewConfig for: {messageBase.Name}");
                return;
            }
            
            // sets up in between delay from config
            waitForDelay = messageQueueConfig.DelayNext;
            
            // create and enqueue the command
            ShowMessageCommand messageCommand = new ShowMessageCommand(messageQueueConfig, messageViewConfig, messageBase);
            
            // kick of the command or wait for its turn
            Enqueue(messageCommand);
        }
        
        /// <summary>
        /// Finds an attached MessageViewConfig based on the messages name. ie toast or modal
        /// </summary>
        /// <param name="messageName"></param>
        /// <returns></returns>
        private MessageViewConfig GetMessageViewConfig(string messageName)
        {
            if (messageViewConfigs != null)
            {
                for (int i = 0; i < messageViewConfigs.Length; i++)
                {
                    MessageViewConfig messageViewConfig = messageViewConfigs[i];
                    if (messageViewConfig.messageModelLocator.name == messageName)
                    {
                        return messageViewConfig;
                    }
                }
            }
            return null;
        }

        protected override void Save()
        {
            var list = _queue.ToArray();
            var serializedList = new string[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                serializedList[i] = list[i].Serialize();
            }

            Debug.Log($"Saving list: {string.Join(", ", serializedList)}");
        }

        protected override void Restore()
        {
            
        }

        // private IEnumerator WaitForFrames(int numFrames)
        // {
        //     int counter = 0;
        //     while (counter < numFrames)
        //     {
        //         counter++;
        //         yield return new WaitForEndOfFrame();    
        //     }
        // }
    }
}

