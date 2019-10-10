using Assets.Scripts.Managers.Inputs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Components.Draggable
{
    public class DraggableComponent : EventTrigger
    {

        private bool dragging;
        private Vector2? offset;

        public void Update()
        {
            if (dragging)
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

            dragging = true;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if(offset.HasValue)
            {
                offset = null;
            }

            dragging = false;
        }
    }
}
