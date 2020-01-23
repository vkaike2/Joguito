using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components.LifeBar
{
    public class LifeBarComponent : BaseComponent
    {
        #region SERIALIZABLE ATRIBUTES
        [Header("RequiredFields")]
        [SerializeField]
        private Image _fillContent;

        [Header("Configuration Fields")]
        [SerializeField]
        private float _fadeOutCooldown;
        #endregion

        #region PRIVATE ATRIBUTES
        private bool _receiveNewUpdate = false;
        private List<GameObject> _childrensGameObjects;
        private Coroutine _corotineShowForSomeTime;
        #endregion

        #region PUBLIC METHODS
        public void PercentageHp(float percentage)
        {
            _receiveNewUpdate = true;
            _fillContent.fillAmount = percentage;
            if(_corotineShowForSomeTime != null)
            {
                StopCoroutine(_corotineShowForSomeTime);
            }
            _corotineShowForSomeTime = StartCoroutine(ShowForSomeTime());
        }
        #endregion

        #region UNITY METHODS
        private void Start()
        {
            GetChildrenGameObjects();
            FadeOut();
        }
        #endregion

        #region COROUTINES
        IEnumerator ShowForSomeTime()
        {
            float _internalCdw = 0f;
            FadeIn();
            while (_internalCdw <= _fadeOutCooldown)
            {
                _internalCdw += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            FadeOut();
        }
        #endregion

        #region PRIVATE METHODS
        private void GetChildrenGameObjects()
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                _childrensGameObjects.Add(this.transform.GetChild(i).gameObject);
            }
        }
        private void FadeOut()
        {
            foreach (var children in _childrensGameObjects)
            {
                children.SetActive(false);
            }
        }

        private void FadeIn()
        {
            foreach (var children in _childrensGameObjects)
            {
                children.SetActive(true);
            }
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            if(_fadeOutCooldown == 0) _fadeOutCooldown = 2f;
            _childrensGameObjects = new List<GameObject>();
        }

        protected override void ValidateValues()
        {
            if(_fillContent is null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_fillContent), this.gameObject.name));
        }
        #endregion
    }
}
