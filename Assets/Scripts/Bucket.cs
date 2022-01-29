using System;
using UnityEngine;

public class Bucket : MonoBehaviour, IInteractable
{

    [SerializeField] private bool _full;
    [SerializeField] private int _travel;
    [SerializeField] private GameObject _otherEnd;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Interact(Element element)
    {
        switch (element)
        {
            case Element.FIRE:
                OnFire();
                break;
            case Element.WATER:
                OnWater();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(element), element, null);
        }
    }
    
    private void OnFire()
    {
        if (!_full) return;
        _full = false;
        transform.position += Vector3.up * _travel;
        _otherEnd.transform.position += Vector3.down * _travel;
    }

    private void OnWater()
    {
        if (_full) return;
        _full = true;
        transform.position += Vector3.down * _travel;
        _otherEnd.transform.position += Vector3.up * _travel;
    }
}
