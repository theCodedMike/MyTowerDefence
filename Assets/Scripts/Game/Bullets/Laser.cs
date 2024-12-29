using Game.Skeletons;
using UnityEngine;

namespace Game.Bullets
{
    public class Laser : MonoBehaviour
    {
        public float length = 500f;         //laser_length
        public float width = 0.1f;          //laser_width
        public float OvarAll_Size = 1.0f;   //eff_scale

        public GameObject hit_effect;       //hitEffect:GameObject

        [SerializeField]
        private GameObject laser_add;       //main_laser_add:GameObject

        [SerializeField]
        private GameObject laser_alpha;         //main_laser_alpha:GameObject

        [SerializeField]
        private GameObject trf_scaleController;         //eff_scale:GameObject


        public Vector3 Direction { private get; set; }// 激光朝向
        public int Damage
        {
            private get; set;
        }// 激光的伤害值

        void Start()
        {

            // Effect Scale
            if (trf_scaleController)
            {
                trf_scaleController.transform.localScale = new Vector3(OvarAll_Size, OvarAll_Size, OvarAll_Size);
            }

            // laser width
            if (laser_add)
            {
                var pa1_width = laser_add.GetComponent<ParticleSystem>().main;
                pa1_width.startSize = width;
            }
            if (laser_alpha)
            {
                var pa2_width = laser_alpha.GetComponent<ParticleSystem>().main;
                pa2_width.startSize = width;
            }

            // laser length
            if (laser_add)
            {
                var pa1_length = laser_add.GetComponent<ParticleSystemRenderer>();
                pa1_length.lengthScale = length / width / 10;
            }
            if (laser_alpha)
            {
                var pa2_length = laser_alpha.GetComponent<ParticleSystemRenderer>();
                pa2_length.lengthScale = length / width / 10;
            }

        }


        // ReSharper disable once MethodTooLong
        public void Render()
        {
            // Effect Scale
            if (trf_scaleController)
            {
                trf_scaleController.transform.localScale = new Vector3(OvarAll_Size, OvarAll_Size, OvarAll_Size);
            }


            // laser length
            if (laser_add)
            {
                var pa1_length = laser_add.GetComponent<ParticleSystemRenderer>();
                pa1_length.lengthScale = length / width / 10;
            }
            if (laser_alpha)
            {
                var pa2_length = laser_alpha.GetComponent<ParticleSystemRenderer>();
                pa2_length.lengthScale = length / width / 10;
            }


            // Hit Controller:
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Direction, out hit))
            {
                if (hit.collider && hit.distance <= length / 10 * OvarAll_Size)
                {

                    if (laser_add)
                    {
                        var pa1_length = laser_add.GetComponent<ParticleSystemRenderer>();
                        pa1_length.lengthScale = hit.distance * 10 / width / 10 / OvarAll_Size;
                    }
                    if (laser_alpha)
                    {
                        var pa2_length = laser_alpha.GetComponent<ParticleSystemRenderer>();
                        pa2_length.lengthScale = hit.distance * 10 / width / 10 / OvarAll_Size;
                    }

                    // 这里稍微修改一下
                    for (int i = 0; i < 10; i++)
                    {
                        //Hit Effect Instance
                        GameObject ins_hiteff = (GameObject)Instantiate(hit_effect, hit.point, Quaternion.identity);
                        ins_hiteff.transform.localScale = new Vector3(OvarAll_Size, OvarAll_Size, OvarAll_Size);
                    }

                    // 业务逻辑
                    if (hit.transform.CompareTag("Skeleton"))
                    {
                        hit.transform.GetComponent<Skeleton>().Damage(Damage);
                    }
                }
            }
            else
            {
                // laser length
                if (laser_add)
                {
                    var pa1_length = laser_add.GetComponent<ParticleSystemRenderer>();
                    pa1_length.lengthScale = length / width / 10;
                }
                if (laser_alpha)
                {
                    var pa2_length = laser_alpha.GetComponent<ParticleSystemRenderer>();
                    pa2_length.lengthScale = length / width / 10;
                }
            }


            // laser width
            if (laser_add)
            {
                var pa1_width = laser_add.GetComponent<ParticleSystem>().main;
                pa1_width.startSize = width;
            }
            if (laser_alpha)
            {
                var pa2_width = laser_alpha.GetComponent<ParticleSystem>().main;
                pa2_width.startSize = width;
            }

        }
    }
}
