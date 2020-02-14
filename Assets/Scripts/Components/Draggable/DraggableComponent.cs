using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Components.Draggable
{
    /// <summary>
    ///     Turns a UI object into a draggable UI
    /// </summary>
    public class DraggableComponent : EventTrigger
    {
        #region PRIVATE ATRIBUTES
        private Vector2? _offset;
        private bool _isDragging = false;
        #endregion

        #region UNTIY METHODS
        private void Update()
        {
            DragObject();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            GetTheOffsetValue();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            ResetOffsetValue();
        }
        #endregion

        #region PRIVATE METHODS
        private void DragObject()
        {
            if (_isDragging)
            {
                transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - _offset.Value;
            }
        }

        private void GetTheOffsetValue()
        {
            if (!_offset.HasValue)
            {
                _offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            }

            _isDragging = true;
        }

        private void ResetOffsetValue()
        {
            if (_offset.HasValue)
            {
                _offset = null;
            }

            _isDragging = false;
        }
        #endregion
    }
}
