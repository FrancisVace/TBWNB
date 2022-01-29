using System;
using System.Collections;
using UnityEngine;

public class Bush : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject _bushPrefab;
    [SerializeField] private LayerMask _solidLayers, _bushLayers;

    // Update is called once per frame
    void Update()
    {
        // y check to prevent infinite loop
        while(transform.position.y > -100 && !Physics2D.OverlapPoint(
                  transform.position + Vector3.down, 
                  _solidLayers | _bushLayers))
        {
            transform.position += Vector3.down;
        }
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
