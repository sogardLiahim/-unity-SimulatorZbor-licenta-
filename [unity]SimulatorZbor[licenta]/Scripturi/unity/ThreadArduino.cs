using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class ThreadArduino : MonoBehaviour {

	Thread arduino;

    public static string streamRead; 
	public SerialPort serial = new SerialPort("COM3",250000);

	public static string avionVelocity;


	void Start () {
		try 
		{
			if(serial.IsOpen)
			{
				serial.Close();
				serial.Open();
			}
			else 
			{
				serial.Open();
			}
		}

		catch (System.Exception ex) 
		{
			Debug.Log ("Can not open serial port" + ex.Message);
		}

		arduino = new Thread (new ThreadStart (getArduino));
		arduino.Start ();
	}

	// Update is called once per frame
	void Update () {




	}

	private void getArduino () {

		while (arduino.IsAlive) 
		{
			try 
			{
				serial.WriteLine (avionVelocity);
			} 
			catch (System.Exception ex)
			{
				Debug.Log ("?????????????????");
			}

			try 
			{
				SetStreamRead = serial.ReadLine();
			}
			catch (System.Exception ex) 
			{
				throw;	
			}

		}
	}



	public string GetStreamRead {get{ return streamRead; }}
	private string SetStreamRead{set{ streamRead = value; }}
	public string SetAvionVelocity{ set { avionVelocity = value; } }

}
