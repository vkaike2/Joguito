using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components.ActivePlayers
{
    public class ActivePlayerSlotComponent : BaseComponent
    {
        #region PUBLIC ATRIBUTES
        public bool IsActive { get; private set; }
        #endregion

        #region SERIALIZABLE FIELDS
        [Header("Required Fields")]
        [SerializeField]
        private Transform _nextSpanwPoints;
        [SerializeField]
        private GameObject _ActivePlayerSlotPrefab;
        #endregion

        #region PRIVATE ATRIBUTES
        private Image image;
        #endregion

        public void ActivateSlot(bool value)
        {
            IsActive = false;

            if (value)
                image.color = Color.red;
            else
                image.color = Color.white;
        }

        public ActivePlayerSlotComponent SpanwNewPlayerSlot()
        {
            GameObject playerSlotGameObject = GameObject.Instantiate(_ActivePlayerSlotPrefab, _nextSpanwPoints.position, Quaternion.identity);
            return playerSlotGameObject.GetComponent<ActivePlayerSlotComponent>();       
        }

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
        }

        protected override void ValidateValues()
        {
            if (_nextSpanwPoints is null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_nextSpanwPoints), this.gameObject.name));
            if (_ActivePlayerSlotPrefab is null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_ActivePlayerSlotPrefab), this.gameObject.name));

            image = this.GetComponent<Image>();
        }
        #endregion
    }
}
