
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Managers.Inputs
{
    public class InputManager : BaseManager
    {
        #region Public Fields
        public float MouseLeftButton { get; private set; }
        public float Inventory { get; private set; }
        #endregion

        void Update() => CaptureInputs();

        private void CaptureInputs()
        {
            this.MouseLeftButton = Input.GetAxisRaw(EnumInputs.Mouse_Left_Button.ToString());
            this.Inventory = Input.GetAxisRaw(EnumInputs.Inventory.ToString());
        }

        protected override void ValidateValues() { }
        protected override void SetInitialValues() { }
    }
}
