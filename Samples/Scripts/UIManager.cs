using com.spector.CommandQueue.Messages;
using com.spector.GameEvents;
using Sample.Scripts.Messages;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private MessageGameEvent onMessageEvent;
    
    public void ShowToast(string title)
    {
        // grab message data from somewhere
        ToastMessage toastMessage = new ToastMessage(title, "some description", "some url");
        if(onMessageEvent != null)
            onMessageEvent.Raise(toastMessage);
    }

    public void ShowModal()
    {
        ModalMessage modalMessage = new ModalMessage("modal title", "modal description", "modal icon", () =>
        {
            
        });
        
        if(onMessageEvent != null)
            onMessageEvent.Raise(modalMessage);
    }
}
