using Assets.Scripts.Managers.UI;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using Assets.Scripts.Components.Interactable;
using Assets.Scripts.Components.Stomach;
using Assets.Scripts.Structure.Player;
using Assets.Scripts.Components.Draggable;

namespace Assets.Scripts.Managers.PlayerState
{
    /// <summary>
    ///     Manage every state from player
    /// </summary>
    public class PlayerStateManager : BaseManager
    {
        #region PUBLIC ATRIBUTES
        public bool PlayerCantMove
        {
            get
            {

                List<Components.GenericUI.GenericUIComponent> test = _uiManager.GenericUIList.Where(e => e.MouseInUI).ToList();
                bool mouseIsOverSomeUi = _uiManager.GenericUIList != null && _uiManager.GenericUIList.Any(e => e.MouseInUI);

                //bool playerIspooping = false;
                //StomachComponent stomachComponent = this.GetActivePlayerStructure().GetStomachComponent();
                //if (stomachComponent != null)
                //    playerIspooping = stomachComponent.IsPooping;

                bool playerIsPlantingOrEating = false;
                InteractableComponent interactableCompoment = this.GetActivePlayerStructure().GetInteractableComponent();
                if (interactableCompoment != null)
                    playerIsPlantingOrEating = interactableCompoment.IsPlantingOrEating;

                if (PlayerIsDoingSomeAction)
                    return true;

                return mouseIsOverSomeUi || playerIsPlantingOrEating;
                //return mouseIsOverSomeUi || playerIspooping || playerIsPlantingOrEating;
            }
        }

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
                activePlayerStructure.ActivatePlayerStructure(true);

        }
        #endregion

        #region UNITY METHODS
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (!_playerStrucutreList.Any() && _playerStrucutreList.Count > 1) return;

                int? intexOfSelectedOne = null;
                for (int i = 0; i < _playerStrucutreList.Count; i++)
                {
                    if (_playerStrucutreList[i].IsActive)
                    {
                        _playerStrucutreList[i].ActivatePlayerStructure(false);
                        intexOfSelectedOne = i;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (intexOfSelectedOne < _playerStrucutreList.Count - 1)
                {
                    _playerStrucutreList[intexOfSelectedOne.GetValueOrDefault() + 1].ActivatePlayerStructure(true);
                }
                else
                {
                    _playerStrucutreList[0].ActivatePlayerStructure(true);
                }
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
        }
        #endregion
    }
}
