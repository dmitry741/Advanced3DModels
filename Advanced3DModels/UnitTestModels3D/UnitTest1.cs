using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models3DLib;
using System.Linq;
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

        [TestMethod]
        [Description("Проверем корректность создания триангулированной четырехугольной поверхности. Квадрат.")]
        public void TestPolgon4Plane1()
        {
            Point3D point1 = new Point3D(0, 2, 0);
            Point3D point2 = new Point3D(2, 2, 0);
            Point3D point3 = new Point3D(2, 0, 0);
            Point3D point4 = new Point3D(0, 0, 0);

            AbstractPlane plane = new Polygon4Plane(point1, point2, point3, point4, 1.0f);

            Assert.IsTrue(plane.Triangles.Count() == 8);
            Assert.IsTrue(plane.Points.Count() == 9);
            Assert.IsTrue(plane.Triangles.All(x => x.Normal.Z > 0));
        }

        [TestMethod]
        [Description("Проверем корректность создания триангулированной четырехугольной поверхности. Прямоугольник длина > высоты.")]
        public void TestPolgon4Plane2()
        {
            Point3D point1 = new Point3D(0, 2, 0);
            Point3D point2 = new Point3D(3, 2, 0);
            Point3D point3 = new Point3D(3, 0, 0);
            Point3D point4 = new Point3D(0, 0, 0);

            AbstractPlane plane = new Polygon4Plane(point1, point2, point3, point4, 1.0f);

            Assert.IsTrue(plane.Triangles.Count() == 12);
            Assert.IsTrue(plane.Points.Count() == 12);
            Assert.IsTrue(plane.Triangles.All(x => x.Normal.Z > 0));
        }

        [TestMethod]
        [Description("Проверем корректность создания триангулированной четырехугольной поверхности. Прямоугольник длина < высоты.")]
        public void TestPolgon4Plane3()
        {
            Point3D point1 = new Point3D(0, 3, 0);
            Point3D point2 = new Point3D(2, 3, 0);
            Point3D point3 = new Point3D(2, 0, 0);
            Point3D point4 = new Point3D(0, 0, 0);

            AbstractPlane plane = new Polygon4Plane(point1, point2, point3, point4, 1.0f);

            Assert.IsTrue(plane.Triangles.Count() == 12);
            Assert.IsTrue(plane.Points.Count() == 12);
            Assert.IsTrue(plane.Triangles.All(x => x.Normal.Z > 0));
        }
    }
}
