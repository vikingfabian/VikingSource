// using System;
// using System.Collections.Generic;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using VikingEngine.Engine;
// using VikingEngine.Graphics;
// using Microsoft.Xna.Framework.Media;

// namespace VikingEngine.Graphics
// {
//     class VideoImage : ImageAdvanced
//     {
//         Video video;
//         VideoPlayer videoPlayer;


//         public VideoImage(Video video, Vector2 pos, Vector2 sz, ImageLayers layer, bool centerMidpoint)
//             :base(SpriteName.NO_IMAGE, pos, sz, layer, centerMidpoint, true)
//         {
//             this.video = video;
//             videoPlayer = new VideoPlayer();
//             ImageSource = new Rectangle(0, 0, video.Width, video.Height);
//         }

//         public void Play()
//         {
//             MediaPlayer.Stop();
//             videoPlayer.Play(video);
//         }
//         public void Pause()
//         {
//             if (videoPlayer.State == MediaState.Playing)
//                 videoPlayer.Pause();
//             else if (videoPlayer.State == MediaState.Paused)
//                 videoPlayer.Resume();
//         }

//         public override void Draw(int cameraIndex)
//         {
//             if (videoPlayer.State != MediaState.Stopped)
//             {
//                 Texture = videoPlayer.GetTexture();
//                 base.Draw(cameraIndex);
//             }
//         }

//         public MediaState MediaState
//         {
//             get { return videoPlayer.State; }
//         }

//     }
// }
