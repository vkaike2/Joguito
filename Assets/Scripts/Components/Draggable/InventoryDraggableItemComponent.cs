using Assets.Scripts.Components.InventorySlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components.Draggable
{
    [RequireComponent(typeof(Image))]
    public class InventoryDraggableItemComponent : MonoBehaviour, IDraggable
    {
        public bool IsDragging => _isDragging;
        private Vector2? _offset;

        private bool _isDragging;
        private Vector3 _initialPosition;
        private Image _image;

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
            _image.enabled = false;
            if (_inventorySlot != null)
            {
                _inventorySlot.CurrentImage.enabled = true;
            }

            transform.position = _initialPosition;
            _isDragging = false;
        }

    }
}
