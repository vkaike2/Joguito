using Assets.Scripts.Components.GenericUI;
using Assets.Scripts.Components.InventorySlot;
using Assets.Scripts.Managers.UI;
using Assets.Scripts.ScriptableComponents.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Components.Draggable
{
    [RequireComponent(typeof(Image))]
    public class InventoryDraggableItemComponent : MonoBehaviour, IGenericUI
    {
        public EnumUIType Type => EnumUIType.Inventory_Item;

        private Vector2? _offset;

        private bool _isDragging;
        private Vector3 _initialPosition;
        private Image _image;
        public bool MouseInUI => _isDragging;

        public GameObject ThisGameObject => this.gameObject;

        private InventorySlotComponent _inventorySlot;

        private void Start()
        {
            _initialPosition = this.transform.position;
            _image = this.GetComponent<Image>();
            _image.enabled = false;
        }

        private void Update()
        {
            if (_isDragging)
            {
                transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - _offset.Value;
            }
        }

        public void StartDragging(InventorySlotComponent slot)
        {
            if (!_isDragging)
            {
                _inventorySlot = slot;
                transform.position = slot.gameObject.transform.position;
                _offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                _image.sprite = slot.CurrentImage.sprite;
                _image.enabled = true;
                _isDragging = true;
            }
        }

        public void StopDragging()
        {
            List<RaycastResult> raycastsUnderMouseList = this.RaycastMouse();
            InventorySlotComponent currentInventorySlot = raycastsUnderMouseList.Where(e => e.gameObject.GetComponent<InventorySlotComponent>() != null)
                .Select(e => e.gameObject.GetComponent<InventorySlotComponent>())
                .FirstOrDefault();

            if (currentInventorySlot != null && currentInventorySlot.GetInstanceID() != _inventorySlot.GetInstanceID())
            {
                if (!currentInventorySlot.HasItem)
                {
                    currentInventorySlot.AddItem(_inventorySlot.CurrentItem);
                    _inventorySlot.RemoveItem();
                }
                else
                {
                    ItemScriptable targetItem = currentInventorySlot.CurrentItem;
                    currentInventorySlot.RemoveItem();
                    currentInventorySlot.AddItem(_inventorySlot.CurrentItem);
                    _inventorySlot.RemoveItem();
                    _inventorySlot.AddItem(targetItem);
                }
            }
            else
            {
                _inventorySlot.CurrentImage.enabled = true;
            }

            _image.enabled = false;
            transform.position = _initialPosition;
            _isDragging = false;
        }


        public List<RaycastResult> RaycastMouse()
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
            };

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            return results;
        }
    }
}
