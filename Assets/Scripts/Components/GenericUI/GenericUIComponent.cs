using Assets.Scripts.Managers.UI;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Components.GenericUI
{
    [RequireComponent(typeof(EventTrigger))]
    public class GenericUIComponent : BaseComponent/*, IPointerEnterHandler, IPointerExitHandler*/
    {
        private UIManager _uiManager;

        public bool MouseInUI { get; private set; }

        private bool _canControll = true;

        public GameObject ThisGameObject => this.gameObject;

        private void Start()
        {
            EventTrigger trigger = GetComponent<EventTrigger>();
            EventTrigger.Entry pointerEnter = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerEnter,
            };
            pointerEnter.callback.AddListener((data) =>
            {
                if (_canControll)
                    MouseInUI = true;
            });
            trigger.triggers.Add(pointerEnter);

            EventTrigger.Entry pointerExit = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerExit,
            };
            pointerExit.callback.AddListener((data) =>
            {
                if (_canControll)
                    MouseInUI = false;
            });
            trigger.triggers.Add(pointerExit);

            _uiManager.SetGenericUIComponent(this);
        }

        private void OnMouseOver()
        {
            MouseInUI = true;
        }

        private void OnMouseExit()
        {
            MouseInUI = false;
        }

        private void OnDisable()
        {
            if (_canControll)
                MouseInUI = false;
        }
        private void OnDestroy()
        {
            _uiManager.RemoveGenericUIComponent(this);
        }

        public void SetMouseInUi(bool value)
        {
            _canControll = false;
            MouseInUI = value;
        }

        protected override void ValidateValues()
        {
            if (_uiManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_uiManager), this.gameObject.name));
        }
        protected override void SetInitialValues()
        {
            _uiManager = GameObject.FindObjectOfType<UIManager>();
        }

        //public void OnPointerEnter(PointerEventData eventData)
        //{
        //    Debug.Log("Enter" + MouseInUI);
        //    //MouseInUI = true;
        //}

        //public void OnPointerExit(PointerEventData eventData)
        //{
        //    Debug.Log("Exit" + MouseInUI);
        //    //MouseInUI = false;
        //}
    }
}
