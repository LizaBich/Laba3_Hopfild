using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laba3_Hopfild.Core
{
    internal sealed class Weights
    {
        private IDictionary<int, IDictionary<int, float>> weights;
        private int _countOfNeurons;

        public Weights(int countOfNeurons)
        {
            this._countOfNeurons = countOfNeurons;
            this.Initialize();
        }

        public float this[int x, int y]
        {
            get
            {
                return this.weights[y][x];
            }

            private set
            {
                this.weights[y][x] = value;
            }
        }

        public void Learning(IList<byte[,]> images)
        {
            this.Clear();
            this.Initialize();

            for (var count = 0; count < 1; ++count)
            {
                foreach (var image in images)
                {
                    var imageArray = this.ConvertToSingleRankArray(image);
                    for (var j = 0; j < imageArray.Length; ++j)
                    {
                        for (var i = j; i < imageArray.Length; ++i)
                        {
                            if (j == i)
                            {
                                this[i, j] = 0;
                            }
                            else
                            {
                                this[i, j] += imageArray[i] * imageArray[j];
                            }
                        }
                    }
                }
            }

            this.Transponent();
        }

        private void Clear()
        {
            foreach (var item in this.weights)
            {
                item.Value.Clear();
            }
            this.weights.Clear();
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
            for (var j = 0; j < this._countOfNeurons; ++j)
            {
                for (var i = j; i < this._countOfNeurons; ++i)
                {
                    this[j, i] = this[i, j];
                }
            }
        }

        private void Initialize()
        {
            this.weights = new Dictionary<int, IDictionary<int, float>>();
            for (var j = 0; j < this._countOfNeurons; ++j)
            {
                this.weights.Add(j, new Dictionary<int, float>());
                for (var i = 0; i < this._countOfNeurons; ++i)
                {
                    this.weights[j].Add(i, 0.0f);
                }
            }
        }
    }
}
