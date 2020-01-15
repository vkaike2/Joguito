using Assets.Scripts.Components.ButtHole;
using Assets.Scripts.Components.MovementMouse;
using Assets.Scripts.DTOs;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.ScriptableComponents.Item;
using Assets.Scripts.ScriptableComponents.Poop;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Components.Stomach
{
    /// <summary>
    ///     Represents the player stomach
    /// </summary>
    public class StomachComponent : BaseComponent
    {
        #region PUBLIC ATRIBUTES
        public List<StomachItemDTO> StomachItemList { get; private set; }
        public bool Active => true;
        #endregion

        #region PRIVATE ATRIBUTES
        private StomachUIComponent _stomachUIComponent;
        private InputManager _inputManager;
        private PoopScriptable[] _poopList;
        private const int TOTAL_CAPACITY_OF_FOOD = 3;
        private Animator _animator;
        private StomachAnimatorVariables _animatorVariables;
        private PlayerStateManager _playerStateManager;
        private PoopScriptable _currentPoop;
        private MovementMouseComponent _movementMouseComponent;
        private bool _isPooping = false;
        #endregion

        #region PUBLIC METHODS
        public void AddFood(ItemDTO food)
        {
            if (food.Amount > 1 || food.Amount == 0) Debug.LogError("You can only eat one flower at ounce!");
            if (food.Item.ItemType != EnumItemScriptableType.Flower) Debug.LogError("You can only eat flowerrs in this world!");

            StomachItemDTO stomachItem = new StomachItemDTO()
            {
                Id = Guid.NewGuid(),
                Item = food.Item,
                DigestionPercent = 0
            };

            StomachItemList.Add(stomachItem);
            _stomachUIComponent.AddFood(stomachItem);
            StartCoroutine(StartDigestion(food.Item.DigestionTime, stomachItem));
        }

        public bool StomachCantAcceptNewFood()
        {
            return StomachItemList.Count >= TOTAL_CAPACITY_OF_FOOD;
        }

        public void Animator_StopPoop()
        {
            _movementMouseComponent.Animator_CanMove();
            _isPooping = false;
        }

        public void Aniamtor_SetCurrentPoop()
        {
            this.GetComponentInChildren<ButtHoleComponent>().SetCurrentPoop(_currentPoop);
            _currentPoop = null;
        }
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            this.Activate();
        }

        private void Update()
        {
            ManageTheButHole();
        }

        #endregion

        #region COROUTINES
        IEnumerator StartDigestion(float digestionTime, StomachItemDTO stomachItem)
        {
            float internalCdw = 0f;

            while (internalCdw <= digestionTime)
            {
                internalCdw += Time.deltaTime;
                stomachItem.DigestionPercent = internalCdw / digestionTime;
                _stomachUIComponent.UpdateCdwFillAmountDigestionBar(stomachItem);
                yield return new WaitForFixedUpdate();
            }
        }
        #endregion

        #region PRIVATE METHODS
        private void ManageTheButHole()
        {
            if (_inputManager.PoopButton == 1 && !_isPooping && StomachItemList.Any(e => e.State == EnumStomachItemDTOState.ReadyToPoop))
            {
                _movementMouseComponent.Animator_CantMove();
                _isPooping = true;
                List<StomachItemDTO> foodToPoopList = StomachItemList.Where(e => e.State == EnumStomachItemDTOState.ReadyToPoop).ToList();
                string currentFlowersRecipeInStomach = String.Join("-", foodToPoopList.Select(e => e.Item.GetHashCode()).OrderBy(e => e).ToList());

                _currentPoop = _poopList.FirstOrDefault(e => e.Recipe == currentFlowersRecipeInStomach);

                _animator.SetTrigger(_animatorVariables.Poop);

                List<Guid> foodToPoopIdList = foodToPoopList.Select(e => e.Id).ToList();

                StomachItemList.RemoveAll(e => foodToPoopIdList.Contains(e.Id));
                _stomachUIComponent.RemoveFoods(foodToPoopIdList);
            }
        }

        private void Activate()
        {
            _stomachUIComponent.SetStomach(this);
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _inputManager = GameObject.FindObjectOfType<InputManager>();
            _poopList = Resources.LoadAll<PoopScriptable>("ScriptableObjects/Poops");
            _stomachUIComponent = GameObject.FindObjectOfType<StomachUIComponent>();
            StomachItemList = new List<StomachItemDTO>();
            _isPooping = false;
            _animator = this.GetComponent<Animator>();
            _movementMouseComponent = this.GetComponent<MovementMouseComponent>();
            _animatorVariables = new StomachAnimatorVariables();
            _playerStateManager = GameObject.FindObjectOfType<PlayerStateManager>();
        }

        protected override void ValidateValues()
        {
            if (_inputManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inputManager), this.gameObject.name));
            if (_stomachUIComponent == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_stomachUIComponent), this.gameObject.name));
            if (_animator == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_animator), this.gameObject.name));
            if (_animatorVariables == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_animatorVariables), this.gameObject.name));
            if (_playerStateManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_playerStateManager), this.gameObject.name));
        }
        #endregion
    }
}
