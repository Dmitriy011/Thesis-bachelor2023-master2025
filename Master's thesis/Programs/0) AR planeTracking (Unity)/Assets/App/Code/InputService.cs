using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Code
{
    public class InputService : MonoBehaviour,
        IPointerClickHandler, IInputService
    {
        public event Action<PointerEventData> OnClick;

        private void Awake() => 
            Locator.Set<IInputService>(this);

        public void OnPointerClick(PointerEventData eventData) =>
            OnClick?.Invoke(eventData);
    }
}