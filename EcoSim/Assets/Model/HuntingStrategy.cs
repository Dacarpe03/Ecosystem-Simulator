using System.Collections.Generic;

interface HuntingStrategy
{
    Vec3 Hunt(List<Animal> friendly, List<Animal> foes);
}
