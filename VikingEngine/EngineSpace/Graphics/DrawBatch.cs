using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.Graphics.DrawProcess;

namespace VikingEngine.Graphics
{
    class DrawBatchCollection
    {
        Queue<AbsVoxelModelInstance> loadingQueue = new Queue<AbsVoxelModelInstance>();
        Dictionary<int, DrawBatch> batches = new Dictionary<int, DrawBatch>(128);


        public void Add(AbsVoxelModelInstance instance)
        {
            Debug.CrashIfThreaded();

            if (instance.master == null)
            {
                instance.OnDrawBatchAdd();
                loadingQueue.Enqueue(instance);
            }
            else
            {
                Add(instance.master.modelIndex, instance);
            }
        }

        public void Add(int masterId, AbsDraw model)
        {
#if DEBUG
            Debug.CrashIfThreaded();
            //model.InDrawBatchCount++;
            //if (model.InDrawBatchCount != 1)
            //{
            //    lib.DoNothing();
            //}
#endif

            DrawBatch batch;
            if (batches.TryGetValue(masterId, out batch))
            {
//#if DEBUG
//                if (batch.Contains(model))
//                {
//                    throw new Exception();
//                }
//#endif
                batch.Add(model);
            }
            else
            {
                batches.Add(masterId, new DrawBatch(model));
            }

            model.OnDrawBatchAdd();
        }

        //public void PreRemove(int masterId, AbsDraw model)
        //{
        //    model.SetInRender(false);
        //    if (batches.TryGetValue(masterId, out var batch))
        //    {
        //        batch.preremoved++;
        //    }
        //}
        public void DrawDepth(int cameraIndex, LightProjection light, Effect shader)
        {
            foreach (var kv in batches)
            {
                var list = kv.Value;

                foreach (var model in list)
                {
                    (model as Abs3DModel)?.DrawDepthOnly(shader, light, cameraIndex);
                }
            }
        }
        public void RemoveAndDraw(int cameraIndex)
        {
            while (loadingQueue.TryPeek(out var model)
                && model.master != null)
            {
                if (model.InRenderList)
                {
                    Add(model.master.modelIndex, loadingQueue.Dequeue());
                }
                else
                {
                    loadingQueue.Dequeue().OnDrawBatchRemove();
                }
            }

            Span<int> removeStack = stackalloc int[16];
            int removeCount = 0;

            foreach (var kv in batches)
            {
                var list = kv.Value;

                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var model = list[i];
                    if (model.InRenderList)
                    {
                        model.Draw(cameraIndex);
                    }
                    else
                    {
                        if (list.Count <= 1)
                        {
                            if (removeCount < removeStack.Length)
                            {
                                removeStack[removeCount++] = kv.Key;
                            }
                            else
                            {
                                list.Clear();
                            }
                        }
                        else
                        {
                            list.RemoveAt(i);
                        }

                        model.OnDrawBatchRemove();
                    }
                }
            }

            for (int i = 0; i < removeCount; i++)
            {
                batches.Remove(removeStack[i]);
            }
        }

        public void Remove(int masterId, AbsDraw model)
        {
            Debug.CrashIfThreaded();
            DrawBatch batch;
            if (batches.TryGetValue(masterId, out batch))
            {
                if (batch.Count <= 1)
                {
                    batches.Remove(masterId);
                }
                else
                {
                    batch.Remove(model);
                }
            }
        }
    }

    class DrawBatch : List<AbsDraw>
    {
        public int preremoved = 0;
        public DrawBatch(AbsDraw model)
            :base(16)
        {
            Add(model);
        }
    }

}
