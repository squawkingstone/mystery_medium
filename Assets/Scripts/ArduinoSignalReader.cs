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

	public static SerialPort port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);      
    bool updateNeeded;
    string deviceMessage;
    Mutex mutex = new Mutex();

    void Start () 
    { 
        port.Open();
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
        StopAllCoroutines();
        port.Close(); 
    }

    void UpdateProps()
    {
        Debug.Log(deviceMessage);
    }

    private IEnumerator ReadFromDevice()
    {
        string temp;
        while (true)
        {
            temp = port.ReadLine();
            mutex.WaitOne();
            deviceMessage = temp;
            updateNeeded = true;
            mutex.ReleaseMutex();
            yield return null;
        }
    }

}