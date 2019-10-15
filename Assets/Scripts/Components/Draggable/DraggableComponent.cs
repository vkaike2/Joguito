using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Components.Draggable
{
    public class DraggableComponent : EventTrigger
    {
        public bool IsDragging { get; private set; }
        private Vector2? offset;

        public void Update()
        {
            if (IsDragging)
            {
                transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset.Value;
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if(!offset.HasValue)
            {
                offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            }

            IsDragging = true;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if(offset.HasValue)
            {
                offset = null;
            }

            IsDragging = false;
        }
    }
}
