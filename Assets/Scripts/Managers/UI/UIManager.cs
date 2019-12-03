using Assets.Scripts.Components.GenericUI;
using Assets.Scripts.Components.Inventory;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Managers.UI
{
    public class UIManager : BaseManager
    {
#pragma warning disable 0649
        [Header("Required Fields")]
        [SerializeField]
        private InputManager _inputManager;
        [SerializeField]
        private InventoryComponent _inventory;
#pragma warning restore 0649

        public IGenericUI[] GenericUIList { get; private set; }

        private bool _keyPressedInventory = false;

        private void Start()
        {
            this.GenericUIList = _inventory.GetComponentsInChildren<IGenericUI>();
        }

        private void FixedUpdate()
        {
            this.InventoryController();
        }

        private void InventoryController()
        {
            if(!_keyPressedInventory && _inputManager.Inventory == 1)
            {
                _keyPressedInventory = true;

                _inventory.gameObject.SetActive(!_inventory.gameObject.activeSelf);
            }

            if(_keyPressedInventory && _inputManager.Inventory == 0)
            {
                _keyPressedInventory = false;
            }
        }

        protected override void ValidateValues()
        {
            if (_inputManager == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inventory), this.gameObject.name));
            if (_inventory == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_inventory), this.gameObject.name));
        }

        protected override void SetInitialValues()
        {
            _inventory.gameObject.SetActive(false);
        }
    }
}
