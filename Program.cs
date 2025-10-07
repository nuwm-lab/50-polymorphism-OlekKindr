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
            // Демонстрація: створимо об'єкти і покажемо поліморфізм
            Triangle t = new Triangle((0, 0), (3, 0), (0, 4));
            Tetrahedron tet = new Tetrahedron((0, 0), (3, 0), (0, 4), (0, 0));

            // Покажемо явний виклик
            Console.WriteLine("Трикутник:");
            t.PrintCoordinates();
            Console.WriteLine($"Площа трикутника = {t.Area():F4}\n");

            Console.WriteLine("Тетраедр:");
            tet.PrintCoordinates();
            Console.WriteLine($"Об'єм тетраедра = {tet.Volume():F4}\n");

            // Демонстрація поліморфізму через базовий абстрактний клас Shape
            Shape[] shapes = new Shape[] { t, tet };
            Console.WriteLine("-- Поліморфний виклик Measure() на масиві Shape --");
            foreach (var s in shapes)
            {
                s.Describe();
                Console.WriteLine($"Measure = {s.Measure():F4}\n");
            }
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

    // Абстрактний базовий клас Shape для демонстрації поліморфізму
    public abstract class Shape
    {
        // Описати фігуру
        public abstract void Describe();

        // Загальний метод для виміру: для Triangle повертатиме площу, для Tetrahedron — об'єм
        public abstract double Measure();

        // Зручний синонім
        public double MeasureValue() => Measure();
    }

    // Клас "трикутник"
    public class Triangle : Shape
    {
    protected Point2D _a, _b, _c;

        public Triangle() { }

        public Triangle((double x, double y) a, (double x, double y) b, (double x, double y) c)
        {
            SetCoordinates(a, b, c);
        }

        // Встановлення координат вершин
        public virtual void SetCoordinates((double x, double y) a, (double x, double y) b, (double x, double y) c)
        {
            _a = new Point2D(a.x, a.y);
            _b = new Point2D(b.x, b.y);
            _c = new Point2D(c.x, c.y);
        }

        // Виведення координат
        public virtual void PrintCoordinates()
        {
            Console.WriteLine($"A = {_a}");
            Console.WriteLine($"B = {_b}");
            Console.WriteLine($"C = {_c}");
        }

        // Обчислення площі трикутника
        public virtual double Area()
        {
            double x1 = _a.X, y1 = _a.Y;
            double x2 = _b.X, y2 = _b.Y;
            double x3 = _c.X, y3 = _c.Y;
            double area = 0.5 * Math.Abs(x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));
            return area;
        }

        // Реалізація Shape
        public override void Describe()
        {
            Console.WriteLine("Shape: Triangle");
            PrintCoordinates();
        }

        public override double Measure() => Area();
    }

    // Похідний клас "тетраедр"
    public class Tetrahedron : Triangle
    {
        private Point2D _d;

        public Tetrahedron() { }

        public Tetrahedron((double x, double y) a, (double x, double y) b, (double x, double y) c, (double x, double y) d)
            : base(a, b, c)
        {
            _d = new Point2D(d.x, d.y);
        }

        // Перевантаження: встановлення координат для 4 точок
        public void SetCoordinates((double x, double y) a, (double x, double y) b, (double x, double y) c, (double x, double y) d)
        {
            base.SetCoordinates(a, b, c);
            _d = new Point2D(d.x, d.y);
        }

        // Виведення координат (перевизначено)
        public override void PrintCoordinates()
        {
            base.PrintCoordinates();
            Console.WriteLine($"D = {_d}");
        }

        // Обчислення об'єму тетраедра
        public double Volume()
        {
            // Відобразимо точки як (x,y,0)
            double[,] p = new double[4, 3]
            {
                { _a.X, _a.Y, 0 },
                { _b.X, _b.Y, 0 },
                { _c.X, _c.Y, 0 },
                { _d.X, _d.Y, 0 }
            };

            double[] BA = { p[1,0] - p[0,0], p[1,1] - p[0,1], p[1,2] - p[0,2] };
            double[] CA = { p[2,0] - p[0,0], p[2,1] - p[0,1], p[2,2] - p[0,2] };
            double[] DA = { p[3,0] - p[0,0], p[3,1] - p[0,1], p[3,2] - p[0,2] };

            double det = BA[0] * (CA[1] * DA[2] - CA[2] * DA[1])
                       - BA[1] * (CA[0] * DA[2] - CA[2] * DA[0])
                       + BA[2] * (CA[0] * DA[1] - CA[1] * DA[0]);

            double volume = Math.Abs(det) / 6.0;
            return volume;
        }

        // Коли ми демонструємо Measure() з базового Shape, повертатимемо об'єм
        public override double Measure() => Volume();
    }
}
