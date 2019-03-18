using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEditor;
using System.Threading;
using System;

/* So this thing is gonna get massively reworked

    This needs to be made wildly more modular, we're gonna do that in 2 parts.

    All the arduino specific stuff (reading from the arduino, parsing the tags)
    is going to it's own separate component to handle the Arduino stuff independantly.
    This way, it separates out the Arduino from the game manager layer, allowing me
    to implement a debug mode by just toggling some things on and off. And on that
    note

    Separate out the gameplay layer stuff and make it modular as fuck. This way we
    can have a system where we can slot in new stuff to build out more levels.

    Maybe break this out into a third component MVC style to translate from arduino
    RFID tags to something more workable in the manager layer, this would again
    make it way easier to insert a debug mode into this workflow

    Thirdly, build in a debug mode. That way I can test game interaction without
    being in VR. This will speed up development massively.
 */


 public class ArduinoSignalReader : MonoBehaviour
 { 
    [System.Serializable]
    public struct spots
    {
        public int first;
        public int second;
    }
 
    [SerializeField] spots[] connected_spots;
    [SerializeField] string[] evidenceID;
    [SerializeField] string[] cardID;
    [SerializeField] string topRow;
    [SerializeField] string bottomRow;
    
    [SerializeField] EvidenceModel model;

    private SerialPort top;
    private SerialPort bottom;

    private string topMessage;
    private string bottomMessage;

    private bool topUpdate;
    private bool bottomUpdate;

    private Dictionary<string,string> evidence;
    private string[] evidenceState;

    Mutex mutexTop = new Mutex();
    Mutex mutexBottom = new Mutex();

    Thread topThread;
    Thread bottomThread;

    void Start()
    {  
        evidence = new Dictionary<string,string>();
        for(int i = 0; i < cardID.Length; i++){ evidence.Add(cardID[i], evidenceID[i]); }

        evidenceState = new string[6];
        for(int i = 0; i < 6; i++){
            evidenceState[i] = "none";
        }

        topMessage = "0,00000000";
        bottomMessage = "0,00000000";
        topUpdate = false;
        bottomUpdate = false;

        top = new SerialPort(topRow, 9600);
        bottom = new SerialPort(bottomRow,9600);
        System.IO.Ports.SerialPort.GetPortNames();

        top.Open();
        topThread = new Thread(new ThreadStart(readTopRow));;
        topThread.Start();

        bottom.Open();
        bottomThread = new Thread(new ThreadStart(readBottomRow));
        bottomThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (topUpdate || bottomUpdate)
        {
            mutexTop.WaitOne();
            mutexBottom.WaitOne();
            Debug.Log(topMessage);
            Debug.Log(bottomMessage);
            updateProps();
            topUpdate = false;
            bottomUpdate = false;
            mutexBottom.ReleaseMutex();
            mutexTop.ReleaseMutex();
        }
    }

    void OnApplicationQuit(){
        topThread.Abort();
        bottomThread.Abort();
        top.Close();
        bottom.Close();
    }

    void readTopRow(){
        string t;
        while (true)
        {   
            t = top.ReadLine();
            mutexTop.WaitOne();
            topMessage = t;
            topUpdate = true;
            mutexTop.ReleaseMutex();
        }
    }

    void readBottomRow(){
        string b;
        while (true)
        {
            b = bottom.ReadLine();
            mutexBottom.WaitOne();
            bottomMessage = b;
            bottomUpdate = true;
            mutexBottom.ReleaseMutex();
        }
    }

    void updateProps(){
        string[] topLine = topMessage.Split(',');
        topLine[0].Replace(" ","");
        topLine[1].Replace(" ","");
        Debug.Log(topLine[0]+" "+evidence[topLine[1]]);
        
        string[] bottomLine = bottomMessage.Split(',');
        bottomLine[0].Replace(" ","");
        bottomLine[1].Replace(" ","");
        Debug.Log(bottomLine[0]+" "+evidence[bottomLine[1]]);

        int tindex = int.Parse(topLine[0]);
        int bindex = int.Parse(bottomLine[0]);

        evidenceState[tindex] = evidence[topLine[1]];
        evidenceState[bindex + 3] = evidence[bottomLine[1]];

        List<Connection<string>> connections = new List<Connection<string>>();
        foreach (spots s in connected_spots)
        {
            connections.Add(new Connection<string>(evidenceState[s.first], evidenceState[s.second]));
        }

        model.UpdateState(connections);
    }
 }