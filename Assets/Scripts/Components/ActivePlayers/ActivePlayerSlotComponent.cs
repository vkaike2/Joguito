using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Structure.Player;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Components.ActivePlayers
{
    public class ActivePlayerSlotComponent : BaseComponent
    {
        #region PUBLIC ATRIBUTES
        public bool IsActive { get; private set; }
        public int CurrentPLayerStructureInstanceId => _currentPlayerStructure.GetInstanceID();
        #endregion

        #region PRIVATE ATRIBUTES
        private Image image;
        private PlayerStructure _currentPlayerStructure;
        private PlayerStateManager _playerStateManager;
        #endregion

        #region PUBLIC METHODS
        public void ActivateSlot(bool value)
        {
            IsActive = value;

            if (value)
                image.color = Color.red;
            else
                image.color = Color.white;
        }

        public void SetPlayerStructure(PlayerStructure playerStructure)
        {
            image.sprite = playerStructure.SpriteForActiveStatus;
            _currentPlayerStructure = playerStructure;
        }

        public void Active_OnClick()
        {
            if (IsActive) return;
            _playerStateManager.ActiveNewPlayerStructure(_currentPlayerStructure.GetInstanceID());
        }
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            EventTrigger trigger = GetComponent<EventTrigger>();
            EventTrigger.Entry pointerClick = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerClick,
            };
            pointerClick.callback.AddListener((data) => Active_OnClick());
            trigger.triggers.Add(pointerClick);
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _playerStateManager = GameObject.FindObjectOfType<PlayerStateManager>();
        }

        protected override void ValidateValues()
        {
            image = this.GetComponent<Image>();
        }
        #endregion
    }
}
