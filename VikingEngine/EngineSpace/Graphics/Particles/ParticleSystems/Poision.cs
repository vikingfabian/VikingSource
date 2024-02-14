#region File Description
//-----------------------------------------------------------------------------
// SmokePlumeParticleSystem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace VikingEngine.Graphics
{
    class Poision : Smoke
    {
        public Poision()
            : base()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            base.InitializeSettings(settings);

            settings.MinColor = new Color(23, 172, 3, 120);
            settings.MaxColor = new Color(116, 255, 32, 180);
            settings.BlendState = BlendState.Additive;
        }
    }
}
