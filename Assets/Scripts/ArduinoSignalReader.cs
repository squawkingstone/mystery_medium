using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoSignalReader : MonoBehaviour {

	public static SerialPort port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);      
     
    void Start () 
    {
    	OpenDeviceConnection();
    }
 
    void Update()
    {
     	Debug.Log(port.ReadLine());
    }
     
    void OpenDeviceConnection()
	{
		port.Open();
	}
 
    void OnApplicationQuit() 
    {
        port.Close();
    }
}
