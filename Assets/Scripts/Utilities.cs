using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static Vector3 Wrap(Vector3 v, Vector3 min, Vector3 max, Vector3 offset) 
    {
        Vector3 result = v;

        // x
        if (result.x > max.x + offset.x) result.x = min.x;
        else if (result.x < min.x - offset.x) result.x = max.x;

        // y
        if (result.y > max.y + offset.y) result.y = min.y;
        else if (result.y < min.y - offset.y) result.y = max.y;

        // z
        if (result.z > max.z + offset.z) result.z = min.z;
        else if (result.z < min.z - offset.z) result.z = max.z;

        return result;
    }


}
