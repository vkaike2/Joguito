
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Managers.Inputs
{
    public class InputManager : BaseManager
    {
        public float MouseLeftButton { get; private set; }
        public float Inventory { get; private set; }
        public float ToggleItem { get; private set; }
        public float TroggleItemWhenPressed { get; set; }

        void Update() => CaptureInputs();

        private void CaptureInputs()
        {
            this.MouseLeftButton = Input.GetAxisRaw(EnumInputs.Mouse_Left_Button.ToString());
            this.Inventory = Input.GetAxisRaw(EnumInputs.Inventory.ToString());
            this.ToggleItem = Input.GetAxisRaw(EnumInputs.Z.ToString());
            this.TroggleItemWhenPressed = Input.GetAxisRaw(EnumInputs.Alt.ToString());
        }

        protected override void ValidateValues() { }
        protected override void SetInitialValues() { }
    }
}
