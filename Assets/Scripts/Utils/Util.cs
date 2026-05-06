using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static RaycastHit GetMouseToRay(Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit;
        }
        return hit;
    }
}
