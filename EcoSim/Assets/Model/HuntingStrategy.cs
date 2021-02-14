using System.Collections.Generic;

interface HuntingStrategy
{
    public Vec3 Hunt(List<Animal> friendly, List<Animal> foes);
}
