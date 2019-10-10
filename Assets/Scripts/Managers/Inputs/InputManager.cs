
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Managers.Inputs
{
    public class InputManager : MonoBehaviour
    {
        public float MouseLeftButton { get; private set; }
        public float Inventory { get; set; }

        void Update()
        {
            MouseLeftButton = Input.GetAxisRaw(EnumInputs.Mouse_Left_Button.ToString());
            Inventory = Input.GetAxisRaw(EnumInputs.Inventory.ToString());
        }
    }
}
