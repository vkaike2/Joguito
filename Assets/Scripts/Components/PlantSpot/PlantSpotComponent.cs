using System;
using Assets.Scripts.Components.Plant;
using Assets.Scripts.DTOs;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.ScriptableComponents.Item;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Components.PlantSpot
{
    [RequireComponent(typeof(Animator))]
    public class PlantSpotComponent : BaseComponent
    {
        [Header("Requird Fields")]
        [SerializeField]
        private PlantComponent _plantComponent;
        [SerializeField]
        private Canvas _internalCanvas;

        [Header("Configuration")]
        [SerializeField]
        [Range(0, 3)]
        private float _radioToInteract;

        private Animator _animator;
        private PlantSpotAnimatorVariables _animatorVariables;
        private PlayerStateManager _playerState;
        private InputManager _inputManager;
        private EnumPlantSpotState _plantSpotState;

        private bool _mousePressed = false;

        private void OnMouseOver()
        {
            this.ChangePlantSpotMouseOverAnimation(true);
            this.ManageThePlayersClick();
        }

        private void OnMouseExit()
        {
            this.ChangePlantSpotMouseOverAnimation(false);
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

        private void ManageThePlayersClick()
        {

            if (_inputManager.MouseLeftButton == 1 && !_mousePressed)
            {
                _mousePressed = true;

                ToggleInternalUIMenu();
                StartPlantingProccess();
            }
            else if (_inputManager.MouseLeftButton == 0 && _mousePressed)
            {
                _mousePressed = false;
            }
        }

        private void StartPlantingProccess()
        {
            if (_plantSpotState != EnumPlantSpotState.Empty) return;

            _playerState.GetActiveInteractableComponent().SetInteractableState(Interactable.EnumInteractableState.Plant, this.GetInstanceID());
            _playerState.GetActiveMovementMouseComponent().ObjectGoTo(this.transform.position, _radioToInteract);
        }

        public void Btn_TakeSeed()
        {
            _playerState.GetActiveInteractableComponent().SetInteractableState(Interactable.EnumInteractableState.TakeSeed, this.GetInstanceID());
            _playerState.GetActiveMovementMouseComponent().ObjectGoTo(this.transform.position, _radioToInteract);
        }

        public ItemDTO TakeSeeds()
        {
            if (_plantSpotState != EnumPlantSpotState.Ready)
            {
                Debug.LogError("You are trying to get the seeds of a plant spot thats isn't ready!");
            }

            ItemScriptable seedType = _plantComponent.CurrentScriptableSeedType;
            return new ItemDTO()
            {
                Item = seedType,
                Amount = seedType.Flower.GetAmoutSeedItGives
            };
        }

        public void ResetPlantSpot()
        {
            _plantComponent.RemovePlant();
            _plantSpotState = EnumPlantSpotState.Empty;
        }

        internal void SetState(EnumPlantSpotState newState, int parentIntanceID)
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

        protected override void SetInitialValues()
        {
            _playerState = GameObject.FindObjectOfType<PlayerStateManager>();
            _inputManager = GameObject.FindObjectOfType<InputManager>();
            _animator = this.GetComponent<Animator>();
            _animatorVariables = new PlantSpotAnimatorVariables();
            _plantSpotState = EnumPlantSpotState.Empty;

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

        internal bool IsReadyToHarvest()
        {
            return _plantSpotState == EnumPlantSpotState.Ready;
        }
    }
}
