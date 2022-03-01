using com.spector.CommandQueue.Messages;
using com.spector.GameEvents;
using Sample.Scripts.Messages;
using UnityEngine;

namespace Sample.Scripts
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private MessageGameEvent onMessageEvent;
    
        public void ShowToast(string title)
        {
            // grab message data from somewhere
            ToastMessage toastMessage = new ToastMessage(title, "some description",
                "https://www.4me.com/wp-content/uploads/2018/03/4me-icon-bell.png");
            if(onMessageEvent != null)
                onMessageEvent.Raise(toastMessage);
        }

        public void ShowModal()
        {
            ModalMessage modalMessage = new ModalMessage("modal title", "modal description", "modal icon", () =>
            {
                // do something here on Yes button   
            });
        
            if(onMessageEvent != null)
                onMessageEvent.Raise(modalMessage);
        }
    }
}

