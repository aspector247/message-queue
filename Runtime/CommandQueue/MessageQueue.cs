using com.spector.CommandQueue.Commands;
using com.spector.CommandQueue.Messages;
using com.spector.CommandQueue.Messages.Config;
using UnityEngine;

namespace com.spector.CommandQueue
{
    /// <summary>
    /// The message queue is used by attaching the component to a game object
    /// It can process any type of Message that is created and sent using the MessageEvent attached to the object
    /// Example usage:
    /// ToastMessage toastMessage = new ToastMessage(title, "some description", "some url");
    ///    if(onMessageEvent != null)
    ///        onMessageEvent.Raise(toastMessage);
    /// </summary>
    public class MessageQueue : CommandQueue
    {
        public MessageQueueConfig messageQueueConfig;
        public MessageViewConfig[] messageViewConfigs;
        
        /// <summary>
        /// Send a sublcass of MessageBase and it will create a ShowMessageCommand and queue it to be executed
        /// </summary>
        /// <param name="messageBase"></param>
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
    }
}

