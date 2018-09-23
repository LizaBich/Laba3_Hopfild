using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba3_Hopfild.Core
{
    public class NeuralNetwork
    {
        private readonly Weights _weights;

        private readonly IDictionary<int, IDictionary<int, Neuron>> _neurons;

        public NeuralNetwork(int length)
        {
            this._neurons = new Dictionary<int, IDictionary<int, Neuron>>();
            this.InitializeNeurons(length);
            this._weights = new Weights(this.CountOfNeurons());
        }

        public void PrepareNetwork(IList<bool[,]> source)
        {
            IList<byte[,]> images = new List<byte[,]>
            {
                this.ConvertToBytes(source[0]),
                this.ConvertToBytes(source[1]),
                this.ConvertToBytes(source[2])
            };

            this._weights.Learning(images);
        }

        public bool Analyze(bool[,] origin, bool[,] forAnalyze)
        {
            var image = this.ProcessData(forAnalyze);
            var countOfHits = 0;

            for (var j = 0; j < origin.GetLength(0); ++j)
            {
                for (var i = 0; i < origin.GetLength(1); ++i)
                {
                    if (origin[j, i] == image[j, i] && origin[j, i] == true)
                    {
                        ++countOfHits;
                    }
                }
            }

            var result = (float)countOfHits / (float)this.CountOfTrue(origin);

            return result > 0.8f ? true : false;
        }

        private bool[,] ProcessData(bool[,] source)
        {
            this.ConvertToNeurons(source);

            this.InitializeNeurons(this._neurons[0].Count, out IDictionary<int, IDictionary<int, Neuron>> previous);

            var countY = this._neurons.Count;
            var countX = this._neurons[0].Count;
            byte count = 1;

            while (count != 3)
            {
                for (var j = 0; j < countY; ++j)
                {
                    for (var i = 0; i < countX; ++i)
                    {
                        float temp = 0.0f;
                        for (var y = 0; y < countY; ++y)
                        {
                            for (var x = 0; x < countX; ++x)
                            {
                                var xN = y * countX + x;
                                var yN = j * countX + i;
                                temp += this._weights[xN, yN] * this._neurons[y][x].State;
                            }
                        }
                        this._neurons[j][i].State = temp > 0 ? 1 : -1;
                    }
                }

                if (this.CompareNetworks(previous, this._neurons)) ++count;
                this.Copy(ref previous);
            }

            return this.ConvertToBool();
        }

        private int CountOfNeurons()
        {
            return this._neurons[0].Count * this._neurons.Count;
        }

        private void InitializeNeurons(int length)
        {
            for (var i = 0; i < length; ++i)
            {
                this._neurons.Add(i, new Dictionary<int, Neuron>());
                for (var j = 0; j < length; ++j)
                {
                    this._neurons[i].Add(j, new Neuron() { State = -1});
                }
            }
        }

        private void InitializeNeurons(int length, out IDictionary<int, IDictionary<int, Neuron>> source)
        {
            source = new Dictionary<int, IDictionary<int, Neuron>>();
            for (var i = 0; i < length; ++i)
            {
                source.Add(i, new Dictionary<int, Neuron>());
                for (var j = 0; j < length; ++j)
                {
                    source[i].Add(j, new Neuron() { State = -1 });
                }
            }
        }

        private void ConvertToNeurons(bool[,] source)
        {
            for (var y = 0; y < source.GetLength(0); ++y)
            {
                for (var x = 0; x < source.GetLength(1); ++x)
                {
                    this._neurons[y][x].State = source[y, x] ? 1 : -1;
                }
            }
        }

        private bool[,] ConvertToBool()
        {
            var xCount = this._neurons[0].Count;
            var yCount = this._neurons.Count;
            var result = new bool[yCount, xCount];

            for (var y = 0; y < yCount; ++y)
            {
                for (var x = 0; x < xCount; ++x)
                {
                    result[y, x] = this._neurons[y][x].State == 1 ? true : false;
                }
            }

            return result;
        }

        private bool CompareNetworks(IDictionary<int, IDictionary<int, Neuron>> previous, IDictionary<int, IDictionary<int, Neuron>> current)
        {
            for (var y = 0; y < previous.Count && y < current.Count; ++y)
            {
                for (var x = 0; x < previous[0].Count && x < current[0].Count; ++x)
                {
                    if (!previous[y][x].Equals(current[y][x]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void Copy(ref IDictionary<int, IDictionary<int, Neuron>> destination)
        {
            for (var y = 0; y < this._neurons.Count && y < destination.Count; ++y)
            {
                for (var x = 0; x < this._neurons[0].Count && x < destination[0].Count; ++x)
                {
                    destination[y][x].State = this._neurons[y][x].State;
                }
            }
        }

        private byte[,] ConvertToBytes(bool[,] source)
        {
            byte[,] result = new byte[source.GetLength(0), source.GetLength(1)];

            for (var j = 0; j < source.GetLength(0); ++j)
            {
                for (var i = 0; i < source.GetLength(1); ++i)
                {
                    result[j, i] = source[j, i] ? (byte)1 : (byte)0;
                }
            }

            return result;
        }

        private int CountOfTrue(bool[,] source)
        {
            int result = 0;

            for (var j = 0; j < source.GetLength(0); ++j)
            {
                for (var i = 0; i < source.GetLength(1); ++i)
                {
                    if (source[j, i]) ++result;
                }
            }

            return result;
        }
    }
}