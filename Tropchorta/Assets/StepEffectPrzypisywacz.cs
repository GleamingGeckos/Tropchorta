using UnityEngine;
using UnityEngine.Animations;

public class StepEffectPrzypisywacz : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var stepEffect=GetComponent<PositionConstraint>();
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = GameObject.Find("Player").transform;
        source.weight = 1;
        stepEffect.AddSource(source);
    }
    
}
