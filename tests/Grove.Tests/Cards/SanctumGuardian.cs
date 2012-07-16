﻿namespace Grove.Tests.Cards
{
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class SanctumGuardian
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void PreventDamageFromBolt()
      {
        var guardian = C("Sanctum Guardian");
        var bolt = C("Lightning Bolt");

        Hand(P1, bolt);
        Battlefield(P2, guardian);

        Exec(
          At(Step.FirstMain)
            .Cast(bolt, P2)
            .Activate(p =>
              {
                p.Card = guardian;
                p.Targets(E(bolt), P2);
              })
            .Verify(() =>
              {
                Equal(20, P2.Life);
                Equal(Zone.Graveyard, C(guardian).Zone);
              })
          );
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void TradeForBetterCreature()
      {
        var dragon = C("Shivan Dragon");
        var bolt1 = C("Lightning Bolt");
        var bolt2 = C("Lightning Bolt");

        Battlefield(P2, dragon, "Sanctum Guardian");
        Hand(P1, bolt1, bolt2);

        Exec(
          At(Step.FirstMain)
            .Cast(bolt1, dragon)
            .Cast(bolt2, dragon),
          At(Step.SecondMain)
            .Verify(() => Equal(Zone.Battlefield, C(dragon).Zone))
          );
      }

      [Fact]
      public void PreventSpellDamageToPlayerOnce()
      {
        var bolt = C("Lightning Bolt");
        var shock = C("Shock");        

        Hand(P1, bolt, shock);
        Battlefield(P2, "Sanctum Guardian");

        P2.Life = 3;

        Exec(
          At(Step.FirstMain)
            .Cast(bolt, P2)
            .Cast(shock, P2),            
          At(Step.SecondMain)
           .Verify(()=> Equal(1, P2.Life)));

      }

      [Fact]
      public void PreventCombatDamage()
      {
        var guardian = C("Sanctum Guardian");
        var baloth1 = C("Leatherback Baloth");
        var baloth2 = C("Leatherback Baloth");

        P2.Life = 4;

        Battlefield(P1, baloth1, baloth2);
        Battlefield(P2, guardian);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(baloth1, baloth2),
          At(Step.SecondMain)
            .Verify(() => Equal(4, P2.Life))
          );
      }
    }
  }
}