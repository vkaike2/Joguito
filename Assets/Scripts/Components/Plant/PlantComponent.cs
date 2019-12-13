using Assets.Scripts.Components.PlantSpot;
using Assets.Scripts.DTOs;
using Assets.Scripts.ScriptableComponents.Item;
using Assets.Scripts.Utils;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components.Plant
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlantComponent : BaseComponent
    {
        [Header("Required Fields")]
        [SerializeField]
        private PlantSpotComponent _plantSpotComponent;

        private Animator _animator;
        private PlantAnimatorVariables _animatorVariables;
        private ItemDTO _currentPlant;
        public ItemScriptable CurrentScriptableSeedType => _currentPlant.Item;

        public bool CanAcceptNewPlant => _currentPlant == null;

        public void SetPlant(ItemDTO plant)
        {
            if (_currentPlant != null) Debug.LogError("You cannot set a plant if the PlantComponent already have a flower!");

            _currentPlant = plant;
            _animator.runtimeAnimatorController = plant.Item.PlantingAnimatorController;
            StartCoroutine(StartPlantingProcces(plant.Item.SecondsToBeReadry));
        }

        public void RemovePlant()
        {
            if (_currentPlant == null) Debug.LogError("You cannot remove a plant if the PlantComponent doesn't have any flower!");

            _currentPlant = null;
            _animator.runtimeAnimatorController = null;
        }

        IEnumerator StartPlantingProcces(float plantingTimeProccess)
        {
            yield return new WaitForSeconds(plantingTimeProccess / 2);
            _animator.SetTrigger(_animatorVariables.MiddleState);
            yield return new WaitForSeconds(plantingTimeProccess / 2);
            _animator.SetTrigger(_animatorVariables.FinalState);
            _plantSpotComponent.SetState(EnumPlantSpotState.Ready, this.transform.parent.gameObject.GetInstanceID());
        }


        protected override void SetInitialValues()
        {
            _animator = this.GetComponent<Animator>();
            _animatorVariables = new PlantAnimatorVariables();
        }

        protected override void ValidateValues()
        {
            if (_animator == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_animator), this.gameObject.name));
            if (_plantSpotComponent == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_plantSpotComponent), this.gameObject.name));
        }
    }
}
