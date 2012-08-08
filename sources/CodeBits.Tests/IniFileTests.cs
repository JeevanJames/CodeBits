using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace CodeBits.Tests
{
    public sealed class IniFileTests
    {
         private const string ValidIni =
@"[Game State]
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

            List<string> sections = ini.GetSections().ToList();
            Assert.Equal(3, sections.Count);
            Assert.Equal("Game State", sections[0]);
            Assert.Equal("Jeevan", sections[1]);
            Assert.Equal("Merina", sections[2]);

            IReadOnlyDictionary<string, string> section = ini.GetSection(sections[0]);
            Assert.Equal(2, section.Count);
            Assert.Equal("Jeevan", section["Player1"]);
            Assert.Equal("Merina", section["Player2"]);
        }
    }
}