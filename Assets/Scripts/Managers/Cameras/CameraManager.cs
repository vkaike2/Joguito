using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers.Cameras
{
    public class CameraManager : BaseManager
    {

        #region PRIVATE ATTRIBUTES
        private CinemachineVirtualCamera _cinemachine;
        private Vector2? _initialPosition;
        private Vector2? _cameraInitialPosition;
        #endregion

        #region PUBLIC METHODS
        public void MoveCamera(bool value)
        {
            if (!value)
            {
                _initialPosition = null;
                return;
            }
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (_initialPosition is null)
            {
                _initialPosition = mousePosition;
                _cameraInitialPosition = _cinemachine.transform.position;
            }

            Vector2 offsetPosition = new Vector2(mousePosition.x - _initialPosition.GetValueOrDefault().x, mousePosition.y - _initialPosition.GetValueOrDefault().y);
            Vector2 offsetCameraPosition = new Vector2(_cinemachine.transform.position.x - _cameraInitialPosition.GetValueOrDefault().x, _cinemachine.transform.position.y - _cameraInitialPosition.GetValueOrDefault().y);
            offsetPosition -= offsetCameraPosition;

            if (offsetPosition == Vector2.zero || Math.Round(offsetPosition.x * 100f) == Math.Round(offsetCameraPosition.x * 100f)) return;
            
            //Debug.Log("MoveCamera - " + offsetPosition + " - " + offsetCameraPosition);
            Debug.Log("MoveCamera - " + offsetPosition.x.ToString("0.00") + " - " + offsetCameraPosition.x.ToString("0.00"));

            _cinemachine.Follow = null;

            _cinemachine.transform.position += (Vector3)new Vector2(offsetPosition.x > 0 ? 1 : -1, /*offsetPosition.y > 0 ? 1 : -1*/ 0) * 0.1f;




        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _cinemachine = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}
