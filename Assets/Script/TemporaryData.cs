using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TemporaryData
{
    public static Vector3 lastPortalPosition = Vector3.zero;
    public static bool UseTemporaryPosition { get; set; } = false;
}
