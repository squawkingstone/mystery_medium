using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEditor;
using System.Threading;

public class ArduinoSignalReader : MonoBehaviour {

    [Header("Toggle Props")]
    [SerializeField] ToggleProp letters;
    [SerializeField] ToggleProp intimates;
    [SerializeField] ToggleProp vermouth;
    [SerializeField] ToggleProp parisGreen;
    [SerializeField] ToggleProp lamp;
    [SerializeField] ToggleProp bottles;
    [Space]
    [Header("RFID Tags")]
    [SerializeField] string daughterID;
    [SerializeField] string gardenerID;
    [SerializeField] string victimID;
    [SerializeField] string drawerID;
    [SerializeField] string glassID;
    [SerializeField] string stainID;
    [SerializeField] string paintingID;

	public static SerialPort port;      
    bool updateNeeded;
    string deviceMessage;
    Mutex mutex = new Mutex();
    Thread reader;

    void Start () 
    { 
        port = new SerialPort("COM3", 9600);
        port.Open();
        reader = new Thread(new ThreadStart(ReadFromDevice));
        reader.Start();
        updateNeeded = false; 
    }
 
    void Update()
    {
        if (updateNeeded)
        {
            mutex.WaitOne();
            UpdateProps();
            updateNeeded = false;
            mutex.ReleaseMutex();
        }
    }
 
    void OnApplicationQuit() 
    { 
        reader.Abort();
        port.Close(); 
    }

    void UpdateProps()
    {
        Debug.Log(deviceMessage);
    }

    void ReadFromDevice()
    {
        string temp;
        while (true)
        {
            temp = port.ReadLine();
            mutex.WaitOne();
            deviceMessage = temp;
            updateNeeded = true;
            mutex.ReleaseMutex();
        }
    }

}