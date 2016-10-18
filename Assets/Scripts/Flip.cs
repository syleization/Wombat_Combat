using UnityEngine;
using System.Collections;

public class Flip : MonoBehaviour
{

    public float progress = 0;
    public float height = 5f;

    private Vector3 origin;
    private Vector3 origRot;
    private Vector3 flipRot;

    void Start()
    {
        origin = transform.position;
        flipRot = origRot = transform.rotation.eulerAngles;
        flipRot.z += 180;
    }

    void Update()
    {
        progress = Mathf.Clamp(progress, 0, 90);

        Vector3 pos = origin;
        pos.y = (Mathf.Sin(Mathf.Deg2Rad * progress) * Mathf.Cos(Mathf.Deg2Rad * progress) * height) + origin.y;

        transform.position = pos; // handle moving upwards
        transform.rotation = Quaternion.Lerp(Quaternion.Euler(origRot), Quaternion.Euler(flipRot), progress / 90); // handle rotation

    }
}