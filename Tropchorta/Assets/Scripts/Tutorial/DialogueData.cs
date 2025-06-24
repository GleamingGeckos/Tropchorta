using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueData : ScriptableObject
{
    [TextArea(5, 10)]
    [Multiline] public string fullText;
}
