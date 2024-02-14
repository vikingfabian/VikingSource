using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Display3D
{
    class UnitHoverGui : AbsDeleteable
    {
        const float GuiHeight = 0.12f;
        public const float IconHeight = GuiHeight * 1.5f;

        const float halfSz = GuiHeight * 0.5f;

        AbsUnit unit;
        UnitStatusGuiSettings sett;
        Graphics.ImageGroupParent3D images = new Graphics.ImageGroupParent3D(16);

        public UnitHoverGui(AbsUnit unit, UnitStatusGuiSettings sett)
        {
            this.unit = unit;
            this.sett = sett;
            unit.statusGui = this;

            refresh();
        }

        public void refresh()
        {
            images.DeleteAll();

            Vector3 bottomLeft = VectorExt.V3FromY(0.08f);
            float centerOffset = 0.42f - halfSz;

            bottomLeft.X -= centerOffset;
            bottomLeft.Z += centerOffset;

            Vector3 position = bottomLeft;

            if (sett.health)
            {
                valueBar(position, 
                    SpriteName.cmdUnitStatusGui_Health, 
                    SpriteName.cmdUnitStatusGui_HealthBar, 
                    unit.health);
                nextRow(ref position);
            }
            if (sett.stamina)
            {
                HeroQuest.Unit hu = unit.hq();
                if (hu != null && hu.data.hero != null)
                {
                    valueBar(position, 
                        SpriteName.cmdUnitStatusGui_Stamina, 
                        SpriteName.cmdUnitStatusGui_StaminaBar, 
                        hu.data.hero.stamina);
                    nextRow(ref position);
                }
            }
            if (sett.statusEffects)
            {
                statusEffectIcons(position);
            }
            UpdatePosition();
        }

        void nextRow(ref Vector3 pos)
        {
            pos += toggLib.UpVector * (GuiHeight * 1.2f);
        }

        void statusEffectIcons(Vector3 position)
        {
            var icons = unit.hq().unitHoverStatusIcons();
            foreach (var icon in icons)
            {
                Graphics.Mesh img = new Graphics.Mesh(LoadedMesh.plane, position,
                    new Vector3(IconHeight), Graphics.TextureEffectType.Flat,
                    icon, Color.White);
                img.Rotation = toggLib.PlaneTowardsCam;

                images.Add(img);
                position.X += GuiHeight;
            }
        }

        void valueBar(Vector3 position, SpriteName icon, SpriteName barSprite, ValueBar value)
        {
            IntVector2 barSpriteSz = new IntVector2(6, 11);

            Graphics.Mesh heartIcon = new Graphics.Mesh(LoadedMesh.plane, position,
                new Vector3(GuiHeight * 0.8f), Graphics.TextureEffectType.Flat,
                icon, Color.White);
            heartIcon.Rotation = toggLib.PlaneTowardsCam;

            images.Add(heartIcon);

            position.X += GuiHeight * 0.6f;
            float h = GuiHeight * 0.5f;
            Vector3 barSz = VectorExt.V3FromXZ(barSpriteSz.SimilarVectorFromHeight(h), 0);//new Vector3(h * 0.5f, 0, h);
            float barStep = barSz.X * 1.1f;

            for (int i = 0; i < value.maxValue; ++i)
            {
                Graphics.Mesh bar = new Graphics.Mesh(LoadedMesh.plane, position, barSz,
                    Graphics.TextureEffectType.Flat, 
                    i < value.Value ? barSprite : SpriteName.cmdUnitStatusGui_EmptyBar,
                    Color.White);
                bar.Rotation = toggLib.PlaneTowardsCam;
                images.Add(bar);
                position.X += barStep;
            }
        }

        public void UpdatePosition()
        {
            images.ParentPosition = unit.soldierModel.GetCharacterPosition();
        }

        public override void DeleteMe()
        {
            base.DeleteMe();

            if (unit.statusGui == this)
            {
                unit.statusGui = null;
            }
            images.DeleteAll();
        }
    }

    struct UnitStatusGuiSettings
    {
        public static readonly UnitStatusGuiSettings MouseHover = new UnitStatusGuiSettings(true, false, true);
        public static readonly UnitStatusGuiSettings HealDamage = new UnitStatusGuiSettings(true, false, true);

        public bool health;
        public bool stamina;
        public bool statusEffects;

        public UnitStatusGuiSettings(bool health, bool stamina, bool statusEffects)
        {
            this.health = health;
            this.stamina = stamina;
            this.statusEffects = statusEffects;
        }
    }
}
