using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models3DLib;
using System.Numerics;

namespace UnitTestModels3D
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [Description("Проверяем корректность нахождения вектора нормали к треугольнику.")]
        public void TestCross()
        {
            Point3D point1 = new Point3D(0, 0, 0);
            Point3D point2 = new Point3D(1, 0, 0);
            Point3D point3 = new Point3D(0, 2, 0);

            Triangle triangle = new Triangle(point1, point2, point3);
            Vector3 v = triangle.Normal;

            Assert.IsTrue(v.X == 0 && v.Y == 0 && v.Z == 2);
        }
    }
}
