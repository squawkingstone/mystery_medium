using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


public class arduinoConnection : MonoBehaviour
{

    SerialPort stream = new SerialPort("COM3", 9600);
    int button = 0;
    // Start is called before the first frame update
    void Start()
    {
        stream.Open();
    }

    // Update is called once per frame
    void Update()
    {
        string value = stream.ReadLine();
        button = int.Parse(value);
        Debug.Log(button);
    }
}
