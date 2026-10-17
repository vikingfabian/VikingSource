using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.MapModels;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.WeaponAttack;

namespace VikingEngine.DSSWars.GameState.MapEditor
{
    class GeneratorMap
    {

        /// <summary>
        /// Max preview texture side. The ScaleUp pass makes the icon grid 16x larger,
        /// and a full-resolution texture exceeds Vulkan's allocation limits (VK_ERROR_OUT_OF_DEVICE_MEMORY).
        /// </summary>
        private const int MaxPreviewTextureSize = 4096;
        private int _iconSampleStep = 1;
        private bool _isTextureOwned = false;
        private readonly Graphics.ImageAdvanced _image;
        private Vector2 _textureSize;
        private float _zoom = 1;
        private readonly Vector2 _topLeft;
        public float Scale = 1;
        public FactionPixelTexture _texture;

        public GeneratorMap(Vector2 pos)
        {
            _topLeft = pos;
            _texture = new FactionPixelTexture(-1, false, FactionMapFilter.Terrain);
            _image = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE, pos, Vector2.One, ImageLayers.Lay8, false);
        }

        public void ResetPos()
        {
            _image.position = _topLeft;
        }

        public void UserInput(InputMap input, bool mouseOverHud)
        {
            if ( !mouseOverHud)
            {
                if (input.mousePan.IsDown)
                {
                    _image.position += Input.Mouse.MoveDistance + input.moveCursor.direction;
                }
                _image.position -= input.move.directionAndTime * 4f;

                _zoom = Bound.Set(_zoom + input.ZoomValue() * -0.014f * _zoom, 0.25f, 4f);



            }
            _image.size = _textureSize * _zoom * Scale;
        }

        public bool PointerToTilePos(Vector2 pointer, IntVector2 tileSize, out IntVector2 tilePos)
        {
            tilePos = IntVector2.Zero;
            var ar = _image.Area;
            if (ar.IntersectPoint(pointer))
            {
                tilePos.Vec = ar.PositionToPercent(pointer) * tileSize.Vec;
                return true;
            }
            return false;
        }

        public Vector2 TileToScreenPos(IntVector2 tilePos, IntVector2 tileSize)
        {
           return _image.Area.PercentToPosition(tilePos.Vec / tileSize.Vec);
        }

        public void Generate()
        {
            if (!_isTextureOwned)
            {
                _texture.texture = null;
            }
            _texture.initTexture();
            _isTextureOwned = true;
            _texture.filter = arraylib.HasMembers(DssRef.world.cities)? FactionMapFilter.FactionCols : FactionMapFilter.Terrain;
            _texture.refreshWorld();
           
            _image.Texture = _texture.texture;
            _image.SetFullTextureSource();
            _textureSize = new Vector2(_texture.texture.Width, _texture.texture.Height);
            Scale = 1;
            _image.size = _textureSize * _zoom * Scale;
            _image.Visible = true;
        }


        public void GenerateNodes(Map.Map2.NodeMap map)
        {
            if (_isTextureOwned && _texture.texture != map.texture)
            {
                Graphics.GpuResourceSync.Release(_texture.texture);
            }
            _texture.texture = map.texture;
            _isTextureOwned = false;
            _image.Texture = map.texture;
            _image.SetFullTextureSource();
            _textureSize = new Vector2(_texture.texture.Width, _texture.texture.Height);
            Scale = 4;
            _image.size = _textureSize * _zoom * Scale;
            _image.Visible = true;
        }

        public void GenerateIcon(Map.Map2.IconWorldData world)
        {
            IntVector2 gridSize = world.iconGrid.Size;
            int maxSide = Math.Max(gridSize.X, gridSize.Y);
            _iconSampleStep = Math.Max(1, (maxSide + MaxPreviewTextureSize - 1) / MaxPreviewTextureSize);
            IntVector2 texSize = new IntVector2(
                (gridSize.X + _iconSampleStep - 1) / _iconSampleStep,
                (gridSize.Y + _iconSampleStep - 1) / _iconSampleStep);

            var previousTexture = _isTextureOwned ? _texture.texture : null;
            _texture.texture = new Graphics.PixelTexture(texSize);
            _isTextureOwned = true;
            Graphics.GpuResourceSync.Release(previousTexture);

            RefreshTexture(world);

            _image.Texture = _texture.texture;
            _image.SetFullTextureSource();
            // Use the logical grid size so on-screen size and tile picking are unchanged
            _textureSize = gridSize.Vec;
            Scale = 2;
            _image.size = _textureSize * _zoom * Scale;
            _image.Visible = true;
        }

        public void RefreshTexture(Map.Map2.IconWorldData world)
        {
            var texture = _texture.texture;
            var grid = world.iconGrid;
            int step = _iconSampleStep;

            for (int y = 0; y < texture.Height; y++)
            {
                int gy = Math.Min(y * step, grid.Size.Y - 1);
                for (int x = 0; x < texture.Width; x++)
                {
                    int gx = Math.Min(x * step, grid.Size.X - 1);
                    texture.SetPixel(x, y, grid.Get(gx, gy).color);
                }
            }

            texture.ApplyPixelsToTexture();
        }

        public void Hide()
        {
            _image.Visible = false;
        }
    }
}
