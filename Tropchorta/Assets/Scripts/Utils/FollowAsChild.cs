using UnityEngine;

/// <summary>
/// FOLLOW AS CHILD ZADZIAŁA JAK DASZ GO NA SAMYM DOLE W SCRIPT EXECUTION ORDER
/// </summary>
public class FollowAsChild : MonoBehaviour
{
    [Header("Parent")]
    public Transform ParentTransform;
    
    [Header("Misc")]
    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;
    private Vector3 initialLocalScale;
    private Transform lastParentTransform;

    void Start()
    {
        if (ParentTransform != null)
        {
            UpdateOffsets();
        }
    }

    void LateUpdate()
    {
        if (ParentTransform == null) return;

        // Detect if the parent changed
        if (ParentTransform != lastParentTransform)
        {
            UpdateOffsets();
        }
        
        transform.position = ParentTransform.TransformPoint(initialLocalPosition);
        transform.rotation = ParentTransform.rotation * initialLocalRotation;
        transform.localScale = Vector3.Scale(ParentTransform.localScale, initialLocalScale);
    }
    
    public void SetNonHierarchicalParent(Transform newParent)
    {
        ParentTransform = newParent;
        UpdateOffsets();
    }
    
    private void UpdateOffsets()
    {
        if (ParentTransform == null) return;

        lastParentTransform = ParentTransform;

        initialLocalPosition = ParentTransform.InverseTransformPoint(transform.position);
        initialLocalRotation = Quaternion.Inverse(ParentTransform.rotation) * transform.rotation;

        // Convert world scale to relative scale
        initialLocalScale = Vector3.Scale(transform.localScale, new Vector3(
            1f / ParentTransform.localScale.x,
            1f / ParentTransform.localScale.y,
            1f / ParentTransform.localScale.z));
    }
    
}