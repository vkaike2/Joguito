using Assets.Scripts.Components.MouseCursor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Component.MouseCursor
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class MouseCursorComponent : MonoBehaviour, IMouseCursorComponent
    {

        private SpriteRenderer _mouseCursorSprite;

        public Vector2 CurrentPosition => this.transform.position;

        void Awake()
        {
            Cursor.visible = false;
        }

        private void Start()
        {
            _mouseCursorSprite = this.GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.transform.position = cursorPosition;
        }
    }

}
