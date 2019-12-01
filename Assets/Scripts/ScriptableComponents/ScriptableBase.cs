using UnityEngine;

namespace Assets.Scripts.ScriptableComponents
{
    public abstract class ScriptableBase : ScriptableObject
    {
        protected abstract void ValidateValues();

        private void OnValidate()
        {
            this.ValidateValues();
        }
    }
}
