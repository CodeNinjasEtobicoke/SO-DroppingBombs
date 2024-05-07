using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private spawner spawner;


    void Awake()
    {
        spawner = GameObject.Find("Spawner").GetComponent<spawner>();
    }
    // Start is called before the first frame update
    void Start()
    {
        spawner.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            spawner.active = true;
        }
    }
}
