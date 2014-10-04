﻿namespace Grove.Tests.Cards
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class HushwingGryff
    {
        public class Ai : AiScenario
        {
            [Fact]
            public void DoesNotActivateTrigger()
            {
                Hand(P1, "Coral Barrier");
                Battlefield(P1, "Island", "Island", "Island");

                Battlefield(P2, "Hushwing Gryff", "Island");

                RunGame(1);

                Equal(0, P1.Battlefield.Count(c => c.Is("Squid")));
            }
        }
    }
}