using com.spector.Extensions;
using com.spector.views;
using Sample.Scripts.Messages;
using TMPro;
using UnityEngine.UI;

public class ModalView : MessageView<ModalMessage>
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image icon;
    public Button okay;
    public Button cancel;
    
    public override void Show(float duration)
    {
        icon.enabled = false;
        this.gameObject.SetActive(true);
            
        title.text = Model.Title;
        description.text = Model.Description;
        icon.SetImageFromUrlAsync(Model.IconUrl, result =>
        {
            icon.enabled = true;
        });

        if (okay != null)
        {
            // we will just hide the message for demo purposes
            okay.onClick.AddListener(Hide);
        }

        if (cancel != null)
        {
            // we will just hide the message for demo purposes
            cancel.onClick.AddListener(Hide);
        }
        
        base.Show(duration);
    }

    protected override void CloseAfterDuration(float duration)
    {
        // Do nothing because we don't want the modal to close until user interaction
    }
}
