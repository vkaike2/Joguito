using UnityEngine;

namespace Assets.Scripts.ScriptableComponents.Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
    public class ItemScriptable : ScriptableBase
    {
#pragma warning disable 0649
        #region Required Fields
        [Header("RequiredFields")]
        [SerializeField]
        private string _name;
        [TextArea]
        [SerializeField]
        private string _description;
        [SerializeField]
        private bool _stackable;
        [SerializeField]
        private Sprite _inventorySprite;
        [SerializeField]
        private RuntimeAnimatorController _dropAnimatorController;
        #endregion

        #region Configuration Fields
        [Header("If Stackable")]
        [SerializeField]
        private int _maxStackableAmout;
        #endregion
#pragma warning restore 0649

        public string Name => _name;
        public string Description => _description;
        public Sprite InventorySprite => _inventorySprite;
        public bool Stackable => _stackable;
        public RuntimeAnimatorController DropAnimatorController => _dropAnimatorController;

        // => If Stackable
        public int MaxStackableAmout => _maxStackableAmout;

        protected override void ValidateValues()
        {
            if (string.IsNullOrEmpty(_name)) Debug.LogError($"The value of {nameof(Name)} cannot be null in some iten");
            if (string.IsNullOrEmpty(_description)) Debug.LogError($"The value of {nameof(Description)} cannot be null in some iten");
            if (_inventorySprite == null) Debug.LogError($"The value of {nameof(InventorySprite)} cannot be null in some iten");

            if (_stackable)
                if (_maxStackableAmout == 0) Debug.LogError($"The value of {nameof(MaxStackableAmout)} cannot be 0 in some iten");
        }
    }

    public class ItemDTO
    {
        public ItemScriptable Item { get; set; }
        public int Amount { get; set; }
    }
}
