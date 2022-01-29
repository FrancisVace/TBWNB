using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] private LayerMask _platformLayers, _solidLayers, _interactableLayers, _flipLayers;

    [SerializeField] private float _actionCooldown;
    private bool actionsRestricted;

    [SerializeField] private Element _element;
    private SpriteRenderer _sr;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        if (actionsRestricted) return;
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            if (CheckDirection(Vector2.up, _platformLayers))
            {
                transform.position += Vector3.up;
                PostAction();
            }
        }
        if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            if (CheckDirection(Vector2.down, _platformLayers))
            {
                transform.position += Vector3.down;
                PostAction();
            }
        }
        if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            TrySideways(Vector3.left);
        }
        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            TrySideways(Vector3.right);
        }
    }

    private void TrySideways(Vector3 dir)
    {
        var transform1 = transform;
        var inter = Physics2D.OverlapPoint(transform1.position + dir, _interactableLayers);
        if (inter)
        {
            inter.GetComponent<IInteractable>()?.Interact(_element);
            PostAction();
        }
        else if (!CheckDirection(dir, _solidLayers))
        {
            var flip = CheckDirection(dir, _flipLayers);
            transform1.position += dir;
            PostAction(flip);
        }
    }

    private bool CheckDirection(Vector2 dir, LayerMask layerMask)
    {
        return Physics2D.Raycast(transform.position, dir, 1f, layerMask);
    }

    private void PostAction(bool flip = false)
    {
        actionsRestricted = true;
        if (flip)
        {
            switch (_element)
            {
                case Element.FIRE:
                    _element = Element.WATER;
                    _sr.color = Color.cyan;
                    break;
                case Element.WATER:
                    _element = Element.FIRE;
                    _sr.color = Color.yellow;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        Invoke(nameof(TryAllowAction), _actionCooldown);
    }

    private void TryAllowAction()
    {
        if (!CheckDirection(Vector2.down, _solidLayers | _platformLayers))
        {
            transform.position += Vector3.down;
            PostAction();
        }
        else
        {
            actionsRestricted = false;
        }
    }
}
