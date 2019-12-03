using UnityEngine;

namespace Assets.Scripts
{
    public abstract class BaseObject : MonoBehaviour
    {
        protected abstract void ValidateValues();
        protected abstract void SetInitialValues();

        private void Awake()
        {
            SetInitialValues();
            ValidateValues();
        }
    }
}
