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
    public sealed partial class exercisesList : Page
    {
        public exercisesList()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            refresh();
        }
        private void refresh()
        {
            using (MobileContext db = new MobileContext())
            {
                exerciseList.ItemsSource = db.Exercises.OrderBy(r=> r.Name).ToList();
            }

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
            Frame.Navigate(typeof(exercisePage));
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделеный пункт меню
            if (exerciseList.SelectedItem != null)
            {
                Exercise ex = exerciseList.SelectedItem as Exercise;
                if (ex != null)
                    Frame.Navigate(typeof(exercisePage), ex.Id);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // получаем выделеный пункт меню
            if (exerciseList.SelectedItem != null)
            {
                Exercise ex = exerciseList.SelectedItem as Exercise;
                if (ex != null)
                {
                    using (MobileContext db = new MobileContext())
                    {
                        db.Exercises.Remove(ex);
                        db.SaveChanges();
                        exerciseList.ItemsSource = db.Exercises.ToList();
                    }
                }
            }

        }

    }
}
