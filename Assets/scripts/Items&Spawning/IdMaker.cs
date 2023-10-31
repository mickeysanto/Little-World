using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdMaker : MonoBehaviour
{
    public uint id = 0;

    void Start()
    {
        id = 0;
    }

    public uint getID()
    {
        return id++;
    }

}
