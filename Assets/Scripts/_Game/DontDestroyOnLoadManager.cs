using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadManager : MonoBehaviour
{
    [SerializeField] List<GameObject> thingsToNotDestroy = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in thingsToNotDestroy)
        {
            DontDestroyOnLoad(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
