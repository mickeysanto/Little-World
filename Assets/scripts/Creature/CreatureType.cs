using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureType : MonoBehaviour
{
    public enum typeOfCreature
    {
        Carnivore,
        Herbivore,
        Omnivore,
        Dead
    }

    public typeOfCreature type;
}
