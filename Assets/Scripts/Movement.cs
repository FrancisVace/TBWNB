using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static Movement instance;

    [SerializeField] private LayerMask _platformLayers, _solidLayers, _interactableLayers, _flipLayers;

    [SerializeField] private float _actionCooldown;
    private bool _actionsRestricted, _inputRestricted;

    [SerializeField] private Element _element;
    [SerializeField] private Animator _foxAnim;

    private bool _facingRight = true;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(gameObject);
    }


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
            if (CheckFacing(false))
                TrySideways(Vector3.left);
        }
        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            if (CheckFacing(true))
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

    private bool CheckFacing(bool faceRight)
    {
        _inputRestricted = true;
        if (faceRight == _facingRight) return true;
        var transform1 = transform;
        var localScale = transform1.localScale;
        localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        transform1.localScale = localScale;
        _facingRight = faceRight;
        return false;
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
                    _foxAnim.SetTrigger("Water");
                    break;
                case Element.WATER:
                    _element = Element.FIRE;
                    _foxAnim.SetTrigger("Fire");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (Grounded())
        {
            _actionsRestricted = flip;
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

    public void AllowActions()
    {
        _actionsRestricted = false;
    }
}
