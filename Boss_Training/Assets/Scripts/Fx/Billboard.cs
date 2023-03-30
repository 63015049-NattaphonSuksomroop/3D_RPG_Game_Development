
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fx
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer _sprite;

        private bool _isShown = true;
        Transform _lookAtTarget; 

        public void LookAt(Transform target)
        {
            _lookAtTarget = target;
        }

        public void Show()
        {
            _isShown = true;
            _sprite.enabled = true;
        }

        public void Hide()
        {
            _isShown = false;
            _sprite.enabled = false;
        }

        void LateUpdate()
        {
            if (_isShown && _lookAtTarget != null)
            {
                transform.LookAt(_lookAtTarget);
            }
        }
    }
}
