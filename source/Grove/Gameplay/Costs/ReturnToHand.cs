﻿namespace Grove.Gameplay.Costs
{
  using Targeting;

  public class ReturnToHand : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      result.CanPay = true;
    }

    protected override void Pay(ITarget target, int? x, int repeat)
    {
      Card.PutToHand();
    }
  }
}