using System.Collections.Generic;

public abstract class HuntingStrategy
{
    public int FramesUpdate;
    public abstract Vec3 GetDesiredPosition(Animal agent, List<Vec3> friendlyPositions, Animal prey);
}
