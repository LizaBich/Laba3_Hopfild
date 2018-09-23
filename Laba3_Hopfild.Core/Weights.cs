using System.Collections.Generic;

namespace Laba3_Hopfild.Core
{
    internal sealed class Weights
    {
        private IDictionary<int, IDictionary<int, float>> _weights;

        public Weights(int countOfNeurons)
        {
            this._weights = new Dictionary<int, IDictionary<int, float>>(countOfNeurons);
            for (var i = 0; i < countOfNeurons; ++i)
            {
                this._weights.Add(i, new Dictionary<int, float>());
                for (var j = 0; j < countOfNeurons; ++j)
                {
                    this._weights[i].Add(j, 0.0f);
                }
            }
        }

        public float this[int x, int y]
        {
            get
            {
                return this._weights[y][x];
            }
            private set
            {
                this._weights[y][x] = value;
            }
        }

        public void Learning(IList<byte[,]> images)
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
            for (var j = 0; j < this._weights.Count; ++j)
            {
                for (var i = j; i < this._weights[0].Count; ++i)
                {
                    this[j, i] = this[i, j];
                }
            }
        }
    }
}
