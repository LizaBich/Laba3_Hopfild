using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laba3_Hopfild.Core
{
    internal sealed class Weights
    {
        private int _countOfNeurons;
        private DbProcessor db;

        public Weights(int countOfNeurons)
        {
            this._countOfNeurons = countOfNeurons;
            this.db = new DbProcessor();
            this.Initialize();
        }

        public float this[int x, int y]
        {
            get
            {
                return this.db.Weights.FirstOrDefault(item => item.Index == (y * this._countOfNeurons + x)).Value;
            }
        }

        public void Learning(IList<byte[,]> images)
        {
            // this.Clear();
            this.Initialize();

            Task.Run(() => {
                try
                {
                    for (var count = 0; count < 1; ++count)
                    {
                        foreach (var image in images)
                        {
                            var imageArray = this.ConvertToSingleRankArray(image);
                            for (var j = 0; j < imageArray.Length; ++j)
                            {
                                for (var i = j; i < imageArray.Length; ++i)
                                {
                                    var val = this.db.Weights.FirstOrDefault(item => item.Index == (j * this._countOfNeurons + i));
                                    if (j == i)
                                    {
                                        val.Value = 0;
                                    }
                                    else
                                    {
                                        var temp = val.Value + imageArray[i] * imageArray[j];
                                        val.Value = temp;
                                    }
                                }
                            }
                        }
                    }

                    this.db.SaveChanges();
                }
                catch (Exception)
                {
                    this.db.Dispose();
                    throw;
                }
            }).Wait();
            
            this.Transponent();
        }

        private byte[] ConvertToSingleRankArray(byte[,] source)
        {
            var result = new byte[source.Length];
            var maxI = source.GetLength(1);

            for (var j = 0; j < source.GetLength(0); ++j)
            {
                for (var i = 0; i < source.GetLength(1); ++i)
                {
                    result[j * maxI + i] = source[j, i];
                }
            }

            return result;
        }

        private void Transponent()
        {
            Task.Run(() =>
            {
                try
                {
                    for (var j = 0; j < this._countOfNeurons; ++j)
                    {
                        for (var i = j; i < this._countOfNeurons; ++i)
                        {
                            var i1 = j * this._countOfNeurons + i;
                            var i2 = i * this._countOfNeurons + j;
                            var val1 = this.db.Weights.FirstOrDefault(item => item.Index == (j * this._countOfNeurons + i));
                            var val2 = this.db.Weights.FirstOrDefault(item => item.Index == (i * this._countOfNeurons + j));
                            val1.Value = val2.Value;
                        }
                    }

                    this.db.SaveChanges();
                }
                catch (Exception)
                {
                    this.db.Dispose();
                    throw;
                }
            }).Wait();
        }

        private void Initialize()
        {
            Task.Run(() => {
                try
                {
                    for (var i = 0; i < this._countOfNeurons; ++i)
                    {
                        for (var j = 0; j < this._countOfNeurons; ++j)
                        {
                            var index = j * this._countOfNeurons + i;
                            if (this.db.Weights.Any(item => item.Index == index))
                            {
                                this.db.Weights.First(item => item.Index == index).Value = 0.0f;
                            }
                            else
                            {
                                this.db.Weights.Add(new WeightModel()
                                {
                                    Index = index,
                                    Value = 0.0f
                                });
                            }
                        }
                    }

                    this.db.SaveChanges();
                }
                catch (Exception)
                {
                    this.db.Dispose();
                    throw;
                }
            }).Wait();
        }
    }
}
