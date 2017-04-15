using UnityEngine;
using System.Collections;

public abstract class Attack : MonoBehaviour
{
    public GameObject target;

    public abstract void Terminate();
}
