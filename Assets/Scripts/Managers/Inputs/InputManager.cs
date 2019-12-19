
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Managers.Inputs
{
    /// <summary>
    ///     Capture every input in the game
    /// </summary>
    public class InputManager : BaseManager
    {
        #region PUBLIC ATRIBUTES
        public float MouseLeftButton { get; private set; }
        public float Inventory { get; private set; }
        public float ToggleItem { get; private set; }
        public float TroggleItemWhenPressed { get; private set; }
        public float PoopButton { get; private set; }


        public float SlotOne { get; private set; }
        public float SlotTwo { get; private set; }
        public float SlotThree { get; private set; }
        public float SlotFor { get; private set; }
        #endregion

        #region UNITY METHODS
        void Update() => CaptureInputs();
        #endregion

        #region PRIVATE METHODS
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

            this.PoopButton = Input.GetAxisRaw(EnumInputs.F.ToString());
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void ValidateValues() { }
        protected override void SetInitialValues() { }
        #endregion
    }
}
