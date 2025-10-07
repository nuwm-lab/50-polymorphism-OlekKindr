using System;

namespace LabWork
{
    // Даний проект є шаблоном для виконання лабораторних робіт
    // з курсу "Об'єктно-орієнтоване програмування та патерни проектування"
    // Необхідно змінювати і дописувати код лише в цьому проекті
    // Відео-інструкції щодо роботи з github можна переглянути 
    // за посиланням https://www.youtube.com/@ViktorZhukovskyy/videos 
    class Program
    {
        static void Main(string[] args)
        {
            // Демонстрація роботи класів
            Triangle t = new Triangle();
            // Встановимо координати вершин трикутника
            t.SetCoordinates((0, 0), (3, 0), (0, 4));
            Console.WriteLine("Трикутник:");
            t.PrintCoordinates();
            Console.WriteLine($"Площа трикутника = {t.Area():F4}\n");

            // Тетраедр визначимо як похідний клас від Triangle (четверта точка додається)
            Tetrahedron tet = new Tetrahedron();
            tet.SetCoordinates((0, 0), (3, 0), (0, 4), (0, 0)); // четверта точка на площині (задані в умові)
            Console.WriteLine("Тетраедр:");
            tet.PrintCoordinates();
            Console.WriteLine($"Об'єм тетраедра = {tet.Volume():F4}");
        }
    }

    // Клас для точки на площині
    public struct Point2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X}, {Y})";
    }

    // Клас "трикутник"
    public class Triangle
    {
        protected Point2D A, B, C;

        // Встановлення координат вершин
        public virtual void SetCoordinates((double x, double y) a, (double x, double y) b, (double x, double y) c)
        {
            A = new Point2D(a.x, a.y);
            B = new Point2D(b.x, b.y);
            C = new Point2D(c.x, c.y);
        }

        // Виведення координат
        public virtual void PrintCoordinates()
        {
            Console.WriteLine($"A = {A}");
            Console.WriteLine($"B = {B}");
            Console.WriteLine($"C = {C}");
        }

        // Обчислення площі трикутника (формула площі через координати / формула Шульца)
        public virtual double Area()
        {
            // Площа = 0.5 * | x1(y2 - y3) + x2(y3 - y1) + x3(y1 - y2) |
            double x1 = A.X, y1 = A.Y;
            double x2 = B.X, y2 = B.Y;
            double x3 = C.X, y3 = C.Y;
            double area = 0.5 * Math.Abs(x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));
            return area;
        }
    }

    // Похідний клас "тетраедр"
    public class Tetrahedron : Triangle
    {
        protected Point2D D;

        // Перевантажуємо метод встановлення координат для 4 точок
        public void SetCoordinates((double x, double y) a, (double x, double y) b, (double x, double y) c, (double x, double y) d)
        {
            base.SetCoordinates(a, b, c);
            D = new Point2D(d.x, d.y);
        }

        // Виведення координат (перевизначено)
        public override void PrintCoordinates()
        {
            base.PrintCoordinates();
            Console.WriteLine($"D = {D}");
        }

        // Обчислення об'єму тетраедра, якщо задані координати чотирьох вершин у 3D.
        // Але в умові сказано, що точки знаходяться на площині, тому об'єм = 0.
        // Щоб показати роботу, використаємо формулу об'єму для чотирьох точок у 3D,
        // піднявши кожну 2D точку у простір як (x,y,0). Об'єм тоді буде 0.
        public double Volume()
        {
            // Відобразимо точки як (x,y,0)
            double[,] p = new double[4, 3]
            {
                { A.X, A.Y, 0 },
                { B.X, B.Y, 0 },
                { C.X, C.Y, 0 },
                { D.X, D.Y, 0 }
            };

            // Об'єм = 1/6 * |det( B-A, C-A, D-A )|
            double[] BA = { p[1,0] - p[0,0], p[1,1] - p[0,1], p[1,2] - p[0,2] };
            double[] CA = { p[2,0] - p[0,0], p[2,1] - p[0,1], p[2,2] - p[0,2] };
            double[] DA = { p[3,0] - p[0,0], p[3,1] - p[0,1], p[3,2] - p[0,2] };

            double det = BA[0] * (CA[1] * DA[2] - CA[2] * DA[1])
                       - BA[1] * (CA[0] * DA[2] - CA[2] * DA[0])
                       + BA[2] * (CA[0] * DA[1] - CA[1] * DA[0]);

            double volume = Math.Abs(det) / 6.0;
            return volume;
        }
    }
}
