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

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
        }

        protected override void ValidateValues()
        {
            image = this.GetComponent<Image>();
        }
        #endregion
    }
}
