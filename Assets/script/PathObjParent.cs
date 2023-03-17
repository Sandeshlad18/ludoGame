using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathObjParent : MonoBehaviour
{

    public PathPoint[] commonPath;
    public PathPoint[] bluePath;
    public PathPoint[] yellowPath;
    public PathPoint[] greenPath;
    public PathPoint[] redPath;
    public PathPoint[] basePath;

    [Header("scale and position diff")]
    public float[] scale;
    public float[] positionDifference;
    
}
