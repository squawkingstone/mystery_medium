using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEditor;
using System.Threading;

public class ArduinoSignalReader : MonoBehaviour {

    private enum Props
    {
        LETTERS = 0,
        INTIMATES = 1,
        VERMOUTH = 2,
        PARIS_GREEN = 3,
        LAMP = 4,
        BOTTLES = 5,
        NUM_PROPS = 6
    }

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

    [SerializeField] TriggerEnd end;

	public static SerialPort port;      
    bool updateNeeded;
    string deviceMessage;
    Mutex mutex = new Mutex();
    Thread reader;

    ToggleProp[] props;
    bool[] propStates;

    void Start () 
    { 
        // Port Opening
        port = new SerialPort("COM4", 9600);
        port.Open();

        // Spin up reader thread
        reader = new Thread(new ThreadStart(ReadFromDevice));
        reader.Start();

        // Initialize update variables
        updateNeeded = false; 

        // Initialize prop array
        props = new ToggleProp[(int)Props.NUM_PROPS];
        props[(int)Props.LETTERS] = letters;
        props[(int)Props.INTIMATES] = intimates;
        props[(int)Props.VERMOUTH] = vermouth;
        props[(int)Props.PARIS_GREEN] = parisGreen;
        props[(int)Props.LAMP] = lamp;
        props[(int)Props.BOTTLES] = bottles;

        // Initialize prop state array
        propStates = new bool[(int)Props.NUM_PROPS];
        for (int i = 0; i < (int)Props.NUM_PROPS; i++) { propStates[i] = false; }
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
        List<string> tags = new List<string>(deviceMessage.Split(','));
        //for (int i = 0; i < tags.Count; i++) { Debug.Log(tags[i]); }

        if (tags.Contains(gardenerID) && tags.Contains(daughterID) && 
            tags.Contains(stainID) && tags.Contains(paintingID))
        {
            end.Trigger();
        }
        bool[] states = new bool[(int)Props.NUM_PROPS];
        for (int i = 0; i < (int)Props.NUM_PROPS; i++) { states[i] = false; }
        // for each connection do the thing
        UpdateConnection(tags[0], tags[5], states);
        UpdateConnection(tags[1], tags[3], states);
        UpdateConnection(tags[2], tags[4], states);
        // run through the states, and update if the states are different
        for (int i = 0; i < (int)Props.NUM_PROPS; i++)
        {
            if (propStates[i] != states[i])
            {
                props[i].SetRevealed(states[i]);
                propStates[i] = states[i];
            }
        }
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

    void UpdateConnection(string s1, string s2, bool[] states)
    {
        states[(int)Props.LETTERS] = ConnectionEquals(s1, s2, daughterID, drawerID) 
            || states[(int)Props.LETTERS];

        states[(int)Props.INTIMATES] = ConnectionEquals(s1, s2, daughterID, gardenerID) 
            || states[(int)Props.INTIMATES];

        states[(int)Props.VERMOUTH] = ConnectionEquals(s1, s2, glassID, stainID)
            || states[(int)Props.VERMOUTH];

        states[(int)Props.PARIS_GREEN] = ConnectionEquals(s1, s2, gardenerID, paintingID) 
            || states[(int)Props.PARIS_GREEN];

        states[(int)Props.LAMP] = ConnectionEquals(s1, s2, victimID, drawerID) 
            || states[(int)Props.LAMP];

        states[(int)Props.BOTTLES] = ConnectionEquals(s1, s2, paintingID, stainID) 
            || states[(int)Props.BOTTLES];
    }

    bool ConnectionEquals(string s1, string s2, string c1, string c2)
    {
        return (s1 == c1 && s2 == c2) || (s1 == c2 && s2 == c1);
    }

}