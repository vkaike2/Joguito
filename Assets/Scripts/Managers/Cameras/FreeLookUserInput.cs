using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers.Cameras
{
    [RequireComponent(typeof(CinemachineFreeLook))]
    public class FreeLookUserInput : MonoBehaviour
    {
        private bool _freeLookActive;

        // Use this for initialization
        private void Start()
        {
            CinemachineCore.GetInputAxis = GetInputAxis;
        }

        private void Update()
        {
            _freeLookActive = Input.GetMouseButton(1); // 0 = left mouse btn or 1 = right
        }

        private float GetInputAxis(string axisName)
        {
            return !_freeLookActive ? 0 : Input.GetAxis(axisName == "Mouse_Left_Button" ? "Mouse_Left_Button" : "Mouse_Right_Button");
        }
    }
}
