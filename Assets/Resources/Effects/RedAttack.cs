using UnityEngine;
using System.Collections;

public class RedAttack : MonoBehaviour
{
    public float SpinSpeed = 10.0f;
    public float TotalSpins = 1080.0f;
    public float CurrentSpins = 0.0f;
    public float LaunchSpeed = 10.0f;
    public float TargetDirection = 180;
    public bool Activate = false;
    public int state = 0;
    static Vector3 middle = new Vector3(0, 0, -1);

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Activate)
        {
            switch (state)
            {
                case 0:
                    if (transform.position != middle)
                        transform.position = Vector3.MoveTowards(transform.position, middle, LaunchSpeed * .025f);
                    else
                    {
                        state++;
                        TotalSpins = 1080;
                    }
                    break;
                case 1:
                    if (CurrentSpins < TotalSpins)
                    {
                        CurrentSpins += SpinSpeed;
                        transform.Rotate(new Vector3(0, 0, 1), SpinSpeed);
                    }
                    else state++;
                    break;
                case 2:
                    if (transform.rotation.z != TargetDirection)
                        transform.Rotate(new Vector3(0, 0, 1), SpinSpeed);
                    else state++;
                    break;
                    //if ((transform.position - middle).magnitude < 50)
                    //{
                    //    transform.Translate((Vector3.one * TargetDirection) * LaunchSpeed);
                    //}
                    //else state++;
                    //break;
                default:
                    //Destroy(this);
                    break;
            }
        }
    }
}
