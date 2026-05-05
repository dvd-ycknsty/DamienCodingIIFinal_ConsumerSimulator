using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CullObject : MonoBehaviour
{
    public Collider culTarget;

    private Renderer _myRender;

    private SphereCollider _cullCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _cullCollider = GetComponent<SphereCollider>();
        _cullCollider.isTrigger = true;
        _myRender = transform.parent.GetComponent<Renderer>();
        _myRender.enabled = false;

        if (culTarget == null)
        {
            Debug.LogWarning("No culling target specified for =" + gameObject + "!");
        }
    }

    private void OnTriggerEnter(Collider myCollider)
    {
        if (myCollider == culTarget)
        {
            _myRender.enabled = true;
        }
    }

    private void OnTriggerExit(Collider myCollider)
    {
        if (myCollider == culTarget)
        {
            _myRender.enabled = false;
        }
    }
}