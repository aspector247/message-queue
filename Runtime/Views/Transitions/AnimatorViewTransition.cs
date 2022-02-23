using System;
using UnityEngine;

namespace com.spector.views.Transitions
{
    
    [RequireComponent(typeof(Animator))]
    public class AnimatorViewTransition : MonoBehaviour, IViewTransition
    {
        private Animator _animator;
        private static readonly int _showTrigger = Animator.StringToHash("Show");

        private Action _onShowing;
        private Action _onHiding;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Perform custom animation to show view and let others know when we are showing
        /// </summary>
        /// <param name="onShowing"></param>
        public void Show(Action onShowing = null)
        {
            gameObject.SetActive(true);
            _onShowing = onShowing;
            _animator.SetBool(_showTrigger, true);
        }

        /// <summary>
        /// Perform custom animation to hide view and let others know
        /// </summary>
        /// <param name="onHiding"></param>
        public void Hide(Action onHiding = null)
        {
            _onHiding = onHiding;
            _animator.SetBool(_showTrigger, false);
        }

        /// <summary>
        /// Animation event called from Timeline when view has finished showing.
        /// </summary>
        private void OnShowing()
        {
            _onShowing?.Invoke();
        }

        /// <summary>
        /// Animation event called from Timeline when view has finished hiding
        /// </summary>
        private void OnHiding()
        {
            _onHiding?.Invoke();
        }
    }
    
    
}