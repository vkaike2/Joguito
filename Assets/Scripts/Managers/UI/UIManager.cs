using Assets.Scripts.Components.Draggable;
using Assets.Scripts.Components.GenericUI;
using Assets.Scripts.Managers.Inputs;
using Assets.Scripts.Managers.Inventory;
using UnityEngine;

namespace Assets.Scripts.Managers.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Required Fields")]
        [SerializeField]
        private InputManager _inputManager;
        [SerializeField]
        private InventoryManager _inventory;

        public IGenericUI[] GenericUIList { get; set; }

        private bool _keyPressedInventory = false;

        private void Awake()
        {
            _inventory.gameObject.SetActive(false);
        }

        private void Start()
        {
        }

        private void Update()
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

    }
}
