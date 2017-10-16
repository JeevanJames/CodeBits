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
            IniFile ini = IniFile.Load(ValidIni, null);

            Assert.Equal(3, ini.Count);
            Assert.Equal("Game State", ini[0].Name);
            Assert.Equal("Jeevan", ini[1].Name);
            Assert.Equal("Merina", ini[2].Name);

            IniFile.Section section = ini[0];
            Assert.Equal(2, section.Count);
            Assert.Equal("Jeevan", section["Player1"]);
            Assert.Equal("Merina", section["Player2"]);
        }

        [Fact]
        public void Save_tests()
        {
            var ini = new IniFile();

            var section = new IniFile.Section("Test") {
                { "Player1", "Jeevan" },
                { "Player2", "Merina" }
            };
            ini.Add(section);

            ini.Add(new IniFile.Section("Jeevan") {
                { "Powers", "Superspeed,Super strength" },
                { "Costume", "Scarlet" }
            });

            ini.Add(new IniFile.Section("Merina") {
                { "Powers", "Stretchability, Invisibility" },
                { "Costume", "Blue" }
            });

        }
    }
}