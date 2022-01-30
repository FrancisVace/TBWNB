using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour, IInteractable
{

    [SerializeField] private bool _lit;
    [SerializeField] private GameObject _fireIndicator;
    [SerializeField] private Transform _movingObject;

    [SerializeField] private Transform _litPosition, _otherPosition;
    // Start is called before the first frame update
    void Start()
    {
        _fireIndicator.SetActive(_lit);
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
        if (_lit) return;
        _lit = true;
        _fireIndicator.SetActive(_lit);
        _movingObject.position = _litPosition.position;
    }

    private void OnWater()
    {
        if (!_lit) return;
        _lit = false;
        _fireIndicator.SetActive(_lit);
        _movingObject.position = _otherPosition.position;
    }
}
