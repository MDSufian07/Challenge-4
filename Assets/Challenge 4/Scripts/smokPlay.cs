using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smokPlay : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) // Check if the player object exists
        {
            // Set the position of smokPlay to match the player's position
            transform.position = player.transform.position;
        }
    }
}
