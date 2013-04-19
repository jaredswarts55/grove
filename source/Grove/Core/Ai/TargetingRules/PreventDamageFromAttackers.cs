﻿namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class PreventDamageFromAttackers : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
        .OrderByDescending(x => x.CalculateCombatDamage(allDamageSteps: true));

      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }
  }
}