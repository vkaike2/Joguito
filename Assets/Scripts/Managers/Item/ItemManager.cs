using Assets.Scripts.Components.ToggleableItem;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers.Item
{
    public class ItemManager : BaseManager
    {
#pragma warning disable 0649
        [Header("Requeired Fields")]
        [SerializeField]
        private InputManager _inputManager;
        [SerializeField]
        private List<ToggleableItemComponent> _togleableList;
#pragma warning restore 0649

        public bool ItemNameIsVisible { get; private set; }
        private bool _keyToggleItempressed = false;

        private void Update()
        {
            ToggleItemVisibility();
        }

        private void ToggleItemVisibility()
        {
            if (_inputManager.ToggleItem == 1 && !_keyToggleItempressed)
            {
                _keyToggleItempressed = true;

                foreach (var item in _togleableList)
                {
                    item.SetActive(!ItemNameIsVisible);
                }

                ItemNameIsVisible = !ItemNameIsVisible;
            }
            else if (_inputManager.ToggleItem == 0 && _keyToggleItempressed)
            {
                _keyToggleItempressed = false;
            }


            if (!ItemNameIsVisible)
            {
                foreach (var item in _togleableList)
                {
                    item.SetActive(_inputManager.TroggleItemWhenPressed == 1);
                }
            }
        }

        public void SetToggleabeItem(ToggleableItemComponent toggleableItem)
        {
            _togleableList.Add(toggleableItem);
        }

        public void RemoveToggleableItem(ToggleableItemComponent toggleableItem)
        {
            _togleableList.Remove(toggleableItem);
        }

        protected override void SetInitialValues()
        {
            ItemNameIsVisible = true;
        }

        protected override void ValidateValues()
        {
            if (_inputManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inputManager), this.gameObject.name));
        }
    }
}
