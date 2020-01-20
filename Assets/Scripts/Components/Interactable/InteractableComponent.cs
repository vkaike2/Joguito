using System;
using Assets.Scripts.Components.ActionSlot;
using Assets.Scripts.Components.Combat;
using Assets.Scripts.Components.CombatAttributes;
using Assets.Scripts.Components.Inventory;
using Assets.Scripts.Components.ItemDrop;
using Assets.Scripts.Components.PlantSpot;
using Assets.Scripts.Components.Stomach;
using Assets.Scripts.DTOs;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Managers.UI;
using Assets.Scripts.ScriptableComponents.Item;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Components.Interactable
{
    /// <summary>
    ///     Make possible to the player interact with other prefabs
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class InteractableComponent : BaseComponent
    {
        #region PUBLIC ATRIBUTES
        public bool Active => true;
        public bool IsPlantingOrEating => _isEating || _isPlanting;

        public bool CanPickupItem => _canPickupItem;
        public bool CanPlant => _canPlant;
        public bool CanTakeSeed => _canTakeSeed;
        public bool CanTakeFlower => _canTakeFlower;
        public bool CanEatFlower => _canEatFlower;
        public bool MouseIsOver => _mouseIsOver;
        #endregion

        #region SERIALIZABLE ATRIBUTES
        [Header("Required Fields")]
        [SerializeField]
        private InventoryComponent _inventoryComponent;

        [SerializeField]
        private GameObject _itemDropPrefab;

        [Header("Configuration Fields")]
        [SerializeField]
        private bool _canPickupItem;
        [SerializeField]
        private bool _canPlant;
        [SerializeField]
        private bool _canTakeSeed;
        [SerializeField]
        private bool _canTakeFlower;
        [SerializeField]
        private bool _canEatFlower;
        [SerializeField]
        private bool _canAttack;
        #endregion

        #region PRIVATE ATRIBUTES
        private StomachComponent _stomachComponent;
        private InputManager _inputManager;
        private PlayerStateManager _playerStateManager;
        private UIManager _uiManager;
        private EnumInteractableState _currentInteractableState;
        private int? _interactableInstanceId;
        private Animator _animator;
        private InteractableAnimatorVariables _animatorVariables;
        private bool _isPlanting = false;
        private bool _isEating = false;
        private bool _mouseIsOver = false;
        private CombatAttributesComponent _combatAttributesComponent;
        #endregion

        #region PUBLIC METHODS
        public bool CheckIfCanAtack()
        {
            return _canAttack;
        }

        public void RemoveInteractableState()
        {
            _currentInteractableState = EnumInteractableState.Nothing;
            _interactableInstanceId = null;
            _animatorVariables.ResetAuxiliarObjects();
        }

        public void SetInteractableState(EnumInteractableState interactableState, int instanceId)
        {
            if (!ValidateIfPlayerCanDoThings(interactableState)) return;

            _currentInteractableState = interactableState;
            _interactableInstanceId = instanceId;
        }

        public bool IsInteracting()
        {
            return _currentInteractableState != EnumInteractableState.Nothing;
        }

        public bool IsAttackingThisMonster(int monsterInstanceId)
        {
            return _currentInteractableState == EnumInteractableState.Atack && _interactableInstanceId == monsterInstanceId;
        }

        public void Animator_ToPlant()
        {
            if (_animatorVariables.PlantSpotComponent == null) Debug.LogError("The auxiliar PlantSpotComponent Cannot be null while Planting");
            if (_animatorVariables.SelectedActionSlot == null) Debug.LogError("The auxiliar SelectedActionSlot Cannot be null while Planting");

            ItemDTO itemDto = _animatorVariables.SelectedActionSlot.GetOneItem();
            _animatorVariables.PlantSpotComponent.PlantThisSeed(itemDto);
            this.RemoveInteractableState();
            _isPlanting = false;
        }

        public void Animator_ToEat()
        {
            if (_animatorVariables.PlantSpotComponent == null) // => Is used by drag
            {
                _stomachComponent.AddFood(new ItemDTO()
                {
                    Item = _animatorVariables.Flower.Item,
                    Amount = 1
                });
            }
            else
            {
                ItemDTO flower = _animatorVariables.PlantSpotComponent.TakeFlower();
                _stomachComponent.AddFood(new ItemDTO()
                {
                    Item = flower.Item,
                    Amount = 1
                });
                flower.Amount--;

                _animatorVariables.PlantSpotComponent.ResetPlantSpot();

                if (flower.Amount <= 0)
                {
                    this.RemoveInteractableState();
                    _isEating = false;
                    return;
                }

                if (_inventoryComponent.InventoryIsFull())
                {
                    GameObject itemDropGameObject = Instantiate(_itemDropPrefab, this.gameObject.transform.position, Quaternion.identity);
                    itemDropGameObject.GetComponentInChildren<ItemDropComponent>().SetCurrentItem(flower);
                }
                else
                {
                    _inventoryComponent.AddItem(flower);
                }
            }
            _animatorVariables.ResetAuxiliarObjects();
            this.RemoveInteractableState();
            _isEating = false;
        }

        public bool PlayerCanEatFlower()
        {
            if (_stomachComponent.StomachCantAcceptNewFood())
            {
                Debug.LogError("Player stomach Is Full while tryng to eat a flower!");
                return false;
            }

            return true;
        }

        public void EatFlowerByDrag(ItemDTO flower)
        {
            _isEating = true;
            _animatorVariables.Flower = flower;
            _animator.SetTrigger(_animatorVariables.Eat);
        }
        #endregion

        #region UNITY METHODS
        private void OnMouseEnter()
        {
            _mouseIsOver = true;
        }

        private void OnMouseExit()
        {
            _mouseIsOver = false;
        }
        #endregion

        #region COLLIDER METHODS
        private void OnTriggerEnter2D(Collider2D collision)
        {
            ManageEveryInteractionType(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            ManageEveryInteractionType(collision);
        }
        #endregion

        #region PRIVATE METHODS
        private bool ValidateIfPlayerCanDoThings(EnumInteractableState interactableState)
        {
            if (!_canPickupItem && interactableState == EnumInteractableState.PickupItem)
            {
                Debug.LogError("You cannot pickup any item!");
                return false;
            }

            if (!_canPlant && interactableState == EnumInteractableState.Plant)
            {
                Debug.LogError("You cannot plant any item!");
                return false;
            }

            if (!_canTakeSeed && interactableState == EnumInteractableState.TakeSeed)
            {
                Debug.LogError("You cannot take this seed!");
                return false;
            }

            if (!_canTakeFlower && interactableState == EnumInteractableState.TakeFlower)
            {
                Debug.LogError("You cannot take this flower!");
                return false;
            }

            if (!_canEatFlower && interactableState == EnumInteractableState.EatFlower)
            {
                Debug.LogError("You cannot eat Flower!");
                return false;
            };

            if(!_canAttack && interactableState == EnumInteractableState.Atack)
            {
                Debug.LogError("You cannot Atack!");
                return false;
            }

            return true;
        }

        private void ManageEveryInteractionType(Collider2D collision)
        {
            if (_interactableInstanceId == null) return;
            if (_isPlanting) return;
            if (_isEating) return;

            switch (_currentInteractableState)
            {
                case EnumInteractableState.Nothing:
                    break;
                case EnumInteractableState.PickupItem:
                    this.PickUpItem(collision);
                    break;
                case EnumInteractableState.Plant:
                    this.ToPlant(collision);
                    break;
                case EnumInteractableState.TakeSeed:
                    this.ToTakeSeed(collision);
                    break;
                case EnumInteractableState.TakeFlower:
                    this.TakeFlower(collision);
                    break;
                case EnumInteractableState.EatFlower:
                    this.EatFlower(collision);
                    break;
                case EnumInteractableState.Atack:
                    this.Atack(collision);
                    break;
                default:
                    break;
            }

        }

        private void PickUpItem(Collider2D collision)
        {
            ItemDropComponent itemDropComponent = collision.gameObject.GetComponentInParent<ItemDropComponent>();
            if (itemDropComponent == null) return;
            if (itemDropComponent.GetInstanceID() != _interactableInstanceId) return;

            if (_inventoryComponent.InventoryIsFull())
            {
                Debug.LogError("Inventory Is Full while tryng to harverst seeds!");
                return;
            }

            ItemDTO itemDto = itemDropComponent.PickupThisItem();
            itemDropComponent.DestroyGameObject();
            _inventoryComponent.AddItem(itemDto);
            this.RemoveInteractableState();

        }
        private void ToPlant(Collider2D collision)
        {
            PlantSpotComponent plantSpotComponent = collision.gameObject.GetComponent<PlantSpotComponent>();
            if (plantSpotComponent == null) return;

            if (plantSpotComponent.GetInstanceID() != _interactableInstanceId) return;

            ActionSlotComponent selectedActionSlot = _uiManager.GetSelectedActionSlot();
            if (!plantSpotComponent.CanAcceptNewSeed()) return;
            if (!selectedActionSlot.ItemCanBeUsedToPlant()) return;

            _isPlanting = true;

            _animatorVariables.PlantSpotComponent = plantSpotComponent;
            _animatorVariables.SelectedActionSlot = selectedActionSlot;
            _animator.ResetTrigger(_animatorVariables.PlantSeed);
            _animator.SetTrigger(_animatorVariables.PlantSeed);
        }
        private void ToTakeSeed(Collider2D collision)
        {
            PlantSpotComponent plantSpotComponent = this.HarvestPlnantValidatios(collision);
            if (plantSpotComponent == null) return;

            if (_inventoryComponent.InventoryIsFull()) return;

            ItemDTO seeds = plantSpotComponent.TakeSeeds();
            _inventoryComponent.AddItem(seeds);
            plantSpotComponent.ResetPlantSpot();
            this.RemoveInteractableState();
        }
        private void TakeFlower(Collider2D collision)
        {
            PlantSpotComponent plantSpotComponent = this.HarvestPlnantValidatios(collision);
            if (plantSpotComponent == null) return;

            if (_inventoryComponent.InventoryIsFull())
            {
                Debug.LogError("Inventory Is Full while tryng to harverst seeds!");
                return;
            }

            ItemDTO flower = plantSpotComponent.TakeFlower();
            _inventoryComponent.AddItem(flower);
            plantSpotComponent.ResetPlantSpot();
            this.RemoveInteractableState();
        }
        private void EatFlower(Collider2D collision)
        {
            PlantSpotComponent plantSpotComponent = this.HarvestPlnantValidatios(collision);
            if (plantSpotComponent == null) return;

            if (_stomachComponent.StomachCantAcceptNewFood())
            {
                Debug.LogError("Player stomach Is Full while tryng to eat a flower!");
                this.RemoveInteractableState();
                return;
            }

            _isEating = true;
            _animatorVariables.PlantSpotComponent = plantSpotComponent;
            _animator.SetTrigger(_animatorVariables.Eat);
        }
        private void Atack(Collider2D collision)
        {
            CombatComponent combatComponent = collision.gameObject.GetComponentInParent<CombatComponent>();

            if (combatComponent == null) return;
            if (combatComponent.GetInstanceID() != _interactableInstanceId) return;

            combatComponent.StartDefenseOperation(_combatAttributesComponent);
        }

        private PlantSpotComponent HarvestPlnantValidatios(Collider2D collision)
        {
            PlantSpotComponent plantSpotComponent = collision.gameObject.GetComponent<PlantSpotComponent>();
            if (plantSpotComponent == null) return null;
            if (plantSpotComponent.GetInstanceID() != _interactableInstanceId) return null;
            if (!plantSpotComponent.IsReadyToHarvest()) return null;

            return plantSpotComponent;
        }
        #endregion

        #region ABSTRACT ATRIBUTES
        protected override void SetInitialValues()
        {
            _inputManager = GameObject.FindObjectOfType<InputManager>();
            _playerStateManager = GameObject.FindObjectOfType<PlayerStateManager>();
            _uiManager = GameObject.FindObjectOfType<UIManager>();
            _animator = this.GetComponent<Animator>();
            _currentInteractableState = EnumInteractableState.Nothing;
            _stomachComponent = this.GetComponent<StomachComponent>();
            _combatAttributesComponent = this.GetComponent<CombatAttributesComponent>();

            _animatorVariables = new InteractableAnimatorVariables();
        }

        protected override void ValidateValues()
        {
            //if (_inputManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inputManager), this.gameObject.name));
            //if (_playerStateManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_playerStateManager), this.gameObject.name));
            //if (_uiManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_uiManager), this.gameObject.name));
            //if (_itemDropPrefab == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_itemDropPrefab), this.gameObject.name));
        }
        #endregion
    }
}
