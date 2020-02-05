using Assets.Scripts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Map
{
    public class MapSpawnComponent : BaseComponent
    {
        #region PUBLIC ATTRIBUTES
        public Transform Transform => this.transform;
        public bool HasMap { get; private set; }
        public EnumMapPosition Position => _position;
        #endregion

        #region SERIALIZABLE ATTRIBUTES
        [SerializeField]
        private EnumMapPosition _position;
        #endregion

        #region PRIVATE ATTRIBUTES
        private BoxCollider2D _boxCollider;
        #endregion

        #region COLLIDER METHODS
        private void OnTriggerEnter2D(Collider2D collision)
        {
            this.CheckIfHasMap(collision, true);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            this.CheckIfHasMap(collision, false);
        }
        #endregion

        #region PRIVATE METHODS
        private void CheckIfHasMap(Collider2D collision, bool OntriggerEnter)
        {
            if (collision.gameObject.GetComponent<MapComponent>() != null)
            {
                HasMap = OntriggerEnter;
                _boxCollider.enabled = !OntriggerEnter;
            }
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            HasMap = false;
            _boxCollider = this.GetComponentInChildren<BoxCollider2D>();
        }

        protected override void ValidateValues()
        {
        }
        #endregion ABSTRACT METHODS
    }
}
