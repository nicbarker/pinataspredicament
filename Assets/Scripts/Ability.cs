public enum Ability
{
  MOVE_LEFT,
  MOVE_RIGHT,
  JUMP,
  DOUBLE_JUMP,
  DICE_GUN,
}

public static class AbilityExtensions
{
  public static string GetLabel(this Ability ability)
  {
    switch (ability)
    {
      case Ability.MOVE_LEFT:
        return "Left";
      case Ability.MOVE_RIGHT:
        return "Right";
      case Ability.JUMP:
        return "Jump";
      case Ability.DOUBLE_JUMP:
        return "Double Jump";
      case Ability.DICE_GUN:
        return "Dice Gun";
      default:
        throw new System.Exception($"Unrecognized ability {ability.ToString()}");
    }
  }

  public static string GetImagePath(this Ability ability)
  {
    switch (ability)
    {
      case Ability.MOVE_LEFT:
        return "Assets/Sprites/UI/Left.png";
      case Ability.MOVE_RIGHT:
        return "Assets/Sprites/UI/Right.png";
      case Ability.JUMP:
        return "Assets/Sprites/UI/Up.png";
      case Ability.DOUBLE_JUMP:
        return "Assets/Sprites/UI/DoubleJump.png";
      case Ability.DICE_GUN:
        return "Assets/Sprites/UI/DiceGun.png";
      default:
        throw new System.Exception($"Unrecognized ability {ability.ToString()}");
    }
  }
}
