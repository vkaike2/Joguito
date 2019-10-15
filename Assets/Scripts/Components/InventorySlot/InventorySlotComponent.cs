using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components.InventorySlot
{
    public class InventorySlotComponent : MonoBehaviour
    {
        [Header("Required Fields")]
        [SerializeField]
        private Vector2 _position;
        [SerializeField]
        private Image _currentImage;

        public bool HasItem => _currentImage != null && _currentImage.enabled;
        public int Amout { get; set; }

        public Vector2? AddItem(Sprite itemSprite, int amout)
        {
            if (HasItem) return null;

            _currentImage.sprite = itemSprite;
            Amout = amout;
            _currentImage.enabled = true;

            return _position;
        }

        public void RemoveItem()
        {
            if (!HasItem) return;
            _currentImage.enabled = false;
        }

        
    }
}
