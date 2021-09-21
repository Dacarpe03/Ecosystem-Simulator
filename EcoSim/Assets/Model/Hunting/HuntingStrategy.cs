using System.Collections.Generic;

public abstract class HuntingStrategy
{
    public int FramesUpdate;

    public abstract Vec3 GetDesiredPosition(Animal agent, List<Vec3> friendlyPositions, Animal prey);
    public abstract bool HasFixedPrey(Animal agent);
    public abstract void SelectPrey(Dictionary<int, Animal> friendly, Dictionary<int, Animal> foes, Animal agent);
    public abstract int GetFixedPreyId(Animal agent);
    public abstract void HuntPrey(Animal agent, Animal prey);
}
