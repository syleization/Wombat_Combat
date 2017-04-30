using UnityEngine;
using System.Collections;

public abstract class Attack : MonoBehaviour
{
    public GameObject target;
    public Vector3 endPos;
    public bool stop = false;

    public abstract void Terminate();
}