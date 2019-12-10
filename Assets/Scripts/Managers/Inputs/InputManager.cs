
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Managers.Inputs
{
    [HelpURL("https://slimwiki.com/vkaike9/inputmanager")]
    public class InputManager : BaseManager
    {
        public float MouseLeftButton { get; private set; }
        public float Inventory { get; private set; }
        public float ToggleItem { get; private set; }
        public float TroggleItemWhenPressed { get; private set; }

        public float SlotOne { get; private set; }
        public float SlotTwo { get; private set; }
        public float SlotThree { get; private set; }
        public float SlotFor { get; private set; }

        void Update() => CaptureInputs();

        private void CaptureInputs()
        {
            this.MouseLeftButton = Input.GetAxisRaw(EnumInputs.Mouse_Left_Button.ToString());
            this.Inventory = Input.GetAxisRaw(EnumInputs.Inventory.ToString());
            this.ToggleItem = Input.GetAxisRaw(EnumInputs.Z.ToString());
            this.TroggleItemWhenPressed = Input.GetAxisRaw(EnumInputs.Alt.ToString());

            this.SlotOne = Input.GetAxisRaw(EnumInputs.Q.ToString());
            this.SlotTwo = Input.GetAxisRaw(EnumInputs.W.ToString());
            this.SlotThree = Input.GetAxisRaw(EnumInputs.E.ToString());
            this.SlotFor = Input.GetAxisRaw(EnumInputs.R.ToString());
        }

        protected override void ValidateValues() { }
        protected override void SetInitialValues() { }
    }
}
