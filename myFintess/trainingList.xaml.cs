using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace myFintess
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class trainingList : Page
    {
        ObservableCollection<Training> list;
        List<Exercise> exercisesList;

        public trainingList()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (MobileContext db = new MobileContext())
            {
                exercisesList = db.Exercises.ToList();
                list = new ObservableCollection<Training>(db.Trainings.Where(r => r.Date.Date == DateTime.Now.Date).Include(x => x.ex).ToList());
            }
            tList.ItemsSource = list;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack) Frame.GoBack();
        }

        private void App_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame.CanGoBack)
            {
                frame.GoBack(); // переход назад
                e.Handled = true; // указываем, что событие обработано
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(trainingPage));
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделеный пункт меню
            if (tList.SelectedItem != null)
            {
                Training tr = tList.SelectedItem as Training;
                if (tr != null)
                    Frame.Navigate(typeof(trainingPage), tr.Id);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделеный пункт меню
            if (tList.SelectedItem != null)
            {
                Training tr = tList.SelectedItem as Training;
                if (tr != null)
                {
                    using (MobileContext db = new MobileContext())
                    {
                        db.Trainings.Remove(tr);
                        db.SaveChanges();
                        exercisesList = db.Exercises.ToList();
                        list = new ObservableCollection<Training>(db.Trainings.Where(r => r.Date.Date == DateTime.Now.Date).Include(x => x.ex).ToList());
                        tList.ItemsSource = list;
                    }
                }
            }

        }
    }
}
