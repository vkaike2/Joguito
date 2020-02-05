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

        #region ABSTRACT METHODS
        protected override void SetInitialValues()
        {
            HasObstacle = false;
            _spriteRenderer = this.GetComponent<SpriteRenderer>();
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
