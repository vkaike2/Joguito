using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Components.GenericUI
{
    public class GenericUIComponent : MonoBehaviour, IGenericUI
    {
        [SerializeField]
        private EnumUIType _type;

        private bool _mouseInUI = false;

        public bool MouseInUI => _mouseInUI;

        public EnumUIType Type => _type;

        public GameObject ThisGameObject => this.gameObject;

        private void Start()
        {
            EventTrigger trigger = GetComponent<EventTrigger>();
            EventTrigger.Entry pointerEnter = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerEnter,
            };
            pointerEnter.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });
            trigger.triggers.Add(pointerEnter);

            EventTrigger.Entry pointerExit = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerExit,
            };
            pointerExit.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
            trigger.triggers.Add(pointerExit);

            //EventTrigger.Entry pointerDown = new EventTrigger.Entry()
            //{
            //    eventID = EventTriggerType.,
            //};
            //pointerDown.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
            //trigger.triggers.Add(pointerDown);
        }

        public void OnPointerEnterDelegate(PointerEventData eventData)
        {
            _mouseInUI = true;
        }

        public void OnPointerExitDelegate(PointerEventData eventData)
        {
            _mouseInUI = false;
        }

        //public void OnPointerDownDelegate(PointerEventData eventData)
        //{
        //    Debug.Log(this.gameObject.name);
        //}
    }
}
