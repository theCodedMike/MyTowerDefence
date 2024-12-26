using UnityEngine;

namespace Game
{
    public class Bullet : MonoBehaviour
    {
        [Header("移动速度")]
        public float moveSpeed;

        private Rigidbody rb;


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }


        public void SetVelocity(Vector3 dir)
        {
            if (rb == null)
                rb = GetComponent<Rigidbody>();

            rb.velocity = dir * moveSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground"))
            {
                Invoke(nameof(Destroy), 1f);
            }
        }

        private void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}
