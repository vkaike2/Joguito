using Assets.Scripts.Managers.Inputs;
using UnityEngine;

namespace Assets.Scripts.Managers.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Required Fields")]
        [SerializeField]
        private InputManager _inputManager;
        [SerializeField]
        private GameObject _inventory;

        private bool _keyPressedInventory = false;

        private void Awake()
        {
            _inventory.SetActive(false);
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

                _inventory.SetActive(!_inventory.activeSelf);
            }

            if(_keyPressedInventory && _inputManager.Inventory == 0)
            {
                _keyPressedInventory = false;
            }
        }

    }
}
