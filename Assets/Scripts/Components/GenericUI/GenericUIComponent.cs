using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Components.GenericUI
{
    public class GenericUIComponent : MonoBehaviour, IGenericUI
    {
        private bool _mouseInUI = false;
        public bool MouseInUI => _mouseInUI;

        private void Start()
        {
            EventTrigger trigger = GetComponent<EventTrigger>();
            EventTrigger.Entry pointerEnter = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerEnter,
            };
            pointerEnter.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });

            EventTrigger.Entry pointerExit = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerEnter,
            };
            pointerExit.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });

            trigger.triggers.Add(pointerExit);
        }

        public void OnPointerEnterDelegate(PointerEventData eventData)
        {
            _mouseInUI = true;
        }

        public void OnPointerExitDelegate(PointerEventData eventData)
        {
            _mouseInUI = false;
        }
    }
}
