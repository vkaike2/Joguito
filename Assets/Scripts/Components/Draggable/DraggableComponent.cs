using Assets.Scripts.Components.GenericUI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Components.Draggable
{
    [HelpURL("https://slimwiki.com/vkaike9/draggablecomponent")]
    public class DraggableComponent : EventTrigger
    {
        private Vector2? _offset;
        private bool _isDragging = false;
        
        public void Update()
        {
            if (_isDragging)
            {
                transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - _offset.Value;
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if(!_offset.HasValue)
            {
                _offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            }

            _isDragging = true;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if(_offset.HasValue)
            {
                _offset = null;
            }

            _isDragging = false;
        }
    }
}
