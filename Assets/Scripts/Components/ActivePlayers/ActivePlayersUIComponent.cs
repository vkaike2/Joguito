using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Structure.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Components.ActivePlayers
{
    public class ActivePlayersUIComponent : BaseComponent
    {
        #region SERIALIZABLE FIELDS
        [Header("Required Fields")]
        [SerializeField]
        private GameObject _ActivePlayerSlotPrefab;
        #endregion

        #region PRIVATE ATRIBUTES
        private List<ActivePlayerSlotComponent> _playerSlotComponentList;
        private PlayerStateManager _playerStateManager;
        private InputManager _inputManager;
        #endregion

        #region PUBLIC METHODS
        public int CreateNewPlayerSlotCompoennt(PlayerStructure playerStructure)
        {
            ActivePlayerSlotComponent nextSlot = GameObject.Instantiate(_ActivePlayerSlotPrefab, this.transform).GetComponent<ActivePlayerSlotComponent>();
            nextSlot.SetPlayerStructure(playerStructure);
            _playerSlotComponentList.Add(nextSlot);
            return nextSlot.GetInstanceID();
        }

        public void ActivatePlayerSlot(int instanceId)
        {
            foreach (ActivePlayerSlotComponent playerSlot in _playerSlotComponentList)
            {
                playerSlot.ActivateSlot(playerSlot.GetInstanceID() == instanceId);
            }
        }

        public void DesactivePlayerSlot(int instanceId)
        {
            ActivePlayerSlotComponent currentSlot = _playerSlotComponentList.FirstOrDefault(e => e != null && e.GetInstanceID() == instanceId);
            if (currentSlot is null) return;
            _playerSlotComponentList.Remove(currentSlot);

            Destroy(currentSlot.gameObject);

            if (currentSlot.IsActive)
                _playerStateManager.ActiveNewPlayerStructure(_playerSlotComponentList.Select(e => e.CurrentPLayerStructureInstanceId).FirstOrDefault());
        }
        #endregion

        #region UNITY METHODS
        private void Update()
        {
            SelectPlayerUsingNumbers();
        }

        #endregion

        #region ABSTRACT METHODS
        private void SelectPlayerUsingNumbers()
        {
            if (_inputManager.Alpha01 > 0)
            {
                if (_playerSlotComponentList[0] is null) return;
                _playerSlotComponentList[0].Active_OnClick();
            }
            else if (_inputManager.Alpha02 > 0)
            {
                if (_playerSlotComponentList[1] is null) return;
                _playerSlotComponentList[1].Active_OnClick();
            }
            else if (_inputManager.Alpha03 > 0)
            {
                if (_playerSlotComponentList[2] is null) return;
                _playerSlotComponentList[2].Active_OnClick();
            }
            else if (_inputManager.Alpha04 > 0)
            {
                if (_playerSlotComponentList[3] is null) return;
                _playerSlotComponentList[3].Active_OnClick();
            }
        }

        protected override void SetInitialValues()
        {
            _inputManager = GameObject.FindObjectOfType<InputManager>();
            _playerStateManager = GameObject.FindObjectOfType<PlayerStateManager>();
            _playerSlotComponentList = this.GetComponentsInChildren<ActivePlayerSlotComponent>().ToList();
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}
