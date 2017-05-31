using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GroupManagementProject_01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Student> students = new ObservableCollection<Student>();


        public MainWindow()
        {
            InitializeComponent();
            
            students = MyStorage.readXML<ObservableCollection<Student>>("Data.xml");
            if (students == null) students = new ObservableCollection<Student>();
            //generateData();
        }

        private void generateData()
        {
            Student student1 = new Student
            {
                id = 1,
                name = "Doe",
                firstName = "John",
                group = "Group1"
            };
            Student student2 = new Student
            {
                id = 2,
                name = "Doe",
                firstName = "Jane",
                group = "Group1"
            };
            Student student3 = new Student
            {
                id = 3,
                name = "Mustermann",
                firstName = "Max",
                group = "Group1"
            };
            Student student4 = new Student
            {
                id = 4,
                name = "Mustermann",
                firstName = "Erika",
                group = "Group2"
            };
            Student student5 = new Student
            {
                id = 5,
                name = "Maier",
                firstName = "Hans",
                group = "Group2"
            };
            students.Add(student1);
            students.Add(student2);
            students.Add(student3);
            students.Add(student4);
            students.Add(student5);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lbx_students.ItemsSource = students;
            
           
        }

        private void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var el = sender as Ellipse;
            char char_lastCharacter = el.Name.Last();
            int int_ellipseNumber = int.Parse(char_lastCharacter.ToString());
            //int int_indexToSelect = int_ellipseNumber - 1;
            //lbx_students.SelectedIndex = int_indexToSelect;

            var element = (from s in students where s.id == int_ellipseNumber select s).FirstOrDefault();
            if (element != null)
            {
                lbx_students.SelectedItem = element;
            }
            

            //lbx_students.SelectedIndex = int.Parse(el.Name.Last().ToString()) - 1;

            //MessageBox.Show(int.Parse(el.Name.Last().ToString()).ToString());
        }

        private void lbx_students_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            initEllipses();
            var element = (Student)((sender as ListBox).SelectedItem);
            if (element == null) return;

            Ellipse el = (Ellipse) FindName("el_" + element.id);
            
            if(el != null)
            { 
                el.Stroke = Brushes.Black;
            }
            fillIndividualEllipse();
        }

        private void initEllipses()
        {
            foreach (var item in grid_groupImage.Children)
            {
                if (item.GetType()==typeof(Ellipse)) ((Ellipse)item).Stroke = Brushes.Transparent;
            }
        }

        private void tbx_filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var elements = (from s in students where s.firstName.StartsWith(tbx_filter.Text, StringComparison.InvariantCultureIgnoreCase) select s).ToList();
            var elements_contain = (from s in students where s.firstName.ToLower().Contains(tbx_filter.Text.ToLower()) select s).ToList();
            elements.AddRange(elements_contain);
            lbx_students.ItemsSource = elements.Distinct();
        }

        private void btn_addStudent_Click(object sender, RoutedEventArgs e)
        {
            Student studentNew = new Student();
            studentNew.id = students.Count + 1;
            studentNew.name = "Please Edit!";
            students.Add(studentNew);
            lbx_students.SelectedItem = studentNew;
        }

        private void btn_deleteStudent_Click(object sender, RoutedEventArgs e)
        {
            if (lbx_students.SelectedItem != null)
            {
                students.Remove((Student)lbx_students.SelectedItem);
            }
            else { MessageBox.Show("Please Select a student to be removed."); }
        }

        private void window_mainWindow_Closed(object sender, EventArgs e)
        {
            MyStorage.storeXML<ObservableCollection<Student>>(students, "Data.xml");
        }

        private void tconitem_studentDetails_Loaded(object sender, RoutedEventArgs e)
        {




        }

        private void fillIndividualEllipse()
        {
            var element = (Student)(lbx_students.SelectedItem);
            if (element == null) return;

            Ellipse el = (Ellipse)FindName("el_" + element.id);

            ImageBrush ib_individual = new ImageBrush();
            ib_individual.ImageSource = image_groupPicture.Source;
            ib_individual.Stretch = Stretch.UniformToFill;
            ib_individual.Viewport = new Rect(0.0, 0.0, k1.0, 1.0);
            double x, y, w, h;
            x = el.Margin.Left / image_groupPicture.ActualWidth;
            y = el.Margin.Top / image_groupPicture.ActualHeight;
            w = el.ActualWidth / image_groupPicture.ActualWidth;
            h = el.ActualHeight / image_groupPicture.ActualHeight;

            ib_individual.Viewbox = new Rect(x, y, w, h);
            el_individualPicture.Fill = ib_individual;
        }
    }
}
