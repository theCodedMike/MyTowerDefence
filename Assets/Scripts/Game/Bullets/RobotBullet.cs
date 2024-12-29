using UnityEngine;

namespace Game.Bullets
{
    public class RobotBullet : MonoBehaviour
    {
        public float lifeTime = 10f;
        public GameObject HitSplash;


        private void Awake()
        {
            Destroy(gameObject, lifeTime);
        }

        /*private void OnTriggerEnter(Collision collision)
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, contact.normal); // turn to Normal
            Vector3 pos = contact.point;

            if (HitSplash != null)
            {
                var hitVFX = Instantiate(HitSplash, pos, rot);
            }

            Destroy(gameObject);
            //Debug.Log("Collision happened");
        }*/

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground"))
            {
                Instantiate(HitSplash, other.transform.position, Quaternion.identity);

                Invoke(nameof(Destroy), 1f);
            }
        }
    }
}
