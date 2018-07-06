using UnityEngine;
using System.Collections;
using System.IO.Ports;


public class Quatornions : MonoBehaviour {


	float angle=20;
	float throttle;
	float lift;
	float drag;
	float Cl = 1.5f;
	float tempTime;
	static float altitude;
	Vector3 aripaUnghi = new Vector3 (0f, 0.5f, 0.866f);
	Quaternion q = new Quaternion (0, 0, 0, -1);
	public static ODEavion avion = new ODEavion(2);
	ThreadArduino threadInfo = new ThreadArduino();
	public Rigidbody rb;
	bool vStall = true;
	static bool isGrounded = true;
	static int stareAvion = 3;
	string StreamRead;

	//arduino
	float anglePot1;
	float anglePot2;
	float throtlePot;
	//arduino
	// Use this for initialization



	void Start () {
		rb = GetComponent<Rigidbody> ();
		InvokeRepeating("getAltitude", 0.3f, 0.5f);
		}









	void FixedUpdate ()
	{
		
		if (throttle >= 0.2f) {
			
			tempTime += Time.deltaTime;
			tempTime = float.Parse (tempTime.ToString ("F2"));

			if (tempTime >= 0.1f) {
				avion.ruggeKutta ();
			//	Debug.Log("Viteza : m/s "+avion.Velocity);
				tempTime = 0;
			}

		}
		lift = (Cl *1.5f*1.225f*(float)avion.Velocity*(float)avion.Velocity*15.2f)/50000f;
	     

		if (lift > 0)
		{
			rb.velocity = transform.forward.normalized * (-(float)avion.Velocity);			
		}

		updateAnglesAvion ();

		switch (stareAvion) {
		case 1:
			{
				q = tractionAirplaneArduino(q);
				transform.rotation = q;
				Debug.Log ("avionul se afla in aer unghiul pitch-yaw e valabil...Default quaternion function");
				break;
			}
		case 2:
			{
				Debug.Log (" avionul se afla in aer dar e in stallspeed");
				break;
			}
		case 3:
			{
				q = tractionAirplaneOnGroundArduino(q);
				transform.rotation = q;
				Debug.Log ("avionul necesita un thrust mai mare pentru decolare.");
				break;
			}
		case 4:
			{
				q = tractionAirplaneOnGroundArduino(q);
				transform.rotation = q;
				Debug.Log ("avionul poate decola dar unghiul pitch-yaw nu e valabil");
				break;
			}

		default:
			break;
		}

		//throttle = getThrottleValue(throttle);
		throttle = throtlePot/10f;
		if (throttle != avion.Throttle) 
		{
			tempTime = 0f;
			avion.Time = 0;

		}
		avion.setThrottle(throttle);

	}





	void Update () {
		StreamRead = threadInfo.GetStreamRead;
		Debug.Log(threadInfo.GetStreamRead);
		threadInfo.SetAvionVelocity = avion.Velocity.ToString ("F1");
		int angleofRotation = (int)Vector3.Angle (aripaUnghi, transform.forward.normalized);
		Cl = avion.anglePower(angleofRotation);



	}



	Quaternion tractionAirplane(Quaternion q) {

		if(Input.GetKey(KeyCode.D)) {

			Quaternion qx = Quaternion.AngleAxis(angle * Time.deltaTime , transform.forward);
			q = qx * q;
		}

		if(Input.GetKey(KeyCode.A)) {

			Quaternion qx = Quaternion.AngleAxis(angle * Time.deltaTime , -transform.forward);
			q = qx * q;
		}


		if(Input.GetKey(KeyCode.S)) {

			Quaternion qx = Quaternion.AngleAxis(angle * Time.deltaTime , transform.right);
			q = qx * q;
		}

		if(Input.GetKey(KeyCode.W)) {

			Quaternion qx = Quaternion.AngleAxis(angle * Time.deltaTime , -transform.right);
			q = qx * q;
		}

		return q;
	}

	Quaternion tractionAirplaneOnGround(Quaternion q) {



		if(Input.GetKey(KeyCode.D)) {

			Quaternion qx = Quaternion.AngleAxis(5 * Time.deltaTime , transform.up);
			q = qx * q;
		}

		if(Input.GetKey(KeyCode.A)) {

			Quaternion qx = Quaternion.AngleAxis(5 * Time.deltaTime , -transform.up);
			q = qx * q;
		}


		if((Input.GetKey(KeyCode.S) && stareAvion == 4)) {

			Quaternion qx = Quaternion.AngleAxis(angle * Time.deltaTime , transform.right);
			q = qx * q;
		}


		return q;
	}

	//arduino
	Quaternion tractionAirplaneArduino(Quaternion q) {


		Quaternion qx = Quaternion.AngleAxis(anglePot2 * Time.deltaTime , transform.forward.normalized);
			q = qx * q;

		qx = Quaternion.AngleAxis(anglePot1 * Time.deltaTime , transform.right.normalized);
			q = qx * q;


		return q;
	}

	Quaternion tractionAirplaneOnGroundArduino(Quaternion q) 

		{

		Quaternion qx = Quaternion.AngleAxis (anglePot2 * Time.deltaTime, transform.up.normalized);
			q = qx * q;

			if (stareAvion == 4) {

			qx = Quaternion.AngleAxis (anglePot1 * Time.deltaTime, transform.right.normalized);
				q = qx * q;
			}
		return q;
		}
	
  //arduino
	//implementare Slider 

	float getThrottleValue(float throttle1) { 

		var d = Input.GetAxis("Mouse ScrollWheel");

			if (d > 0f && throttle < 1.0f) {			
			
					throttle += d;
					throttle = float.Parse(throttle.ToString ("F1"));
				}

			else if (d < 0f && throttle > 0.0f) {
			
					throttle += d;
					throttle = float.Parse(throttle.ToString ("F1"));
				}
			
		return throttle;
	}

	public void getAltitude () {
		altitude = transform.position.y;
		if (avion.Velocity > 50f) {
			vStall = false;
		}
		else
		{
			vStall = true;
		}

		if (altitude > 1.0f) {
			isGrounded = false;		
		} 
		else
		{
			isGrounded = true;
		}



		if (!vStall && !isGrounded)
		{
		 stareAvion = 1; //avionul se afla in aer unghiul pitch-yaw e valabil...Default quaternion function
		}
		else if (vStall && !isGrounded)
		{
		 stareAvion = 2; // avionul se afla in aer dar e in stallspeed
		} 
		else if (vStall && isGrounded) 
		{
		 stareAvion = 3; //avionul necesita un thrust mai mare pentru decolare.
		} 
		else if (!vStall && isGrounded)
		{
		 stareAvion = 4; // avionul poate decola dar unghiul pitch-yaw nu e valabil
		}


	//	Debug.Log (altitude+" vstall :"+ vStall +" Grounded ? :"+isGrounded+ " stare avion "+ stareAvion);
	
	}


	void updateAnglesAvion()

		{
		try {
		string[] values = threadInfo.GetStreamRead.Split ('|');
			anglePot1 = (float.Parse(values[0]));
			anglePot2 = (float.Parse(values[1]));
			throtlePot = (float.Parse(values[2]));
		}
		catch (System.Exception ex)
		{
			Debug.Log ("Format problem" + ex.Message);
		}
		}


	public double avionVelocity() { return avion.Velocity;}
	public Vector3 avionPositie() { return transform.position;}
	public float avionThrottle() { return throttle;}


}
