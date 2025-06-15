using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Graphics
{
    class DrawBatchCollection
    {
        
        Dictionary<int, DrawBatch> batches = new Dictionary<int, DrawBatch>(128);

        public void Add(int masterId, AbsDraw model)
        {
           
            DrawBatch batch;
            if (batches.TryGetValue(masterId, out batch))
            {
                batch.Add(model);
            }
            else
            {
                batches.Add(masterId, new DrawBatch(model));
            }

            model.OnDrawBatchAdd();//SetInRender(true);

        }

        public void PreRemove(int masterId, AbsDraw model)
        {
            model.SetInRender(false);
            if (batches.TryGetValue(masterId, out var batch))
            {
                batch.preremoved++;
            }
        }

        public void RemoveAndDraw(int cameraIndex)
        {
            Span<int> removeStack = stackalloc int[16]; // Use stackalloc for fast temporary storage
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
                            //else
                            //{
                            //    throw new Exception();
                            //}
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
