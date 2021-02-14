using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface HuntingStrategy
{
    public Vec3 Hunt(List<Animal> friendly, List<Animal> foes);
}
