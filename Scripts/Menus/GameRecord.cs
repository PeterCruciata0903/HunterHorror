using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRecord : Singleton<GameRecord>
{
    public int g_numberOfNotes = 5;
    public int g_notesObtained = 0;

    private void Start()
    {
        g_numberOfNotes = 5;
        g_notesObtained = 0;
    }
    public int numberOfNotes
    {
        get { return g_numberOfNotes; }
        set { g_numberOfNotes = value; }
    }
    public int notesObtained
    {
        get { return g_notesObtained; }
        set { g_notesObtained = value;}
    }
}
