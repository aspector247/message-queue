using UnityEngine;

namespace com.spector.views
{
    public abstract class ViewBase : MonoBehaviour
    {
        public abstract void SetModel(object model);
        public abstract void Show(float duration);
    }
}