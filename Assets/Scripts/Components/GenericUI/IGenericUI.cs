using UnityEngine;

namespace Assets.Scripts.Components.GenericUI
{
    public interface IGenericUI
    {
        bool MouseInUI { get; }
        EnumUIType Type { get; }

        GameObject ThisGameObject { get; } 
    }
}
