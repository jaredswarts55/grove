﻿namespace Grove.Gameplay.Effects
{
  using Artifical;

  public class DestroyPermanent : Effect
  {
    private readonly DynParam<Card> _permanent;

    private DestroyPermanent() {}

    public DestroyPermanent(DynParam<Card> permanent)
    {
      _permanent = permanent;
      Category = EffectCategories.Destruction;

      RegisterDynamicParameters(permanent);
    }


    protected override void ResolveEffect()
    {
      _permanent.Value.Destroy();
    }
  }
}