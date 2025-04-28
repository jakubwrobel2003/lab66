using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Win32;
using static lab.MainWindow;
using System.Text.Json.Serialization;
using System;
using System.Text.Json;



namespace lab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();




        }

        private void btnMelduj_Click(object sender, RoutedEventArgs e)
        {
            string sciezkaPliku = "rejestr.txt";
            string tresc = $"{DateTime.Now:G} - Naciśnięto przycisk Melduj";


            using (FileStream fs = new FileStream(sciezkaPliku, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.WriteLine(tresc);
            }




        }
        List<double> liczby = new List<double>();
        private void btnCzytaj_Click(object sender, RoutedEventArgs e)
        {
            listBox.Items.Clear();
            liczby = new List<double>();
            string sciezkaPliku = "dane.txt";
            using (StreamReader reader = new StreamReader(sciezkaPliku))
            {
                string linia;
                while ((linia = reader.ReadLine()) != null)
                {
                    liczby.Add(Convert.ToDouble(linia));
                }
                reader.Close();
            }

            foreach (var item in liczby)
            {
                listBox.Items.Add(item);
            }

            double max, avg, min;

            max = liczby.OrderByDescending(x => x).FirstOrDefault();
            min = liczby.OrderByDescending(x => -x).FirstOrDefault();
            avg = liczby.Average(x => x);
            labelB.Content = max.ToString() + min.ToString() + avg.ToString();


        }

        [XmlRoot("Grupa")]
        public class Grupa
        {
            public string Nazwa { get; set; }
            public List<Student> Studenci { get; set; }


            public Grupa()
            {
                Studenci = new List<Student>();
            }


            public Grupa(string nazwa)
            {
                Nazwa = nazwa;
                Studenci = new List<Student>();
            }

            public int? LiczbaStudentów
            {
                get
                {
                    return Studenci.Count == 0 ? (int?)null : Studenci.Count;
                }
            }

            public double? ŚredniaOcen
            {
                get
                {
                    if (Studenci.Count == 0)
                        return null;
                    return Studenci.Average(s => s.Ocena);
                }
            }

            public void Wyświetl(ListBox listBox)
            {
                listBox.Items.Clear();
                listBox.Items.Add($"Grupa: {Nazwa}");
                foreach (var student in Studenci)
                {
                    listBox.Items.Add($"{student.Nazwisko} {student.Ocena:F1}");
                }
                listBox.Items.Add($"Liczba studentów: {LiczbaStudentów}");
                listBox.Items.Add($"Średnia ocen: {ŚredniaOcen:F1}");
            }

        }

        public class Student
        {
            public string Nazwisko { get; set; }
            public double Ocena { get; set; }


            public Student() { }


            public Student(string nazwisko, double ocena)
            {
                Nazwisko = nazwisko;
                Ocena = ocena;
            }
        }
        Grupa grupa = new Grupa("I16");
        private void ZapiszXML_Click(object sender, RoutedEventArgs e)
        {

           
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pliki XML (*.xml)|*.xml";

            if (saveFileDialog.ShowDialog() == true)
            {
                string sciezka = saveFileDialog.FileName;

              
                
                grupa.Studenci.Add(new Student("Kowalski", 4.5));
                grupa.Studenci.Add(new Student("Wiśniewski", 5.0));
                grupa.Studenci.Add(new Student("Nowak", 4.0));

               
                XmlSerializer serializer = new XmlSerializer(typeof(Grupa));

               
                using (StreamWriter writer = new StreamWriter(sciezka))
                {
                    serializer.Serialize(writer, grupa);
                }

                
                MessageBox.Show("Dane zostały zapisane do pliku XML!");
            }
        }

        private void WczytajXML_Click(object sender, RoutedEventArgs e)
        {
           
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pliki XML (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == true)
            {
                string sciezka = openFileDialog.FileName;

                
                XmlSerializer serializer = new XmlSerializer(typeof(Grupa));

                
                using (StreamReader reader = new StreamReader(sciezka))
                {
                    grupa = (Grupa)serializer.Deserialize(reader); 
                    listBox.Items.Clear();
                    listBox.Items.Add($"Grupa: {grupa.Nazwa}");

                    foreach (var student in grupa.Studenci)
                    {
                        listBox.Items.Add($"{student.Nazwisko} {student.Ocena:F1}");
                    }

                    listBox.Items.Add($"Liczba studentów: {grupa.LiczbaStudentów}");
                    listBox.Items.Add($"Średnia ocen: {grupa.ŚredniaOcen:F1}");
                }
            }
        }


        private void WczytajJSON_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pliki JSON (*.json)|*.json";

            if (openFileDialog.ShowDialog() == true)
            {
                string sciezka = openFileDialog.FileName;


                string jsonString = File.ReadAllText(sciezka);


                Grupa grupa = JsonSerializer.Deserialize<Grupa>(jsonString);

                listBox.Items.Clear();
                listBox.Items.Add($"Grupa: {grupa.Nazwa}");

                foreach (var student in grupa.Studenci)
                {
                    listBox.Items.Add($"{student.Nazwisko} {student.Ocena:F1}");
                }

                listBox.Items.Add($"Liczba studentów: {grupa.LiczbaStudentów}");
                listBox.Items.Add($"Średnia ocen: {grupa.ŚredniaOcen:F1}");
            }
        }
 

    private void ZapiszJSON_Click(object sender, RoutedEventArgs e)
    {
        
        grupa = new Grupa("I16");
        grupa.Studenci.Add(new Student("Kowalski", 4.5));
        grupa.Studenci.Add(new Student("Wiśniewski", 5.0));
        grupa.Studenci.Add(new Student("Nowak", 4.0));

        
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Pliki JSON (*.json)|*.json";

        if (saveFileDialog.ShowDialog() == true)
        {
            string sciezka = saveFileDialog.FileName;

           
            string jsonString = JsonSerializer.Serialize(grupa, new JsonSerializerOptions { WriteIndented = true });

          
            File.WriteAllText(sciezka, jsonString);

            MessageBox.Show("Dane zostały zapisane do pliku JSON!");
        }
    }

    }
}