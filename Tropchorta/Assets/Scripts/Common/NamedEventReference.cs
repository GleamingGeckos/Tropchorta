
using FMODUnity;

[System.Serializable]
public struct NamedEventReference
{
    public string name;
    public EventReference eventReference;

    public NamedEventReference(string name, EventReference eventReference)
    {
        this.name = name;
        this.eventReference = eventReference;
    }
}
