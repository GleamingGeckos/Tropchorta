using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    [SerializeField] public InputReader input;
    [SerializeField] private Collider collider;
    private int layerMask;

    void Awake()
    {
        input.OnInteractEvent += TalkWith;
        // this in Start caused nullrefs in other scripts. Probably order issue.
        PauseController.InputReaderRef = input;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TalkWith()
    {
        //Debug.Log("[DEBUG] Entering TalkWith function");
        Collider[] hitColliders = new Collider[10]; // Bufor na znalezione obiekty
        int numColliders = Physics.OverlapBoxNonAlloc(collider.bounds.center, collider.bounds.extents * 10.0f, hitColliders, Quaternion.identity, ~layerMask);

        //Debug.Log($"[DEBUG] {numColliders} objects overlapping with {collider.gameObject.name}:");
        DebugExtension.DebugWireSphere(collider.bounds.center, Color.red, collider.bounds.extents.x * 5.0f / 2.0f, 1f);
        if (numColliders > 0)
        {
            for (int i = 0; i < numColliders; i++)
            {
                var npc = hitColliders[i].gameObject.GetComponent<NPCBehaviourScript>();
                if (npc != null)
                {
                    npc.TalkBack();
                    break;
                }
            }
        }
    }
}
