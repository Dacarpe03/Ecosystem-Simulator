using System.Collections.Generic;

interface HuntingStrategy
{
    Vec3 Hunt(Dictionary<int, Animal> friendly, Dictionary<int, Animal> foes);
}
