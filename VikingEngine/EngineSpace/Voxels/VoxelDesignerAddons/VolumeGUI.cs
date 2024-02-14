using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.Voxels
{
    class VolumeGUI
    {
        Graphics.Mesh pencilMultiSelection;

        public VolumeGUI()
        {
            pencilMultiSelection = new Graphics.Mesh(LoadedMesh.cube_repeating, Vector3.Zero, Vector3.One,
                TextureEffectType.Flat, SpriteName.EditorMultiSelection, Color.White);
               //new Graphics.TextureEffect(TextureEffectType.Flat, SpriteName.EditorMultiSelection), 1f);
            pencilMultiSelection.Color = Color.White;
            pencilMultiSelection.Visible = false;
        }

        public void refresh(IntervalIntV3 volume, Vector3 offset)
        {
            pencilMultiSelection.Visible = volume.Min  != volume.Max;

            if (pencilMultiSelection.Visible)
            {
                const float MultiSelAddRadius = 0.1f;
                pencilMultiSelection.Position = volume.Center + offset;
                pencilMultiSelection.Scale = (volume.Add.Vec + Vector3.One) + VectorExt.V3(MultiSelAddRadius * 2);
            }
        }

        public void hide()
        {
            pencilMultiSelection.Visible = false;
        }
        
    }
}