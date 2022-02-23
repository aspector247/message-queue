using UnityEngine;

namespace com.spector.CommandQueue.Messages
{
    /// <summary>
    /// The MessageFactory is responsible for creating messages based on json data
    /// { type: "toast", title: "my title", description: "my description", icon: "some url or relative asset" }
    /// </summary>
    public class MessageFactory
    {
        
        public static MessageBase FromJson(string jsonMessage)
        {
            // jsonMessage must contain a type attribute
            MessageBase messageBase = JsonUtility.FromJson<MessageBase>(jsonMessage);
            //MessageType messageType = (MessageType) Enum.Parse(typeof(MessageType), messageBase.Name, true);
            switch (messageBase.Name)
            {
                case "toast":
                    return CreateToastMessage(jsonMessage);
                // case MessageType.Modal:
                //     return CreateModalMessage(jsonMessage);
                default:
                    return null;
            }
        }

        #region Supported Messages
        private static ToastMessage CreateToastMessage(string jsonMessage)
        {
            return JsonUtility.FromJson<ToastMessage>(jsonMessage);
        }
        
        // private static ToastMessage CreateModalMessage(string jsonMessage)
        // {
        //     return JsonUtility.FromJson<ModalMessage>(jsonMessage);
        // }
        #endregion
        
    }
}