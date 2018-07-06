using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheels : MonoBehaviour {
	public GameObject Wheel_object;
	public GameObject propeller;
	public Quatornions comunicateScriptQ;
	public Vector3 startPosition;
	public Vector3 throttleDeltaRot;
	public Vector3 throttlePosition;

	public float angle;
	float throttle;
	float diferentaThrottle;
	bool verificaDiferenta = false;
	void Start () 
	{
		startPosition = transform.position;
		InvokeRepeating("updateThrottle", 0.1f, 0.1f);
		comunicateScriptQ = Wheel_object.GetComponent<Quatornions> ();

	}

	void FixedUpdate()
	{
		
		if (verificaDiferenta) 
		{

			
			propeller.transform.Rotate (throttleDeltaRot);
		} 

		else 
		{

			propeller.transform.Rotate(Vector3.forward * ((2 * 40 * Mathf.PI) * throttle * 57f));
		}
	
	}


	void updateThrottle() {
		if (throttle != comunicateScriptQ.avionThrottle ()) 
		{
			verificaDiferenta = true;			
			throttleDeltaRot = new Vector3	(transform.rotation.x,
											transform.rotation.y, 
											((2*40 * Mathf.PI) * comunicateScriptQ.avionThrottle ()) * 57f );
		}	

		else 
		{
			verificaDiferenta = false;
		}


		throttle = comunicateScriptQ.avionThrottle ();
	}

}

