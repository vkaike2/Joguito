using Assets.Scripts.Events;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Components.LifeBar
{
    public class LifeBarComponent : BaseComponent
    {
        #region PUBLIC ATRIBUTES
        public UEventFloat event_UpdateLifeBar;

        public bool IsBoss => _isBoss;
        #endregion

        #region SERIALIZABLE ATRIBUTES
        [Header("RequiredFields")]
        [SerializeField]
        private Image _fillContent;

        [Header("Configuration Fields")]
        [SerializeField]
        private float _fadeOutCooldown;
        [SerializeField]
        private bool _isBoss;
        #endregion

        #region PRIVATE ATRIBUTES
        private List<GameObject> _childrensGameObjects;
        private Coroutine _corotineShowForSomeTime;
        #endregion

        #region PUBLIC METHODS
       
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
        private void Event_UpdateLifeBar(float percentage)
        {
            _fillContent.fillAmount = percentage;
            if (_corotineShowForSomeTime != null)
            {
                StopCoroutine(_corotineShowForSomeTime);
            }
            _corotineShowForSomeTime = StartCoroutine(ShowForSomeTime());
        }

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

            //TODO CONTINUE FROM HERE
            event_UpdateLifeBar.AddListener(Event_UpdateLifeBar);
        }

        protected override void ValidateValues()
        {
            if(_fillContent is null) Debug.LogError(ValidatorUtils.ValidateNullAtGameObject(nameof(_fillContent), this.gameObject.name));
        }
        #endregion
    }
}
