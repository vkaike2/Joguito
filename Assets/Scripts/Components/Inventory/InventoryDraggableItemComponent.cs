using Assets.Scripts.Components.InventorySlot;
using Assets.Scripts.Components.ItemDrop;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts.DTOs;
using Assets.Scripts.Components.Interactable;
using Assets.Scripts.Managers.PlayerState;
using Assets.Scripts.Component.MouseCursor;

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
        public bool IsDragging { get; private set; }
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
        private Vector3 _initialPosition;
        private Image _image;
        private InventorySlotComponent _dragInventorySlot;
        private ItemDTO _dragItem;
        private PlayerStateManager _playerStateManager;
        private MouseCursorComponent _mouseCursorComponent;
        #endregion

        #region PUBLIC METHODS
        public void StartDragging(InventorySlotComponent slot)
        {
            if (IsDragging) return;
            _mouseCursorComponent.HasItemUnderTheCursor = true;

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
            IsDragging = true;
        }

        public void StopDragging()
        {
            (List<RaycastHit2D>, List<RaycastResult>) underTheMouse = _mouseCursorComponent.GetEveryObjectUnderTheMouse();

            InventorySlotComponent targetInventorySlot = underTheMouse.Item2.Where(e => e.gameObject.GetComponent<InventorySlotComponent>() != null)
                .Select(e => e.gameObject.GetComponent<InventorySlotComponent>())
                .FirstOrDefault();

            InteractableComponent playerInteractableComponent = GetCurrentInteractableComponentUnderTheMouseThatsCanEat(underTheMouse);

            // => CHANGE ITEM SLOT
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
            // => FEED THE PLAYER
            else if (playerInteractableComponent != null && _dragItem.Item.ItemType == ScriptableComponents.Item.EnumItemScriptableType.Flower)
            {
                _dragItem.Amount--;

                playerInteractableComponent.EatFlowerByDrag(new ItemDTO() { Item = _dragItem.Item, Amount = 1 });

                if (_dragItem.Amount > 0)
                {
                    _dragInventorySlot.SetItem(_dragItem);
                }
            }
            // => DROP ITEM
            else if (targetInventorySlot == null)
            {
                this.DropItem();
            }
            // => PUT ITEM ON THE SAME SPOT
            else
            {
                _dragInventorySlot.SetItem(_dragItem);
            }

            _image.enabled = false;
            transform.position = _initialPosition;
            _dragInventorySlot = null;
            IsDragging = false;
            _mouseCursorComponent.HasItemUnderTheCursor = false;
        }

        private InteractableComponent GetCurrentInteractableComponentUnderTheMouseThatsCanEat((List<RaycastHit2D>, List<RaycastResult>) underTheMouse)
        {
            List<int> interactableComponentInstanceIdList = underTheMouse.Item1.Where(e => e.collider.GetComponent<InteractableComponent>() != null)
                            .Select(e => e.collider.GetComponent<InteractableComponent>().GetInstanceID())
                            .ToList();
            List<InteractableComponent> playerInteractableComponent = _playerStateManager.GetAllInteractableComponents();
            InteractableComponent selectedInteractableComponentToEat = playerInteractableComponent
                .FirstOrDefault(e => interactableComponentInstanceIdList.Contains(e.GetInstanceID()) && e.CanEat);

            return selectedInteractableComponentToEat;
        }

        private void DropItem()
        {
            GameObject itemDropGameObject = Instantiate(_itemDropPrefab, this.gameObject.transform.position, Quaternion.identity);
            itemDropGameObject.GetComponentInChildren<ItemDropComponent>().SetCurrentItem(_dragItem);
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
            if (!IsDragging) return;
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - _offset.Value;
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void ValidateValues()
        {
            if (_itemDropPrefab == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_itemDropPrefab), this.gameObject.name));
        }

        protected override void SetInitialValues()
        {
            _playerStateManager = GameObject.FindObjectOfType<PlayerStateManager>();
            _mouseCursorComponent = GameObject.FindObjectOfType<MouseCursorComponent>();

            if (_offsetClickValue == 0f) _offsetClickValue = 0.35f;
            if (_offsetDragValue == 0f) _offsetDragValue = 0.30f;
        }
        #endregion
    }
}
