using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTileController : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        Camera camera = other.gameObject.GetComponent<Camera>();
        if (camera == null)
            return;

        Vector3 dir = camera.transform.position - transform.position;

        float dirX = dir.x < 0 ? -1 : 1;
        float dirY = dir.y < 0 ? -1 : 1;
        
        //Debug.Log($"Abs(X) : {Mathf.Abs(dir.x)}, Abs(Y) : {Mathf.Abs(dir.y)}");
        
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            transform.Translate(Vector3.right * dirX * 200);
        else
            transform.Translate(Vector3.up * dirY * 200);
    }
}
