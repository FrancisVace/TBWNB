using System;
using System.Collections;
using UnityEngine;

public class Bush : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject _bushPrefab;
    [SerializeField] private LayerMask _solidLayers, _bushLayers;
    [SerializeField] private bool _reinforced;
    [SerializeField] private Transform _restingOn;
    private Vector3 _restingOffset;

    private void Start()
    {
        UpdateResting();
    }

    void Update()
    {
        // y check to prevent infinite loop
        if (_restingOn == null)
        {
            while(transform.position.y > -100 && !Physics2D.OverlapPoint(
                      transform.position + Vector3.down, 
                      _solidLayers | _bushLayers))
            {
                transform.position += Vector3.down;
            }
            UpdateResting();
        }
        else
        {
            transform.position = _restingOn.position + _restingOffset;
        }
    }

    private void UpdateResting()
    {
        var rest = Physics2D.OverlapPoint(transform.position + Vector3.down, 
            _solidLayers | _bushLayers);
        if (rest) _restingOn = rest.transform;
        _restingOffset = transform.position - _restingOn.position;
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
        if (_reinforced) return;
        Destroy(gameObject);
    }

    private void OnWater()
    {
        var space = CheckForSpaceAbove();
        if (space.HasValue) Instantiate(_bushPrefab, space.Value, Quaternion.identity);
    }

    private Vector3? CheckForSpaceAbove()
    {
        var solid = Physics2D.OverlapPoint(transform.position + Vector3.up, _solidLayers);
        if (solid) return null;
        var bush = Physics2D.OverlapPoint(transform.position + Vector3.up, _bushLayers);
        if (bush) return bush.GetComponent<Bush>()?.CheckForSpaceAbove();
        return transform.position + Vector3.up;
    }
}
