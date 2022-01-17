using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static Vector3 Wrap(Vector3 v, Vector3 min, Vector3 max) 
    {
        Vector3 result = v;

        // x
        if (result.x > max.x) result.x = min.x;
        else if (result.x < min.x) result.x = max.x;

        // y
        if (result.y > max.y) result.y = min.y;
        else if (result.y < min.y) result.y = max.y;

        // z
        if (result.z > max.z) result.z = min.z;
        else if (result.z < min.z) result.z = max.z;

        return result;
    }


}
