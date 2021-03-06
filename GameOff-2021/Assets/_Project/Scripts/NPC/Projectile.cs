using System.Linq;
using Gisha.Effects.Audio;
using Gisha.GameOff_2021.Core;
using UnityEngine;
using VFXManager = Gisha.Effects.VFX.VFXManager;

namespace Gisha.GameOff_2021.NPC
{
    public class Projectile : MonoBehaviour, IDestroyable
    {
        [SerializeField] private float lifeTime = 5f;
        [SerializeField] private float raycastLength;

        private float _flySpeed;
        private Vector2 _flyDirection;
        private Collider2D _turretCollider;
        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        public void OnSpawn(Vector2 direction, float flySpeed, Collider2D turretCollider)
        {
            _flySpeed = flySpeed;
            _flyDirection = direction;
            _turretCollider = turretCollider;
        }

        private void Update()
        {
            transform.Translate(_flyDirection * _flySpeed * Time.deltaTime, Space.World);

            ForwardRaycast();
        }

        private void ForwardRaycast()
        {
            RaycastHit2D[] hitInfo = Physics2D.RaycastAll(transform.position, _flyDirection, raycastLength)
                .Where(x => x.collider != _turretCollider && x.collider != _collider)
                .ToArray();

            if (hitInfo.Length > 0)
            {
                foreach (var t in hitInfo)
                    if (t.collider.TryGetComponent(out IDestroyable destroyable))
                        destroyable.Destroy();

                Destroy();
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
            
            VFXManager.Instance.Emit("S_Explosion", transform.position);
            AudioManager.Instance.PlaySFX("Hit");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, _flyDirection * raycastLength);
        }
    }
}