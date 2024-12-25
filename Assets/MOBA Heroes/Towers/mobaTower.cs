using UnityEngine;
using System.Collections;

public class mobaTower : MonoBehaviour {

	private Vector3 targetDir;
	private Transform target;
	public float rotationDegreePerSecond = 2;

	public GameObject canon;
	public GameObject projectileRed;
	public GameObject crystal;
	public GameObject projectileBlue;
	public GameObject explosion;
	public GameObject hay;
	public float fireInterval;
	private float currentRotation = 0;
	public Transform shootPointRed;
	public Transform shootPointBlue;
	private GameObject magicMissile;

	public Animator animator;
	public GameObject Marker;
	public Transform TargetMarker;
	private Vector3 markerRadius = Vector3.zero;
	private float clickDown = 0;

	private bool isCrystal = false;

	void Start () {
	
	}
	
	void CastRayToWorld()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("DummyLayer")))
		{
			target = hit.transform;
			markerRadius = Vector3.zero;
			StopCoroutine("shootLoop");
			if (!isCrystal)
				animator.SetBool("shoot", false);
			StartCoroutine("shootLoop");			
		}
		else
		{
			StopCoroutine("shootLoop");
			target = null;
			if (!isCrystal)
				animator.SetBool("shoot", false);

			if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("FloorLayer")) && clickDown <= 0)
			{
				Instantiate(Marker, hit.point + Vector3.up * 0.3f, transform.rotation);
				clickDown = 0.3f;
			}
		}


	}

	void Update()
	{
		// get mouseclick
		if (Input.GetMouseButtonDown(0)) //left
		{
			CastRayToWorld();
		}

		if (Input.GetMouseButtonDown(1)) //right
		{
			if (isCrystal)
			{
				crystal.SetActive(false);
				canon.SetActive(true);
				isCrystal = false;
			}
			else
			{
				crystal.SetActive(true);
				canon.SetActive(false);
				isCrystal = true;
			}
		}

		// Look at target		
		if (target)
			targetDir = Vector3.Normalize(target.transform.position - transform.position);
		else
			targetDir = Vector3.forward;

		targetDir.y = 0;
		currentRotation = Mathf.MoveTowardsAngle(currentRotation, Mathf.Atan2(targetDir.x , targetDir.z) / Mathf.PI * 180, rotationDegreePerSecond);
		transform.localRotation = Quaternion.AngleAxis(currentRotation, Vector3.forward);

		//Show targeting circle
		if (target)
		{
			markerRadius = Vector3.Lerp(markerRadius, new Vector3(1, 1, 1), Time.deltaTime * 10);
			TargetMarker.localScale = markerRadius;
			TargetMarker.position = target.position + Vector3.up * 0.5f;
		}
		else
			TargetMarker.position = new Vector3(0, -1, 0);


		// click cooldown
		if (clickDown > 0)
			clickDown -= Time.deltaTime;
	}


	public IEnumerator shootLoop()
	{

		// keep the loop rolling
		if (target)
		{
			yield return new WaitForSeconds(fireInterval);
			StartCoroutine("shootLoop");
		}

		// fire ze missiles!
		if (!isCrystal)
			{
				magicMissile = Instantiate(projectileRed, shootPointRed.position, transform.rotation) as GameObject;
				animator.SetBool("shoot", true);
			}
		else
			magicMissile = Instantiate(projectileBlue, shootPointBlue.position, transform.rotation) as GameObject;
		StartCoroutine(lerpyLoop(magicMissile, target));
		yield return new WaitForSeconds(0.1f);

		if (!isCrystal)
			animator.SetBool("shoot", false);
		
	}

	public IEnumerator lerpyLoop(GameObject projectileInstance, Transform victim)
	{
		float progress = 0;
		float timeScale = 1.0f / 1f;
		Vector3 origin = projectileInstance.transform.position;

		// lerp ze missiles!
		while (progress < 1)
		{
			progress += timeScale * Time.deltaTime;
			projectileInstance.transform.position = Vector3.Lerp(origin, victim.position + Vector3.up * 2.5f, progress);
			yield return null;
		}
		
		Destroy(projectileInstance);
		Instantiate(explosion, victim.position + Vector3.up * 2.5f, transform.rotation);
		Instantiate(hay, victim.position + Vector3.up * 2f, Quaternion.identity);

		var DummyAnimator = victim.GetComponent<Animator>();
		if (DummyAnimator)
		{
			DummyAnimator.SetBool("hit", true);
			yield return new WaitForSeconds(0.1f);
			DummyAnimator.SetBool("hit", false);
		}
	}

}
