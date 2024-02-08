using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Singleton;
    [SerializeField] public GameObject player;
    [SerializeField] private float offset;

    private void Start()
    {
        if(Singleton == null) { Singleton = this; }
    }
    private void Update()
    {
        if (GameManager.Singleton.initPlayerPos.x - player.transform.position.x < offset)
        {
            transform.position = new Vector3(player.transform.position.x + 10f, transform.position.y,transform.position.z);
        }
    }
}
