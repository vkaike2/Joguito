using Assets.Scripts.Components.DamageTaker;
using Assets.Scripts.Components.Map;
using UnityEngine;

namespace Assets.Scripts.Components.Tile
{
    public class TileMapComponnent : BaseComponent
    {
        #region PUBLIC ATTRIBUTES
        public EnumSide Side => _side;
        public bool HasObstacle { get; private set; }
        #endregion

        #region SERIALIZE ATTRIBUTES
        [Header("Required Methods")]
        [SerializeField]
        private EnumSide _side;
        #endregion

        #region PRIVATE ATTRIBUTES
        private SpriteRenderer _spriteRenderer;
        private MapAnimatorVariables _animatorVariables;
        private Animator _animator;
        private DamageTakerComponent _damageTaker;
        #endregion

        #region PUBLIC METHODS
        public void SpawnObject(GameObject pefab)
        {
            GameObject.Instantiate(pefab, this.transform.position, Quaternion.identity);
            HasObstacle = true;
        }

        public void SetInitialSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }
        #endregion

        #region UNITY METHODS
        private void FixedUpdate()
        {
            if (_animator == null || _damageTaker == null) return;
            //int count = _animator.layerCount;


            _animator.SetLayerWeight(1, 1);
            _animator.SetFloat(_animatorVariables.LifePercentage, _damageTaker.HealthPercenage);
        }
        #endregion

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            HasObstacle = false;
            _spriteRenderer = this.GetComponent<SpriteRenderer>();
            _animator = this.GetComponent<Animator>();
            _damageTaker = this.GetComponentInParent<DamageTakerComponent>();
            _animatorVariables = new MapAnimatorVariables();
        }

        protected override void ValidateValues()
        {
        }
        #endregion
    }

    public enum EnumSide
    {
        LEFT_UP,
        UP,
        RIGHT_UP,
        RIGHT,
        LEFT,
        LEFT_BOTTOM,
        BOTTOM,
        BOTTOM_RIGHT,
        CENTER
    }
}
