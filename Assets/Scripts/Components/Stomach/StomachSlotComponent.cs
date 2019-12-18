using Assets.Scripts.ScriptableComponents.Item;
using Assets.Scripts.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components.Stomach
{
    /// <summary>
    ///     Every slot of stomach has one of those
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class StomachSlotComponent : BaseComponent
    {
        #region PUBLIC ATRIBUTES
        public EnumStomachSlotState StomachSlotState { get; private set; }
        #endregion

        #region SERIALIZABLE FIELDS
        [Header("Required Fields")]
        [SerializeField]
        private Image _cdwDigestionBar;
        #endregion

        #region PRIVATE ATRIBUTES
        private Image _image;
        private ItemScriptable _currentItem;
        #endregion

        #region PUBLIC METHODS
        public void AddFood(ItemScriptable item)
        {
            _currentItem = item;
            _image.enabled = true;
            _cdwDigestionBar.enabled = true;
            _image.sprite = item.InventorySprite;
            StomachSlotState = EnumStomachSlotState.Digesting;
            StartCoroutine(StartDigestion(_currentItem.DigestionTime));
        }

        #endregion

        #region COROUTINES
        IEnumerator StartDigestion(float digestionTime)
        {
            float internalCdw = 0f;
            _cdwDigestionBar.color = Color.white;

            while (internalCdw <= digestionTime)
            {
                _cdwDigestionBar.fillAmount = internalCdw / digestionTime;
                internalCdw += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            StomachSlotState = EnumStomachSlotState.ReadyToPoop;
            _cdwDigestionBar.color = Color.grey;
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _image = this.GetComponent<Image>();
            StomachSlotState = EnumStomachSlotState.Empty;
        }

        protected override void ValidateValues()
        {
            if (_image == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_image), this.gameObject.name));
            if (_cdwDigestionBar == null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_cdwDigestionBar), this.gameObject.name));
        }
        #endregion
    }
}
