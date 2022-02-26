using UnityEditor;
using UnityEngine;

namespace com.spector.CommandQueue.Messages
{
    /// <summary>
    /// The MessageFactory is responsible for creating messages based on json data
    /// { Name: "toast", Title: "my title", Description: "my description", Icon: "some url or relative asset" }
    /// </summary>
    public class MessageFactory
    {
        public static MessageBase FromJson(string jsonMessage)
        {
            // jsonMessage must contain a type attribute
            MessageBase messageBase = JsonUtility.FromJson<MessageBase>(jsonMessage);
            switch (messageBase.Name)
            {
                case "toast":
                    return CreateToastMessage(jsonMessage);
                default:
                    return null;
            }
        }

        #region Supported Messages
        private static ToastMessage CreateToastMessage(string jsonMessage)
        {
            return JsonUtility.FromJson<ToastMessage>(jsonMessage);
        }
        #endregion
        
    }
}