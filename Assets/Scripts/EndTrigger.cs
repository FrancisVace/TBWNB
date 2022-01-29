using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{

    [SerializeField] private string _nextLevel;

    private Transform _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = Movement.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.SqrMagnitude(_player.position - transform.position) < 0.5f)
        {
            SceneManager.LoadScene(_nextLevel);
        }
    }
}
