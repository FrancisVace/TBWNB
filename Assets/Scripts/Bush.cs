using System;
using System.Collections;
using UnityEngine;

public class Bush : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject _bushPrefab;
    [SerializeField] private LayerMask _solidLayers, _bushLayers;
    
    private Coroutine _staggeredUpdate;
    // Start is called before the first frame update
    void Start()
    {
        _staggeredUpdate = StartCoroutine(StaggeredUpdate());
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
        StopCoroutine(_staggeredUpdate);
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
    
    public IEnumerator StaggeredUpdate()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            if (!Physics2D.OverlapPoint(
                    transform.position + Vector3.down, 
                    _solidLayers | _bushLayers))
            {
                transform.position += Vector3.down;
            }
        }
    }
}
