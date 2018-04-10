using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace myFintess
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class trainingPage : Page
    {
        Training tr;

        public trainingPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            using (MobileContext db = new MobileContext())
            {
                cmbTraining.ItemsSource = db.Exercises.OrderBy(r => r.Name).ToList();
                cmbTraining.DisplayMemberPath = "Name";
                cmbTraining.SelectedValuePath = "Id";
            }
            if (e.Parameter != null)
            {
                int id = (int)e.Parameter;
                using (MobileContext db = new MobileContext())
                {
                    tr = db.Trainings.FirstOrDefault(c => c.Id == id);
                }
            }

            if (tr != null)
            {
                headerBlock.Text = "Редактирование подхода";
                cmbTraining.SelectedValue = tr.ExerciseId;
                tbWeight.Text = tr.Weight.ToString();
                tbIteration.Text = tr.Iteration.ToString();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Exercise _ex = cmbTraining.SelectedItem as Exercise;
            if (_ex == null) return;

            bool isRecord = false;

            using (MobileContext db = new MobileContext())
            {
                if (tr != null)
                {
                    tr.ExerciseId = Int32.Parse(cmbTraining.SelectedValue.ToString());
                    tr.Weight = short.Parse(tbWeight.Text.ToString());
                    tr.Iteration = short.Parse(tbIteration.Text.ToString());
                    tr.ex = _ex;
                    db.Trainings.Update(tr);
                    db.SaveChanges();
                }
                else
                {
                    Training tr = new Training
                    {
                        Date = DateTime.Now.Date,
                        Weight = Int16.Parse(tbWeight.Text.ToString()),
                        Iteration = Int16.Parse(tbIteration.Text.ToString()),
                        ExerciseId = Int32.Parse(cmbTraining.SelectedValue.ToString()),
                        ex = _ex
                    };
                    db.Exercises.Attach(_ex);
                    db.Trainings.Add(tr);

                    if ((tr.Weight > _ex.Weight) || (tr.Weight == _ex.Weight && tr.Iteration > _ex.Iteration))
                    {
                        _ex.Weight = tr.Weight;
                        _ex.Iteration = tr.Iteration;
                        isRecord = true;
                    }
                    db.SaveChanges();
                }
            }
            if (isRecord)
            {
                Frame.Navigate(typeof(congratulations));
               
            }
            else GoToMainPage();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            GoToMainPage();
        }

        private void GoToMainPage()
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
            else
                Frame.Navigate(typeof(trainingList));
            
        }
    }
}
