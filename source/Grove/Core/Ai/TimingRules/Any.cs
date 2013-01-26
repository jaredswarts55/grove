﻿namespace Grove.Core.Ai.TimingRules
{
  using System.Linq;

  public class Any : TimingRule
  {
    private TimingRule[] _rules;

    public void Rules(params TimingRule[] rules)
    {
      _rules = rules;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return _rules.Any(x => x.ShouldPlay(p));
    }

    public override void Initialize(Game game)
    {
      foreach (var rule in _rules)
      {
        rule.Initialize(game);
      }
    }
  }
}