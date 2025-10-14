using UnityEngine;

public enum TrashType { Plastic, Metal, Paper, Glass }

public class TrashItem : MonoBehaviour
{
    public TrashType trashType;

    [TextArea]
    public string didYouKnowFact;
}

