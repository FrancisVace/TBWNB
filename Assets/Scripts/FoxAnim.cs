using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxAnim : MonoBehaviour
{
    private Movement _movement;
    // Start is called before the first frame update
    void Start()
    {
        _movement = Movement.instance;
    }

    public void AllowActions()
    {
        _movement.AllowActions();
    }
}
