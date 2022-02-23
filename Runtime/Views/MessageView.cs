using System;
using System.Collections;
using com.spector.CommandQueue.Messages;
using com.spector.views.Transitions;
using UnityEngine;

namespace com.spector.views
{
    /// <summary>
    /// Extend this class to inherit its ability to set a model and transitions of showing and hiding a view
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageView<T> : ViewBase where T : MessageBase
    {
        protected T Model;
        private IViewTransition _viewTransition;
        
        private void Awake()
        {
            _viewTransition = GetComponent<IViewTransition>();
        }

        public override void SetModel(object model)
        {
            Model = model != null ? (T) model : Activator.CreateInstance<T>();
        }
        
        public override void Show(float duration)
        {
            if (_viewTransition != null)
            {
                _viewTransition.Show(() =>
                {
                    CloseAfterDuration(duration);    
                });
            }
            else
            {
                CloseAfterDuration(duration);
            }
        }

        protected virtual void CloseAfterDuration(float duration)
        {
            StartCoroutine(_CloseAfterDuration(duration));
        }

        private IEnumerator _CloseAfterDuration(float duration)
        {
            yield return new WaitForSeconds(duration);
            // dismiss view
            Hide();
        }

        public virtual void Hide()
        {
            if (_viewTransition != null)
            {
                _viewTransition.Hide(() =>
                {
                    Model.OnClose?.Invoke();
                    Destroy(this.gameObject);
                });
            }
            else
            {
                Model.OnClose?.Invoke();
                Destroy(this.gameObject);
            }
        }
    }
}