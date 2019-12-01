using UnityEngine;

namespace Assets.Scripts.ScriptableComponents.Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
    public class ItemScriptable : ScriptableBase
    {
        [Header("RequiredFields")]
        [SerializeField]
        private string _name;
        [TextArea]
        [SerializeField]
        private string _description;
        [SerializeField]
        private bool _stackable;
        [SerializeField]
        private Sprite _sprite;

        [Header("If Stackable")]
        [SerializeField]
        private int _maxStackableAmout;

        public string Name => _name;
        public string Description => _description;
        public Sprite InventorySprite => _sprite;
        public bool Stackable => _stackable;

        // => If Stackable
        public int MaxStackableAmout => _maxStackableAmout;



        protected override void ValidateValues()
        {
            throw new System.NotImplementedException();
        }
    }
}
