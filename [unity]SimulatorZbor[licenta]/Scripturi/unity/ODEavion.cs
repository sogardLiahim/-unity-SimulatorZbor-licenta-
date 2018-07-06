using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ODEavion : MonoBehaviour {
	
	  int enginePower; 
	  int engineRps;
	  int mass;

	 double np;
	 double propDiameter = 1.905;
	 double Cd = 0.045;
	 double [] dq = new double[4];
	 double throttle;
	 double thrust;

	 double time;
	 double velocity = 0.1;



	public ODEavion(int i) {
		
		Debug.Log ("No.Box" +i);
		enginePower = 850000;
		np = 0.85; 
		engineRps = 40;
		propDiameter = 1.905;
		mass = 50000; //5 t * g


	 throttle = 1.0;
	 thrust = (throttle * enginePower * np) / (engineRps * propDiameter);

	}

	double odeSolver(double dt, double v) {
			double velocityAirplane = dt * (thrust - (Cd * (30 * v * v / 2) * 1.225))/mass;
			return velocityAirplane;
	}



	private double odeIteration() {
		
		double dt = time; // temporal getTime..
		double newVelocity;
		dq[0] = odeSolver(dt,velocity);
		dq[1] = odeSolver((dt + 0.5*0.1), (velocity + 0.5 * 0.1 * dq[0]));
		dq[2] = odeSolver((dt + 0.5*0.1), (velocity + 0.5 * 0.1 * dq[1]));
		dq[3] = odeSolver((dt + 0.1),(velocity + dq[2] * 0.1));


		newVelocity = velocity + 0.16 * (dq [0] + 2 * dq [1] + 2 * dq [2] + dq [3]) * 0.1; //0.1 ptr step-ul sau dt
		return newVelocity;
	}


	public void ruggeKutta() {

			double checkVelocity = velocity; // verificare daca velocity e saturat
			time += 0.1;
			Velocity = odeIteration ();


	}


	//intre 0 si 90 max (45) 2.8482 min 0.1
	public float anglePower (int x) {
	
	
		try 
		{
			if(x<90) return -0.0013571f * x * x + 0.122143f * x + 0.1f;			
			else{ x -= 90; return -0.0013571f * x * x + 0.122143f * x + 0.1f;}
		}
		



		catch {Debug.Log ("-------");}
		return 0.1f;
	}


	//get / set

	public double Time {

		get{ return time; }
		set{ time = value; }
	}

	public double Velocity {

		get{ return velocity; }
		set{ velocity = value; }
	}

	public void setThrottle(double value) {
		throttle = value;
		thrust = (throttle * enginePower * np) / (engineRps * propDiameter);
	}
	public double Throttle { get { return throttle; } }

}
