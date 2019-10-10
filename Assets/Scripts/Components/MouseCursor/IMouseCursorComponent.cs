using UnityEngine;

namespace Assets.Scripts.Components.MouseCursor
{
    public interface IMouseCursorComponent
    {
        Vector2 CurrentPosition { get; }
    }
}
