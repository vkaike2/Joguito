using System;
using System.Collections;
using System.Linq;
using Assets.Scripts.Components.GenericUI;
using Assets.Scripts.Components.Interactable;
using Assets.Scripts.Components.Plant;
using Assets.Scripts.DTOs;
using Assets.Scripts.Interface;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Managers.UI;
using Assets.Scripts.ScriptableComponents.Item;
using Assets.Scripts.Structure.Player;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Components.PlantSpot
{
    /// <summary>
    ///     Representes the place where the player plant and harvest
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class PlantSpotComponent : BaseComponent, IPlantable
    {
        #region SERIALIZABLE ATRIBUTES
        [Header("Requird Fields")]
        [SerializeField]
        private PlantComponent _plantComponent;
        [SerializeField]
        private Canvas _internalCanvas;

        [Header("Configuration")]
        [SerializeField]
        [Range(0, 3)]
        private float _radioToInteract;
        [SerializeField]
        private int _interactOrder;
        #endregion

        #region PRIVATE ATRIBUTES
        private Animator _animator;
        private PlantSpotAnimatorVariables _animatorVariables;
        private PlayerStateManager _playerState;
        private InputManager _inputManager;
        private EnumPlantSpotState _plantSpotState;
        private UIManager _uiManager;
        private int _instanceIDForGenereciUI;
        private Coroutine _mouseOverCoroutine;
        #endregion

        #region PUBLIC METHODS
        public void Btn_TakeSeed()
        {
            this.IniciateSomeInteraction(EnumInteractableState.TakeSeed);
        }
        public void Btn_TakeFlower()
        {
            this.IniciateSomeInteraction(EnumInteractableState.TakeFlower);
        }
        public void Btn_EatFlower()
        {
            this.IniciateSomeInteraction(EnumInteractableState.EatFlower);
        }

        public ItemDTO TakeSeeds()
        {
            if (_plantSpotState != EnumPlantSpotState.Ready)
                Debug.LogError("You are trying to get the seeds of a plant spot thats isn't ready!");

            ItemScriptable seedType = _plantComponent.CurrentScriptableSeedType;
            return new ItemDTO()
            {
                Item = seedType,
                Amount = seedType.Flower.GetAmoutSeedItGives
            };
        }

        public ItemDTO TakeFlower()
        {
            if (_plantSpotState != EnumPlantSpotState.Ready)
                Debug.LogError("You are trying to get the flower of a plant spot thats isn't ready!");

            ItemScriptable flower = _plantComponent.CurrentScriptableSeedType.Flower;

            return new ItemDTO()
            {
                Item = flower,
                Amount = flower.AmoutFlowerItGivesWhenHarvest
            };
        }

        public void ResetPlantSpot()
        {
            _plantComponent.RemovePlant();
            _plantSpotState = EnumPlantSpotState.Empty;
        }

        public void SetState(EnumPlantSpotState newState, int parentIntanceID)
        {
            if (parentIntanceID != this.transform.parent.gameObject.GetInstanceID()) Debug.LogError("You are trying to change the state of a a gameplant that isnt yours;");
            _plantSpotState = newState;
        }

        public bool CanAcceptNewSeed() => _plantComponent.CanAcceptNewPlant;

        public void PlantThisSeed(ItemDTO itemDto)
        {
            _plantComponent.SetPlant(itemDto);
            _plantSpotState = EnumPlantSpotState.HasPlant;
        }

        public bool IsReadyToHarvest()
        {
            return _plantSpotState == EnumPlantSpotState.Ready;
        }
        #endregion

        #region INTERFACE METHODS
        public bool Interact()
        {
            PlayerStructure playerStructure = _playerState.GetActivePlayerStructure();
            InteractableComponent interactableComponent = playerStructure.GetInteractableComponent();
            if (interactableComponent is null) return false;

            if (!interactableComponent.CanPlant && !interactableComponent.CanTakePlant && !interactableComponent.CanTakeSeed) return false;

            ToggleInternalUIMenu();
            StartPlantingProccess();

            return true;
        }

        public int Order()
        {
            return _interactOrder;
        }

        public void MouseOver(bool value)
        {
            if (_mouseOverCoroutine != null)
                StopCoroutine(_mouseOverCoroutine);

            _mouseOverCoroutine = StartCoroutine(HoverForSomeSeconds(.1f));
        }
        #endregion

        #region COROUTINES 
        IEnumerator HoverForSomeSeconds(float _cdwOn)
        {
            this.ChangePlantSpotMouseOverAnimation(true);
            float _internalCdw = 0f;
            while (_internalCdw <= _cdwOn)
            {
                _internalCdw += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            this.ChangePlantSpotMouseOverAnimation(false);
        }
        #endregion

        #region PRIVATE METHODS
        private void IniciateSomeInteraction(EnumInteractableState interactionState)
        {
            PlayerStructure playerStructure = _playerState.GetActivePlayerStructure();
            InteractableComponent interactableComponent = playerStructure.GetInteractableComponent();
            if (interactableComponent is null) return;

            if (_uiManager.GenericUIList.Any(e => e.MouseInUI && e.GetInstanceID() != _instanceIDForGenereciUI) && interactionState == EnumInteractableState.Plant) return;

            interactableComponent.SetInteractableState(interactionState, this.GetInstanceID());

            playerStructure.GetMovementMouseComponent().ObjectGoTo(this.transform.position, _radioToInteract);
        }

        private void ChangePlantSpotMouseOverAnimation(bool value)
        {
            _animator.SetBool(_animatorVariables.MouseOver, value);
        }

        private void ToggleInternalUIMenu()
        {
            if (_plantSpotState != EnumPlantSpotState.Ready) return;

            _internalCanvas.enabled = true;
        }

        private void StartPlantingProccess()
        {
            if (_plantSpotState != EnumPlantSpotState.Empty) return;
            this.IniciateSomeInteraction(EnumInteractableState.Plant);
        }
        #endregion

        #region ABSTRACT ATRIBUTES
        protected override void SetInitialValues()
        {
            _playerState = GameObject.FindObjectOfType<PlayerStateManager>();
            _inputManager = GameObject.FindObjectOfType<InputManager>();
            _animator = this.GetComponent<Animator>();
            _animatorVariables = new PlantSpotAnimatorVariables();
            _plantSpotState = EnumPlantSpotState.Empty;
            _uiManager = GameObject.FindObjectOfType<UIManager>();

            _internalCanvas.enabled = false;
            if (_radioToInteract == 0) _radioToInteract = 0.1f;
        }

        protected override void ValidateValues()
        {
            if (_animator == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_animator), this.gameObject.name));
            if (_inputManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inputManager), this.gameObject.name));
            if (_playerState == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_playerState), this.gameObject.name));
            if (_plantComponent == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_plantComponent), this.gameObject.name));
            if (_internalCanvas == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_internalCanvas), this.gameObject.name));
        }
        #endregion
    }
}
