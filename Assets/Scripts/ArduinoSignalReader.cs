using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoSignalReader : MonoBehaviour {

	public static SerialPort port;      
     
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
        port = new SerialPort("COM3", 9600);
		port.Open();
	}
 
    void OnApplicationQuit() 
    {
        port.Close();
    }
}
