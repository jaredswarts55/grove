﻿namespace Grove.Core.Effects
{
  using System;

  public class GainLifeContext<T> : Effect
  {
    public Func<T, int> Selector = delegate { return 0; };

    protected override void ResolveEffect()
    {
      Controller.Life += Selector((T) TriggerContext);
    }
  }
}