using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            for (var count = 0; count < 10; ++count)
            {
                foreach (var image in images)
                {
                    for (var j = 0; j < image.GetLength(0); ++j)
                    {
                        for (var i = 0; i < image.GetLength(1); ++i)
                        {
                            for (var y = 0; y < image.GetLength(0); ++y)
                            {
                                for (var x = 0; x < image.GetLength(1); ++x)
                                {
                                    var xN = y * image.GetLength(1) + x;
                                    var yN = j * image.GetLength(1) + i;
                                    if (j == y && i == x)
                                    {
                                        this[xN, yN] = 0;
                                    }
                                    else
                                    {
                                        this[xN, yN] += image[j, i] * image[y, x];
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
