using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static int coins = 0;
    public static int gems = 0;

     private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
	
}
