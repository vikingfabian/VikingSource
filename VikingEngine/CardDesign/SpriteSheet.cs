using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.CardDesign
{
    class SpriteSheet : Engine.AbsSpriteSheetLayout
    {
        public const int CreatureCount = 129;
        public const int SpellCount = 10;

        public SpriteSheet()
        {
            this.Settings(4096, 128);
            this.TileSheetIx = LoadedTexture.CardTiles;

            add(SpriteName.CardBack, 2, 3);
            add(SpriteName.CardFront, 2, 3);

            //ICON
            currentIndex = numTilesWidth * 3;
            {
                add(SpriteName.CardIconHealth);
                add(SpriteName.CardIconShield);
                add(SpriteName.CardIconAttack);
                add(SpriteName.CardIconMana);
                add(SpriteName.CardIconCoin);
                add(SpriteName.CardIconVictoryPoint);
                add(SpriteName.CardIconStrength);

                add(SpriteName.CardIconManaGreen);
                add(SpriteName.CardIconManaWhite);
                add(SpriteName.CardIconManaRed);
                add(SpriteName.CardIconManaBlack);
                add(SpriteName.CardIconManaBlue);
                add(SpriteName.CardIconManaYellow);
                
                add(SpriteName.CardIconAttackDefence);
                add(SpriteName.CardIconDefence);

            }

            
            //CREATURE
            {
                int tileRow = 11;
                SpriteName currentCreature = SpriteName.CardCreatureImageStart;
                int[] creatureCounts = { 63, 34, 30 };
                foreach (var row in creatureCounts)
                {
                    currentIndex = numTilesWidth * tileRow;
                    for (int cellIx = 0; cellIx < row; ++cellIx)
                    {
                        add(currentCreature, 2, 2);
                        currentCreature++;
                    }
                    tileRow += 2;
                }

                currentIndex = numTilesWidth * tileRow;
                for (int cellIx = 0; cellIx < 2; ++cellIx)
                {
                    add(currentCreature, 3, 3);
                    currentCreature++;
                }
            }

            //SPELL
            currentIndex = numTilesWidth * 23;
            {
                SpriteName currentSpell = SpriteName.CardSpellImageStart;
                
                for (int cellIx = 0; cellIx < 10; ++cellIx)
                {
                    add(currentSpell, 2, 2);
                    currentSpell++;
                }
            }
        }
    }
}