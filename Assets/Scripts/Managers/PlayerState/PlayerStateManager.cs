using Assets.Scripts.Managers.UI;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using Assets.Scripts.Components.Interactable;
using Assets.Scripts.Components.Stomach;
using Assets.Scripts.Structure.Player;
using Assets.Scripts.Components.Draggable;
using Assets.Scripts.Managers.Inputs;

namespace Assets.Scripts.Managers.PlayerState
{
    /// <summary>
    ///     Manage every state from player
    /// </summary>
    public class PlayerStateManager : BaseManager
    {
        #region PUBLIC ATRIBUTES
        public bool PlayerIsDoingSomeAction => _inventoryDraggableItemComponnent != null && _inventoryDraggableItemComponnent.IsDragging;
        #endregion

        #region SERIALIZABLE ATRIBUTES
        [Header("RequiredFields")]
        [SerializeField]
        private UIManager _uiManager;
        #endregion

        #region PRIVATE ATRIBUTES
        private List<PlayerStructure> _playerStrucutreList;
        private InventoryDraggableItemComponent _inventoryDraggableItemComponnent;
        private InputManager _inputManager;
        private bool _playerSlotPressed = false;
        private Audio
        #endregion

        #region PUBLIC METHODS
        public void SetNewPlayerStrucutre(PlayerStructure playerStructure)
        {
            if (!_playerStrucutreList.Any(e => e.IsActive) && playerStructure.IsMainPlayer)
            {
                playerStructure.ActivatePlayerStructure(true);
            }

            _playerStrucutreList.Add(playerStructure);
        }

        public void RemoveOnePlayerStructure(PlayerStructure playerStructure)
        {
            _playerStrucutreList.Remove(playerStructure);
        }

        public PlayerStructure GetActivePlayerStructure()
        {
            PlayerStructure playerStructure = _playerStrucutreList.FirstOrDefault(e => e.IsActive);
            if (playerStructure is null)
                playerStructure = _playerStrucutreList.FirstOrDefault(e => e.IsMainPlayer);

            return playerStructure;
        }

        public List<InteractableComponent> GetAllInteractableComponents()
        {
            return _playerStrucutreList.Select(e => e.GetInteractableComponent()).ToList();
        }

        public void ActiveNewPlayerStructure(int playerStructureInstanceId)
        {
            foreach (PlayerStructure playerStructure in _playerStrucutreList)
            {
                playerStructure.ActivatePlayerStructure(false);
            }

            PlayerStructure activePlayerStructure = _playerStrucutreList.FirstOrDefault(e => e.GetInstanceID() == playerStructureInstanceId);
            if (activePlayerStructure != null)
            {

                activePlayerStructure.ActivatePlayerStructure(true);
            }

        }
        #endregion

        #region UNITY METHODS
        private void Update()
        {
            SwapBetweenPlayerSlots();
        }
        #endregion

        #region PRIVATE METHODS
        private void SwapBetweenPlayerSlots()
        {
            if (_inputManager.ChangePlayerSlot != 0 && !_playerSlotPressed)
            {
                _playerSlotPressed = true;
                if (_playerStrucutreList.Count == 1) return;

                int intexOfSelectedOne = 0;
                for (int i = 0; i < _playerStrucutreList.Count; i++) // => Desactivate every PlayerSlot
                {
                    if (!_playerStrucutreList[i].IsActive) continue;

                    _playerStrucutreList[i].ActivatePlayerStructure(false);
                    intexOfSelectedOne = i;
                }

                if (_inputManager.ChangePlayerSlot == 1) // => Select the next Right
                {
                    intexOfSelectedOne++;
                    if (intexOfSelectedOne > _playerStrucutreList.Count -1)
                    {
                        _playerStrucutreList[0].ActivatePlayerStructure(true);
                        return;
                    }
                }
                else if (_inputManager.ChangePlayerSlot == -1) // => Select the next Left
                {
                    intexOfSelectedOne--;
                    if (intexOfSelectedOne < 0)
                    {
                        _playerStrucutreList[_playerStrucutreList.Count - 1].ActivatePlayerStructure(true);
                        return;
                    }
                }
                _playerStrucutreList[intexOfSelectedOne].ActivatePlayerStructure(true);
            }
            else if (_inputManager.ChangePlayerSlot == 0 && _playerSlotPressed)
            {
                _playerSlotPressed = false;
            }
        }
        #endregion

        #region ABSTRACT ATRIBUTES
        protected override void ValidateValues()
        {
            if (_uiManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_uiManager), this.gameObject.name));
            _inventoryDraggableItemComponnent = GameObject.FindObjectOfType<InventoryDraggableItemComponent>();
        }
        protected override void SetInitialValues()
        {
            _playerStrucutreList = new List<PlayerStructure>();
            _inputManager = GameObject.FindObjectOfType<InputManager>();
        }
        #endregion
    }
}
