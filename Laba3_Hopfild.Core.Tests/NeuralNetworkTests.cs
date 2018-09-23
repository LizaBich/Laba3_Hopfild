﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Laba3_Hopfild.Core.Tests
{
    [TestClass]
    public class NeuralNetworkTests
    {
        private bool[,] imageA =
        {
            { false, false, false, false, false, false, false, false, false, false },
            { false, false, false, false, true,  true,  false, false, false, false },
            { false, false, false, true,  true,  true,  true,  false, false, false },
            { false, false, true,  true,  false, false, true,  true,  false, false },
            { false, false, true,  true,  false, false, true,  true,  false, false },
            { false, false, true,  true,  true,  true,  true,  true,  false, false },
            { false, false, true,  true,  true,  true,  true,  true,  false, false },
            { false, false, true,  true,  false, false, true,  true,  false, false },
            { false, false, true,  true,  false, false, true,  true,  false, false },
            { false, false, false, false, false, false, false, false, false, false }
        };

        private bool[,] imageB =
        {
            { false, false, false, false, false, false, false, false, false, false },
            { false, false, true,  true,  true,  true,  false, false, false, false },
            { false, false, true,  true,  false, true,  true,  false, false, false },
            { false, false, true,  true,  false, true,  true,  false, false, false },
            { false, false, true,  true,  true,  true,  false, false, false, false },
            { false, false, true,  true,  false, true,  true,  false, false, false },
            { false, false, true,  true,  false, false, true,  true,  false, false },
            { false, false, true,  true,  false, true,  true,  true,  false, false },
            { false, false, true,  true,  true,  true,  true,  false, false, false },
            { false, false, false, false, false, false, false, false, false, false }
        };

        private bool[,] imageY =
        {
            { false, false, false, false, false, false, false, false, false, false },
            { false, false, true,  true,  false, false, true,  true,  false, false },
            { false, false, true,  true,  false, false, true,  true,  false, false },
            { false, false, true,  true,  false, false, true,  true,  false, false },
            { false, false, false, true,  true,  true,  true,  false, false, false },
            { false, false, false, false, true,  true,  false, false, false, false },
            { false, false, false, false, true,  true,  false, false, false, false },
            { false, false, false, false, true,  true,  false, false, false, false },
            { false, false, false, false, true,  true,  false, false, false, false },
            { false, false, false, false, false, false, false, false, false, false }
        };

        private bool[,] imageH =
        {
            { false, false, false, false, false, false, false, false, false, false },
            { false, false, true,  true,  false, false, true,  true,  false, false },
            { false, false, true,  true,  false, false, true,  true,  false, false },
            { false, false, true,  true,  false, false, true,  true,  false, false },
            { false, false, true,  true,  true,  true,  true,  true,  false, false },
            { false, false, true,  true,  true,  true,  true,  true,  false, false },
            { false, false, true,  true,  false, false, true,  true,  false, false },
            { false, false, true,  true,  false, false, true,  true,  false, false },
            { false, false, true,  true,  false, false, true,  true,  false, false },
            { false, false, false, false, false, false, false, false, false, false }
        };

        private bool[,] imageANoise =
        {
            { false, false, false, false, false, false, false, false, false, false },
            { false, false, false, true,  true,  true,  false, false, true,  false },
            { false, false, false, true,  true,  true,  true,  true,  false, false },
            { false, true,  true,  true,  false, true,  true,  true,  false, false },
            { false, false, true,  true,  true,  false, true,  true,  false, false },
            { false, false, true,  true,  true,  true,  true,  true,  true,  false },
            { false, false, true,  true,  true,  true,  true,  true,  true,  false },
            { false, true,  true,  true,  false, true,  true,  true,  false, false },
            { false, true,  true,  true,  false, false, true,  true,  false, false },
            { false, false, false, true,  false, false, false, true,  false, false }
        };

        private bool[,] imageBNoise =
        {
            { false, false, false, false, false, true,  false, false, false, false },
            { false, true,  true,  true,  true,  true,  false, false, false, false },
            { false, false, true,  true,  false, true,  true,  true,  false, false },
            { false, true,  true,  true,  false, true,  true,  false, false, false },
            { false, false, true,  true,  true,  true,  true,  false, false, false },
            { false, true,  true,  true,  false, true,  true,  false, false, false },
            { false, false, true,  true,  false, false, true,  true,  false, false },
            { false, true,  true,  true,  true,  true,  true,  true,  false, false },
            { false, false, true,  true,  true,  true,  true,  false, false, false },
            { false, false, true,  false, false, false, true,  false, false, false }
        };

        private bool[,] imageYNoise =
        {
            { false, false, false, false, false, false, false, false, false, false },
            { false, false, true,  true,  false, true,  true,  true,  false, false },
            { false, true,  true,  true,  false, false, true,  true,  false, false },
            { false, false, true,  true,  true,  false, true,  true,  true,  false },
            { false, false, true,  true,  true,  true,  true,  false, false, false },
            { false, false, false, true,  true,  true,  false, false, false, false },
            { false, false, false, false, true,  true,  true,  false, false, false },
            { false, false, false, false, true,  true,  false, false, true,  false },
            { false, false, false, true,  true,  true,  true,  true,  false, false },
            { false, false, false, false, true,  true,  false, false, false, false }
        };

        private IList<bool[,]> images;

        private NeuralNetwork _network;

        [TestInitialize]
        public void Initialize()
        {
            this._network = new NeuralNetwork(10);
            this.images = new List<bool[,]>()
            {
                this.imageA,
                this.imageB,
                this.imageY
            };
        }

        [TestMethod]
        public void TestForBImage()
        {
            this._network.PrepareNetwork(this.images);

            var result = this._network.Analyze(this.imageB, this.imageBNoise);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestForAImage()
        {
            this._network.PrepareNetwork(this.images);

            var result = this._network.Analyze(this.imageA, this.imageANoise);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestForYImage()
        {
            this._network.PrepareNetwork(this.images);

            var result = this._network.Analyze(this.imageY, this.imageYNoise);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestForHImage()
        {
            this._network.PrepareNetwork(this.images);

            var result = this._network.Analyze(this.imageH, this.imageBNoise);

            Assert.AreEqual(false, result);
        }

        private void ArrayEquals(bool[,] expected, bool[,] actual)
        {
            for (var j = 0; j < expected.GetLength(0); ++j)
            {
                for (var i = 0; i < expected.GetLength(1); ++i)
                {
                    Assert.AreEqual(expected[j, i], actual[j, i], $"Not equal in [{j},{i}]");
                }
            }
        }
    }
}
