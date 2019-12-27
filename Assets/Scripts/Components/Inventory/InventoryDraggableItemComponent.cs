using Assets.Scripts.Components.GenericUI;
using Assets.Scripts.Components.InventorySlot;
using Assets.Scripts.Components.ItemDrop;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts.DTOs;

namespace Assets.Scripts.Components.Draggable
{
    /// <summary>
    ///     Used to drag itens between the inventory and action slots
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class InventoryDraggableItemComponent : BaseComponent
    {
        #region PUBLIC ATRIBUTES
        public GameObject ThisGameObject => this.gameObject;
        #endregion

        #region SERIALIZABLE ATRIBUTES
#pragma warning disable 0649
        [Header("Required Fields")]
        [SerializeField]
        private GameObject _itemDropPrefab;

        [Header("Configuration")]
        [SerializeField]
        [Range(0, 1)]
        private float _offsetClickValue;
        [SerializeField]
        [Range(0, 1)]
        private float _offsetDragValue;
#pragma warning restore 0649
        #endregion


        #region PRIVATE ATRIBUTES
        private Vector2? _offset;
        private bool _isDragging;
        private Vector3 _initialPosition;
        private Image _image;
        private GenericUIComponent _gereciUIComponent;
        private InventorySlotComponent _dragInventorySlot;
        private ItemDTO _dragItem;
        #endregion

        #region PUBLIC METHODS
        public void StartDragging(InventorySlotComponent slot)
        {
            if (_isDragging) return;

            _dragInventorySlot = slot;
            _dragItem = _dragInventorySlot.CurrentItem;
            _dragInventorySlot.RemoveItem();
            transform.position = slot.gameObject.transform.position;
            _offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;


            if (_offset.Value.x >= _offsetClickValue) _offset = new Vector2(_offsetDragValue, _offset.Value.y);
            if (_offset.Value.x <= -_offsetClickValue) _offset = new Vector2(-_offsetDragValue, _offset.Value.y);
            if (_offset.Value.y >= _offsetClickValue) _offset = new Vector2(_offset.Value.x, _offsetDragValue);
            if (_offset.Value.y <= -_offsetClickValue) _offset = new Vector2(_offset.Value.x, -_offsetDragValue);

            _image.sprite = slot.CurrentImage.sprite;
            _image.enabled = true;
            _isDragging = true;
            _gereciUIComponent.SetMouseInUi(true);
        }

        public void StopDragging()
        {
            List<RaycastResult> raycastsUnderMouseList = this.RaycastMouse();

            InventorySlotComponent targetInventorySlot = raycastsUnderMouseList.Where(e => e.gameObject.GetComponent<InventorySlotComponent>() != null)
                .Select(e => e.gameObject.GetComponent<InventorySlotComponent>())
                .FirstOrDefault();

            if (targetInventorySlot != null && targetInventorySlot.GetInstanceID() != _dragInventorySlot.GetInstanceID())
            {
                if (targetInventorySlot.CheckIfCanAcceptItem(_dragItem))
                {
                    targetInventorySlot.SetItem(_dragItem);
                }
                else
                {
                    _dragInventorySlot.SetItem(_dragItem);
                }
            }
            else if (targetInventorySlot == null) // => Drop Item
            {
                GameObject itemDropGameObject = Instantiate(_itemDropPrefab, this.gameObject.transform.position, Quaternion.identity);
                itemDropGameObject.GetComponentInChildren<ItemDropComponent>().SetCurrentItem(_dragItem);
            }
            else // => Same slot
            {
                _dragInventorySlot.SetItem(_dragItem);
            }

            _image.enabled = false;
            transform.position = _initialPosition;
            _isDragging = false;
            _gereciUIComponent.SetMouseInUi(false);
        }
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            _initialPosition = this.transform.position;
            _image = this.GetComponent<Image>();
            _image.enabled = false;
        }

        private void Update()
        {
            if (!_isDragging) return;
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - _offset.Value;
        }
        #endregion

        #region PRIVATE ATRIBUTES
        private List<RaycastResult> RaycastMouse()
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
        #endregion

        #region ABSTRACT METHODS
        protected override void ValidateValues()
        {
            if (_itemDropPrefab == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_itemDropPrefab), this.gameObject.name));
        }

        protected override void SetInitialValues()
        {
            _gereciUIComponent = this.GetComponent<GenericUIComponent>();

            if (_offsetClickValue == 0f) _offsetClickValue = 0.35f;
            if (_offsetDragValue == 0f) _offsetDragValue = 0.30f;
        }
        #endregion
    }
}
