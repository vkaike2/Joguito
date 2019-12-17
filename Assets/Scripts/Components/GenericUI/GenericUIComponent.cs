using Assets.Scripts.Managers.UI;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Components.GenericUI
{
    /// <summary>
    ///     Informs the UIManager that player shouldn't move if clicks on it
    /// </summary>
    [RequireComponent(typeof(EventTrigger))]
    public class GenericUIComponent : BaseComponent
    {
        #region PUBLIC ATRIBUTES
        public bool MouseInUI { get; private set; }
        #endregion

        #region PRIVATE ATRIBUTES
        private UIManager _uiManager;
        private bool _canControll = true;
        #endregion

        #region PUBLIC METHODS
        public void SetMouseInUi(bool value)
        {
            _canControll = false;
            MouseInUI = value;
        }
        #endregion

        #region UNITY METHODS
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
            if (_canControll) MouseInUI = false;
        }

        private void OnDestroy()
        {
            _uiManager.RemoveGenericUIComponent(this);
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void ValidateValues()
        {
            if (_uiManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_uiManager), this.gameObject.name));
        }

        protected override void SetInitialValues()
        {
            _uiManager = GameObject.FindObjectOfType<UIManager>();
        }
        #endregion
    }
}
