using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] private LayerMask _platformLayers, _solidLayers, _interactableLayers, _flipLayers;

    [SerializeField] private float _actionCooldown;
    private bool _actionsRestricted, _inputRestricted;

    [SerializeField] private Element _element;
    [SerializeField] private SpriteRenderer _sr;


    void Update()
    {
        if (_inputRestricted)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) < 0.25f
                && Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.25f)
            {
                _inputRestricted = false;
            }
            return;
        }
        if (_actionsRestricted) return;
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
        _inputRestricted = true;
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

        if (Grounded())
        {
            _actionsRestricted = false;
        }
        else
        {
            _actionsRestricted = true;
            Invoke(nameof(Fall), _actionCooldown);
        }
    }

    private void Fall()
    {
        transform.position += Vector3.down;
        PostAction();
    }

    private bool Grounded()
    {
        return CheckDirection(Vector2.down, _solidLayers | _platformLayers);
    }
}
