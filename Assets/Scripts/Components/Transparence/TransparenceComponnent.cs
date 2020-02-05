using Assets.Scripts.Structure.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class TransparenceComponnent : BaseComponent
    {

        #region SCRIPTABLE ATTRIBUTES
        [Header("RequiredFields")]
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        #endregion

        #region COLLIDER METHODS
        private void OnTriggerEnter2D(Collider2D collision)
        {
            SetTransparence(collision != null && collision.gameObject.GetComponent<PlayerStructure>() != null);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            SetTransparence(false);
        }
        #endregion

        #region PRIVATE METHODS
        private void SetTransparence(bool value)
        {
            Color color = _spriteRenderer.color;
            color.a = value ? .5f : 1f;
            _spriteRenderer.color = color;
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }
}
