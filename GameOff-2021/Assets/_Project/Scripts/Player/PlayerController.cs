using Gisha.Effects.Audio;
using Gisha.GameOff_2021.Core;
using Gisha.GameOff_2021.GUI;
using UnityEngine;

namespace Gisha.GameOff_2021.Player
{
    public class PlayerController : MonoBehaviour, IDestroyable
    {
        private PlayerBehaviour[] _behaviours;

        private PlayerAnimationsHandler _animationsHandler;

        private int _lives = 5;
        private bool _onceDie = false;

        private void Awake()
        {
            // Adding behaviours.
            _animationsHandler = GetComponent<PlayerAnimationsHandler>();
            _behaviours = new PlayerBehaviour[2];
            _behaviours[0] = GetComponent<MovementBehaviour>();
            _behaviours[1] = GetComponent<ControlBehaviour>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                SwitchBehaviour();
        }

        private void SwitchBehaviour()
        {
            PlayerBehaviour temp = _behaviours[0];
            temp.ResetOnBehaviourChange();

            // Enabling right one behaviour.
            _behaviours[0].enabled = false;
            _behaviours[1].enabled = true;

            // Switching behaviours.
            _behaviours[0] = _behaviours[1];
            _behaviours[1] = temp;

            Debug.Log("Behaviour was switched!");
            
            AudioManager.Instance.PlaySFX("powerUp");
        }

        public void Die()
        {
            if (!_onceDie)
            {
                AudioManager.Instance.PlaySFX("Death3");
                _animationsHandler.InitDieAnimation();
                _onceDie = true;
            }
        }

        private void DieAfterAnimation()
        {
            _lives--;
            HealthGUI.ChangeHPCount(_lives);
            if (_lives > 0)
            {
                GameManager.RespawnOnLevel(this);
            }
            else
                GameManager.RestartLocation();

            _onceDie = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Obstacle"))
                Die();
        }

        public void Destroy()
        {
            Die();
        }
    }
}