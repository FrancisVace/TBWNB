using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] private LayerMask _platformLayers, _solidLayers;

    [SerializeField] private float _actionCooldown;
    private bool actionsRestricted;

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
            if (!CheckDirection(Vector2.left, _solidLayers))
            {
                transform.position += Vector3.left;
                PostAction();
            }
        }
        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            if (!CheckDirection(Vector2.right, _solidLayers))
            {
                transform.position += Vector3.right;
                PostAction();
            }
        }
    }

    private bool CheckDirection(Vector2 dir, LayerMask layerMask)
    {
        return Physics2D.Raycast(transform.position, dir, 1f, layerMask);
    }

    private void PostAction()
    {
        actionsRestricted = true;
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
