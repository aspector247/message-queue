using com.spector.CommandQueue.Messages;
using UnityEngine;

namespace com.spector.GameEvents
{
    [CreateAssetMenu(fileName = "New Message Event", menuName = "Game Event/Message Event")]
    public class MessageGameEvent : BaseGameEvent<MessageBase>
    {
    }
}