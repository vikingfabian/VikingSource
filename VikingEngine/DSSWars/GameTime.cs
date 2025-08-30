using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Data;

namespace VikingEngine.DSSWars
{
    class GameTime
    {
        const float Quarter = 0.25f;

        public bool oneSecond = false;
        public bool oneMinute = false;
        public bool oneSecond_part2 = false;
        public bool halfSecond = false;
        float second = 0;
        int quarter = 0;
        int secondsToMinute = 0;
        public int totalMinutes = 0;

        float asyncGameObjects_Seconds = 0;
        float asyncWork_Seconds = 0;

        const float DayLight_Min = 0.7f;
        const float DayLight_Add = 0.2f;

        const float DayLight_Terrain_Min = 0.9f;
        const float DayLight_Terrain_Add = 0.2f;


        public Vector3 ShaderDayLight_Objects = new Vector3(0.3f);
        public Vector4 ShaderDayLight_Map = Vector4.One;
        public float ShaderDayLight_RedTint = 0f;

        public Vector3 shadow_LightDirection = new Vector3(-0.2f, -1f, -0.2f);
        public Vector3 shadow_SunColor = new Vector3(0.5f, 0.45f, 0.45f);

        public GameTime()
        {
            DssRef.time = this;
            SetFullDaylightShader();
        }

        public void update()
        {
            second += Ref.DeltaGameTimeSec;
            oneSecond = false;
            oneSecond_part2 = false;
            halfSecond = false;
            
            if (second >= Quarter)
            {
                second -= Quarter;
                ++quarter;
                if (quarter >= 4)
                { 
                    quarter = 0;
                }

                switch (quarter)
                {
                    case 0:
                        oneSecond = true;

                        float diff = Math.Abs(secondsToMinute - 30) / 30f;
                        float light = 1f - diff;

                        if (Ref.gamesett.modelShadow)
                        {
                            shadow_LightDirection = new Vector3(-(0.2f + 0.15f * diff), -(1f - 0.15f * diff), -(0.2f + 0.15f * diff));
                            shadow_SunColor = new Vector3(0.4f + diff * 0.12f, 0.45f, 0.45f) * (0.7f + light);
                        }
                        else if (Ref.gamesett.ModelLightShaderEffect)
                        {
                            ShaderDayLight_Objects = new Vector3(DayLight_Min + DayLight_Add * light);
                            ShaderDayLight_Map = new Vector4(DayLight_Terrain_Min + DayLight_Terrain_Add * light);
                            ShaderDayLight_Map.W = 1f;
                            ShaderDayLight_RedTint = diff * 0.12f;
                        }
                        else
                        {
                            ShaderDayLight_Map = Vector4.One;
                        }
                        break;
                    case 1: halfSecond = true; break;
                    case 2:
                        oneMinute = false;
                        oneSecond_part2 = true;
                        asyncGameObjects_Seconds += 1f;
                        asyncWork_Seconds  += 1f;
                        if (++secondsToMinute >= 60)
                        {
                            secondsToMinute = 0;
                            ++totalMinutes;
                            oneMinute = true;
                            DssRef.state.OneMinute_Update();
                        }
                        break;
                    case 3: halfSecond = true; break;
                }                
            }
        }

        public float pullAsyncGameObjects_Seconds()
        {
            float result = asyncGameObjects_Seconds;
            asyncGameObjects_Seconds -= result;
            return result;
        }

        public float pullAsyncWork_Seconds()
        {
            float result = asyncWork_Seconds;
            asyncWork_Seconds -= result;
            return result;
        }

        public bool pullMinute(ref int totalMinutes)
        {
            if (this.totalMinutes > totalMinutes)
            { 
                totalMinutes = this.totalMinutes;
                return true;
            }

            return false;
        }

        public TimeSpan TotalIngameTime()
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(totalMinutes);
            timeSpan = timeSpan.Add(TimeSpan.FromSeconds(secondsToMinute));

            return timeSpan;
        }

        public void setTotalTime(TimeSpan time)
        {
            totalMinutes = (int)time.TotalMinutes;
            secondsToMinute = time.Seconds;
        }

        public static void SetFullDaylightShader()
        {
            DssRef.time.ShaderDayLight_Objects = new Vector3(DayLight_Min + DayLight_Add);
            DssRef.time.ShaderDayLight_Map = new Vector4(DayLight_Terrain_Min + DayLight_Terrain_Add);
            DssRef.time.ShaderDayLight_Map.W = 1f;
            DssRef.time.ShaderDayLight_RedTint = 0f;
        }
        //public void writeGameState(System.IO.BinaryWriter w)
        //{

        //}
        //public void readGameState(System.IO.BinaryReader r, int subversion, ObjectPointerCollection pointers)
        //{

        //}
    }
}
