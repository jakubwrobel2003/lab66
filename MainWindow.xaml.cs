using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace lab6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Towar> towary;

        public MainWindow()
        {
            InitializeComponent();
            towary = new List<Towar>
            {
                new Towar("Laptop", 3500, 10, Kategoria.Elektronika),
                new Towar("T-shirt", 80, 50, Kategoria.Odzież),
                new Towar("Chleb", 25, 200, Kategoria.Spożywcze),
                new Towar("Smartfon", 2000, 5, Kategoria.Elektronika),
                new Towar("Mikser", 450, 15, Kategoria.Elektronika),
                new Towar("Spodnie", 150, 25, Kategoria.Odzież),
                new Towar("Jabłka", 3, 120, Kategoria.Spożywcze),
                new Towar("Koszula", 120, 30, Kategoria.Odzież),
                new Towar("TV", 1500, 12, Kategoria.Elektronika),
                new Towar("Czekolada", 5, 100, Kategoria.Spożywcze)
            };
        }

        private void Funkcja1_Button_Click(object sender, RoutedEventArgs e)
        {
            double rosenbrock(double x, double y)
            {
                return Math.Pow(1 - x, 2) + 100 * Math.Pow(y - Math.Pow(x, 2), 2);
            }

            var wynik = matematyka.ZnajdzMinFun2D(-2, 2, -2, 2, 100000, rosenbrock);

            xLabel.Content = $"x: {wynik.x}";
            yLabel.Content = $"y: {wynik.y}";
            zLabel.Content = $"z: {wynik.z}";
        }

        private void Funkcja3_Button_Click(object sender, RoutedEventArgs e)
        {
            Func<double, double, double> funkcja3 = (x, y) =>
            {
                if (x > -1 && x < 1 && y > -2 && y < 2)
                {
                    return Math.Pow(x, 2) + Math.Pow(y, 2);
                }
                else
                {
                    return 30;
                }
            };

            var wynik = matematyka.ZnajdzMinFun2D(-2, 2, -3, 3, 100000, funkcja3);

            xLabel.Content = $"x: {wynik.x}";
            yLabel.Content = $"y: {wynik.y}";
            zLabel.Content = $"z: {wynik.z}";
        }
    }

    public enum Kategoria
    {
        Elektronika,
        Odzież,
        Spożywcze
    }

    public class Towar
    {
        public string Nazwa { get; set; }
        public decimal Cena { get; set; }
        public int Ilość { get; set; }
        public Kategoria Kategoria { get; set; }

        public Towar(string nazwa, decimal cena, int ilość, Kategoria kategoria)
        {
            Nazwa = nazwa;
            Cena = cena;
            Ilość = ilość;
            Kategoria = kategoria;
        }

        public override string ToString()
        {
            return $"{Nazwa} - {Cena:C} - Ilość: {Ilość} - Kategoria: {Kategoria}";
        }
    }

    public static class matematyka
    {
        public static (double x, double y, double z) ZnajdzMinFun2D(double minX, double maxX, double minY, double maxY, int iteracja, Func<double, double, double> calc)
        {
            Random rand = new Random();
            double? bestX = null;
            double? bestY = null;
            double? bestZ = null;

            for (int i = 0; i < iteracja; i++)
            {
                double x = rand.NextDouble() * (maxX - minX) + minX;
                double y = rand.NextDouble() * (maxY - minY) + minY;
                double z = calc(x, y);

                if (bestZ == null || z < bestZ)
                {
                    bestX = x;
                    bestY = y;
                    bestZ = z;
                }
            }
            return ((double)bestX, (double)bestY, (double)bestZ);
        }
    }

    // Przeniesienie rozszerzeń do osobnej klasy
    public static class Extensions
    {
        // Metoda rozszerzająca MinMax dla IEnumerable<T>
        public static (T min, T max) MinMax<T>(this IEnumerable<T> collection, Func<T, IComparable> selector)
        {
            if (collection == null || !collection.Any())
                throw new InvalidOperationException("Collection is empty or null.");

            var firstElement = collection.First();
            var min = selector(firstElement);
            var max = selector(firstElement);

            foreach (var item in collection.Skip(1))
            {
                var value = selector(item);
                if (value.CompareTo(min) < 0)
                {
                    min = value;
                }
                if (value.CompareTo(max) > 0)
                {
                    max = value;
                }
            }

            return (min, max);
        }
    }
}
