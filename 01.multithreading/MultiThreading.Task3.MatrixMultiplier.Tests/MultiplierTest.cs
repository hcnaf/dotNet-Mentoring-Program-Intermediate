using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        private TestContext _testContextInstance;

        public TestContext TestContext
        {
            get { return _testContextInstance; }
            set { _testContextInstance = value; }
        }

        [TestMethod]
        public void MultiplyMatrix1On1Test()
        {
            TestMatrix1On1(new MatricesMultiplier());
            TestMatrix1On1(new MatricesMultiplierParallel());
        }

        [TestMethod]
        public void MultiplyMatrix2On2Test()
        {
            TestMatrix2On2(new MatricesMultiplier());
            TestMatrix2On2(new MatricesMultiplierParallel());
        }

        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());
        }

        [TestMethod]
        public void MultiplyMatrix7On7Test()
        {
            TestMatrix7On7(new MatricesMultiplier());
            TestMatrix7On7(new MatricesMultiplierParallel());
        }

        [TestMethod]
        public void MultiplyMatrix100On100Test()
        {
            TestMatrix100On100(new MatricesMultiplier());
            TestMatrix100On100(new MatricesMultiplierParallel());
        }

        [TestMethod]
        public void MultiplyMatrix10000On10000Test()
        {
            TestMatrix10000On10000(new MatricesMultiplier());
            TestMatrix10000On10000(new MatricesMultiplierParallel());
        }

        [TestMethod]
        public void ParallelEfficiencyTest()
        {
            var timeMatricesMultiplier1On1 = GetTimestampMethod(() => { TestMatrix1On1(new MatricesMultiplier()); });
            var timeMatricesMultiplierParallel1On1 = GetTimestampMethod(() => { TestMatrix1On1(new MatricesMultiplierParallel()); });

            TestContext.WriteLine("Time MatricesMultiplier1On1 {0}", timeMatricesMultiplier1On1);
            TestContext.WriteLine("Time MatricesMultiplierParallel1On1 {0}", timeMatricesMultiplierParallel1On1);
            TestContext.WriteLine("");

            var timeMatricesMultiplier2On2 = GetTimestampMethod(() => { TestMatrix2On2(new MatricesMultiplier()); });
            var timeMatricesMultiplierParallel2On2 = GetTimestampMethod(() => { TestMatrix2On2(new MatricesMultiplierParallel()); });

            TestContext.WriteLine("Time MatricesMultiplier2On2 {0}", timeMatricesMultiplier2On2);
            TestContext.WriteLine("Time MatricesMultiplierParallel2On2 {0}", timeMatricesMultiplierParallel2On2);
            TestContext.WriteLine("");

            var timeMatricesMultiplier3On3 = GetTimestampMethod(() => { TestMatrix3On3(new MatricesMultiplier()); });
            var timeMatricesMultiplierParallel3On3 = GetTimestampMethod(() => { TestMatrix3On3(new MatricesMultiplierParallel()); });

            TestContext.WriteLine("Time MatricesMultiplier3On3 {0}", timeMatricesMultiplier3On3);
            TestContext.WriteLine("Time MatricesMultiplierParallel3On3 {0}", timeMatricesMultiplierParallel3On3);
            TestContext.WriteLine("");

            var timeMatricesMultiplier7On7 = GetTimestampMethod(() => { TestMatrix7On7(new MatricesMultiplier()); });
            var timeMatricesMultiplierParallel7On7 = GetTimestampMethod(() => { TestMatrix7On7(new MatricesMultiplierParallel()); });

            TestContext.WriteLine("Time MatricesMultiplier7On7 {0}", timeMatricesMultiplier7On7);
            TestContext.WriteLine("Time MatricesMultiplierParallel7On7 {0}", timeMatricesMultiplierParallel7On7);
            TestContext.WriteLine("");

            var timeMatricesMultiplier100On100 = GetTimestampMethod(() => { TestMatrix100On100(new MatricesMultiplier()); });
            var timeMatricesMultiplierParallel100On100 = GetTimestampMethod(() => { TestMatrix100On100(new MatricesMultiplierParallel()); });

            TestContext.WriteLine("Time MatricesMultiplier100On100 {0}", timeMatricesMultiplier100On100);
            TestContext.WriteLine("Time MatricesMultiplierParallel100On100 {0}", timeMatricesMultiplierParallel100On100);
            TestContext.WriteLine("");

            var timeMatricesMultiplier10000On10000 = GetTimestampMethod(() => { TestMatrix10000On10000(new MatricesMultiplier()); });
            var timeMatricesMultiplierParallel10000On10000 = GetTimestampMethod(() => { TestMatrix10000On10000(new MatricesMultiplierParallel()); });

            TestContext.WriteLine("Time MatricesMultiplier10000On10000 {0}", timeMatricesMultiplier10000On10000);
            TestContext.WriteLine("Time MatricesMultiplierParallel10000On10000 {0}", timeMatricesMultiplierParallel10000On10000);
            TestContext.WriteLine("");

        }

        #region private methods
        void TestMatrix1On1(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(1, 1, randomInit: false);
            m1.SetElement(0, 0, 34);

            var m2 = new Matrix(1, 1, randomInit: false);
            m2.SetElement(0, 0, 12);

            var multiplied = matrixMultiplier.Multiply(m1, m2);

            Assert.AreEqual(408, multiplied.GetElement(0, 0));
        }

        void TestMatrix2On2(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(2, 2, randomInit: false);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);

            var m2 = new Matrix(2, 2, randomInit: false);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);

            var multiplied = matrixMultiplier.Multiply(m1, m2);

            Assert.AreEqual(418, multiplied.GetElement(0, 0));
            Assert.AreEqual(1778, multiplied.GetElement(0, 1));

            Assert.AreEqual(80, multiplied.GetElement(1, 0));
            Assert.AreEqual(280, multiplied.GetElement(1, 1));
        }

        void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        void TestMatrix7On7(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(7, 7, randomInit: true);
            var m2 = new Matrix(7, 7, randomInit: true);

            Assert.IsNotNull(m1);
            Assert.IsNotNull(m2);
        }

        void TestMatrix100On100(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(100, 100, randomInit: true);
            var m2 = new Matrix(100, 100, randomInit: true);

            Assert.IsNotNull(m1);
            Assert.IsNotNull(m2);
        }

        void TestMatrix10000On10000(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(10000, 10000, randomInit: true);
            var m2 = new Matrix(10000, 10000, randomInit: true);

            Assert.IsNotNull(m1);
            Assert.IsNotNull(m2);
        }

        double GetTimestampMethod(Action method)
        {
            var startingTime = Stopwatch.GetTimestamp();
            method();
            var endingTime = Stopwatch.GetTimestamp();
            var elapsedSeconds = (endingTime - startingTime) * (1.0 / Stopwatch.Frequency);

            return elapsedSeconds;
        }

        #endregion
    }
}
