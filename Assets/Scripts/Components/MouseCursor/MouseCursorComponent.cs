using Assets.Scripts.Managers.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Component.MouseCursor
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class MouseCursorComponent : MonoBehaviour
    {
        [Header("Required Fields")]
        [SerializeField]
        private UIManager _uiManager;

        public bool CantMovePlayer { get; private set; }
        public Vector2 CurrentPosition => this.transform.position;

        void Awake()
        {
            Cursor.visible = false;
        }

        void Update()
        {
            this.CheckIfIsCanMovePlayer();
            this.ChangeMouseCursor();
        }

        private void CheckIfIsCanMovePlayer()
        {
            CantMovePlayer = _uiManager.DraggableComponentList.Any(e => e.IsDragging);
            CantMovePlayer = _uiManager.GenericUIList.Any(e => e.MouseInUI);
        }

        private void ChangeMouseCursor()
        {
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.transform.position = cursorPosition;
        }
    }

}
