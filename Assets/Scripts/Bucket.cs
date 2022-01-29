using System;
using UnityEngine;

public class Bucket : MonoBehaviour, IInteractable
{

    [SerializeField] private bool _full;
    [SerializeField] private int _travel;
    [SerializeField] private GameObject _otherEnd, _extraRope, _otherExtraRope;
    
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
        UpdateRope();
    }

    private void OnWater()
    {
        if (_full) return;
        _full = true;
        transform.position += Vector3.down * _travel;
        _otherEnd.transform.position += Vector3.up * _travel;
        UpdateRope();
    }

    private void UpdateRope()
    {
        _extraRope.SetActive(_full);
        _otherExtraRope.SetActive(!_full);
    }
}
