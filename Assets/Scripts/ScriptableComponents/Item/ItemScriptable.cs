using UnityEngine;

namespace Assets.Scripts.ScriptableComponents.Item
{
    [HelpURL("https://slimwiki.com/vkaike9/itemscriptable")]
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
    public class ItemScriptable : ScriptableBase
    {
#pragma warning disable 0649
        [Header("RequiredFields")]
        [SerializeField]
        private string _name;
        [TextArea]
        [SerializeField]
        private string _description;
        [SerializeField]
        private bool _stackable;
        [SerializeField]
        private EnumItemScriptableType _itemType;

        [Header("Inventory Configurations")]
        [SerializeField]
        private Sprite _inventorySprite;

        [Header("Floor Configurations")]
        [SerializeField]
        private RuntimeAnimatorController _dropAnimatorController;

        [Header("Planting Configurations")]
        [SerializeField]
        private RuntimeAnimatorController _plantingAnimatorController;
        [SerializeField]
        private float _secondsToBeReady;

        [Header("If Stackable")]
        [SerializeField]
        private int _maxStackableAmout;
#pragma warning restore 0649

        public string Name => _name;
        public string Description => _description;
        public Sprite InventorySprite => _inventorySprite;
        public bool Stackable => _stackable;
        public RuntimeAnimatorController DropAnimatorController => _dropAnimatorController;
        public RuntimeAnimatorController PlantingAnimatorController => _plantingAnimatorController;
        public EnumItemScriptableType ItemType => _itemType;
        public float SecondsToBeReadry => _secondsToBeReady;

        // => If Stackable
        public int MaxStackableAmout => _maxStackableAmout;

        protected override void ValidateValues()
        {
            if (string.IsNullOrEmpty(_name)) Debug.LogError($"The value of {nameof(Name)} cannot be null in some iten");
            if (string.IsNullOrEmpty(_description)) Debug.LogError($"The value of {nameof(Description)} cannot be null in some iten");
            if (_inventorySprite == null) Debug.LogError($"The value of {nameof(InventorySprite)} cannot be null in some iten");
            if (_dropAnimatorController == null) Debug.LogError($"The value of {nameof(_dropAnimatorController)} cannot be null in some iten");
            if (_plantingAnimatorController == null) Debug.LogError($"The value of {nameof(_plantingAnimatorController)} cannot be null in some iten");
            if (_secondsToBeReady == 0) Debug.LogError($"The value of {nameof(_secondsToBeReady)} cannot be 0 in some iten");

            if (_stackable)
                if (_maxStackableAmout == 0) Debug.LogError($"The value of {nameof(MaxStackableAmout)} cannot be 0 in some iten");
        }
    }

  
}
