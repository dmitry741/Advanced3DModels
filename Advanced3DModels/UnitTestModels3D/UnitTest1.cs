using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models3DLib;
using System.Linq;
using System.Numerics;
using System.Drawing;

namespace UnitTestModels3D
{
    [TestClass]
    public class UnitTest1
    {
        private IPoint3D ResolvePoint3D(float X, float Y, float Z)
        {
            return new Point3D(X, Y, Z);
        }

        [TestMethod]
        [Description("Проверяем корректность нахождения вектора нормали к треугольнику.")]
        public void TestCross()
        {
            IPoint3D point1 = ResolvePoint3D(0, 0, 0);
            IPoint3D point2 = ResolvePoint3D(1, 0, 0);
            IPoint3D point3 = ResolvePoint3D(0, 2, 0);

            Triangle triangle = new Triangle(point1, point2, point3);
            Vector3 v = triangle.Normal;

            Assert.IsTrue(v.X == 0 && v.Y == 0 && v.Z == 2);
        }

        [TestMethod]
        [Description("Проверем корректность создания триангулированной четырехугольной поверхности. Квадрат.")]
        public void TestPolgon4Plane1()
        {
            IPoint3D point1 = ResolvePoint3D(0, 2, 0);
            IPoint3D point2 = ResolvePoint3D(2, 2, 0);
            IPoint3D point3 = ResolvePoint3D(2, 0, 0);
            IPoint3D point4 = ResolvePoint3D(0, 0, 0);

            Models3DLib.Plane plane = new Polygon4Plane(point1, point2, point3, point4, 1.0f, Color.Black, string.Empty);

            Assert.IsTrue(plane.Triangles.Count() == 8);
            Assert.IsTrue(plane.Points.Count() == 9);
            Assert.IsTrue(plane.Triangles.All(x => x.Normal.Z > 0));
        }

        [TestMethod]
        [Description("Проверем корректность создания триангулированной четырехугольной поверхности. Прямоугольник длина > высоты.")]
        public void TestPolgon4Plane2()
        {
            IPoint3D point1 = ResolvePoint3D(0, 2, 0);
            IPoint3D point2 = ResolvePoint3D(3, 2, 0);
            IPoint3D point3 = ResolvePoint3D(3, 0, 0);
            IPoint3D point4 = ResolvePoint3D(0, 0, 0);

            Models3DLib.Plane plane = new Polygon4Plane(point1, point2, point3, point4, 1.0f, Color.Black, string.Empty);

            Assert.IsTrue(plane.Triangles.Count() == 12);
            Assert.IsTrue(plane.Points.Count() == 12);
            Assert.IsTrue(plane.Triangles.All(x => x.Normal.Z > 0));
        }

        [TestMethod]
        [Description("Проверем корректность создания триангулированной четырехугольной поверхности. Прямоугольник длина < высоты.")]
        public void TestPolgon4Plane3()
        {
            IPoint3D point1 = ResolvePoint3D(0, 3, 0);
            IPoint3D point2 = ResolvePoint3D(2, 3, 0);
            IPoint3D point3 = ResolvePoint3D(2, 0, 0);
            IPoint3D point4 = ResolvePoint3D(0, 0, 0);

            Models3DLib.Plane plane = new Polygon4Plane(point1, point2, point3, point4, 1.0f, Color.Black, string.Empty);

            Assert.IsTrue(plane.Triangles.Count() == 12);
            Assert.IsTrue(plane.Points.Count() == 12);
            Assert.IsTrue(plane.Triangles.All(x => x.Normal.Z > 0));
        }
    }
}
