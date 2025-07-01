using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AnimationEventSound : MonoBehaviour
{
    [SerializeField] List<NamedEventReference> soundEventsList;

    public void PlaySound(string soundName)
    {
        var ev = soundEventsList.Find(sound => sound.name == soundName).eventReference;
        if (ev.IsNull)
        {
            Debug.LogWarning($"Sound event '{soundName}' not found in {gameObject.name}");
            return;
        }  
        RuntimeManager.PlayOneShot(ev, transform.position);
    }
}
