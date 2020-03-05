using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.Inventory
{
    public class InventoryItemInformationComponent : BaseComponent
    {
        #region SERIALIZE ATRIBUTES
        [Header("ITEM INFORMATION")]
        [SerializeField]
        private GameObject _gameObjectItemInformation;
        [SerializeField]
        private TextMeshProUGUI _txtItemName;
        [SerializeField]
        private TextMeshProUGUI _txtDescription;
        #endregion

        #region PRIVATE ATRIBUTES
        private Coroutine _mouseOverCoroutine;
        #endregion

        #region PUBLIC METHODS
        public void StartShowInformation(string name, string description)
        {
            if (!this.gameObject.activeSelf) return;

            if (_mouseOverCoroutine != null)
                StopCoroutine(_mouseOverCoroutine);

            _mouseOverCoroutine = StartCoroutine(HoverForSomeSeconds(.1f, name, description));
        }
        #endregion

        #region COROUTINE
        IEnumerator HoverForSomeSeconds(float _cdwOn, string name, string description)
        {
            this.ShowInformation(true, name, description);
            float _internalCdw = 0f;
            while (_internalCdw <= _cdwOn)
            {
                _internalCdw += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            this.ShowInformation(false, name, description);
        }
        #endregion


        #region PRIVATE METHODS
        private void ShowInformation(bool show, string name, string description)
        {
            _gameObjectItemInformation.SetActive(show);
            _txtItemName.SetText(name);
            _txtDescription.SetText(description);
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            _gameObjectItemInformation.SetActive(false);
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}
