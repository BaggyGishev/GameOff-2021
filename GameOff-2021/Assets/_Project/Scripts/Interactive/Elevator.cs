using UnityEngine;

namespace Gisha.GameOff_2021.Interactive
{
    public class Elevator : Controllable
    {
        private bool _isRising = false;

        private void Update()
        {
            if (!_isRising)
                return;

            transform.Translate(Vector2.up * 1f * Time.deltaTime);
        }

        public override void InteractAction()
        {
            _isRising = true;
        }
    }
}