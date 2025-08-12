using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    public Transform[] waypoints;
    public enum pathType
    {
        LOOP,
        REVERSE
    }
}

