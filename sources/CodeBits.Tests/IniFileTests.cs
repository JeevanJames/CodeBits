using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace CodeBits.Tests
{
    public sealed class IniFileTests
    {

        private const string ValidIni = @"[Game State]
; These are the players
Player1=Jeevan
Player2=Merina

 ; One section per player

[Jeevan]
  Level = 7
  Karma = 5.23
  Weapons = BFG9000, Star

[Merina]
Level=3
Karma=2.95
Weapons=Star, Fists";

        [Fact]
        public void Basic_tests()
        {
            IniFile ini = IniFile.Load(ValidIni);

            Assert.Equal(3, ini.Count);
            Assert.Equal("Game State", ini[0].Name);
            Assert.Equal("Jeevan", ini[1].Name);
            Assert.Equal("Merina", ini[2].Name);

            IniFileSection section = ini[0];
            Assert.Equal(2, section.Count);
            Assert.Equal("Jeevan", section["Player1"]);
            Assert.Equal("Merina", section["Player2"]);
        }
    }
}