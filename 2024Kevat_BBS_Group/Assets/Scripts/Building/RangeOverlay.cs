using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RangeOverlay : MonoBehaviour, IPreviewAware
{
    [SerializeField, Min(3)]
    private int segmentCount = 16;

    [SerializeField]
    private float radius = 1;

    [SerializeField]
    private Color color = Color.white;

    private LineRenderer lineRenderer;
    private Animator animator;
    private Collider2D triggerCollider;
    private readonly List<Material> initialMaterials = new();
    private bool visible;

    private void OnValidate()
    {
        var target = GetComponent<LineRenderer>();
        UpdatePoints(target);
        UpdateColor(target);
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();
        triggerCollider = GetComponent<Collider2D>();
        lineRenderer.GetMaterials(initialMaterials);
    }

    private void OnEnable()
    {
        UpdatePoints(lineRenderer);
        UpdateColor(lineRenderer);
    }

    private void Update()
    {
        UpdateColor(lineRenderer);
        CheckHover();
    }

    public void OnPreviewBegin()
    {
        // Play start animation
        animator.SetTrigger("Show");
    }

    public void OnPreviewUpdate()
    {
        // We have to move the circle
        UpdatePoints(lineRenderer);
        // Reset material override (we know better)
        lineRenderer.SetMaterials(initialMaterials);
    }

    private void CheckHover()
    {
        var cam = Camera.main;
        if (cam == null || triggerCollider == null) return;
        var newVisible = triggerCollider.OverlapPoint(cam.ScreenToWorldPoint(Input.mousePosition));

        if (newVisible && !visible)
        {
            animator.SetTrigger("Show");
        } else if (!newVisible && visible)
        {
            animator.SetTrigger("Hide");
        }

        visible = newVisible;
    }

    private void UpdatePoints(LineRenderer target)
    {
        if (target == null) throw new NullReferenceException("UpdatePoints called with null line renderer");
        var angleIncrement = 2 * math.PI / segmentCount;
        target.positionCount = segmentCount;
        for (var i = 0; i < segmentCount; i++)
        {
            target.SetPosition(i, transform.position + new Vector3(math.sin(angleIncrement * i), math.cos(angleIncrement * i)) * radius);
        }
    }

    private void UpdateColor(LineRenderer target)
    {
        var gradient = target.colorGradient;
        gradient.SetKeys(new[] { new GradientColorKey(color, 0) }, new[] { new GradientAlphaKey(color.a, 0) });
        target.colorGradient = gradient;
    }
}