﻿using System;
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
    public sealed partial class exercisePage : Page
    {
        Exercise ex;

        public exercisePage()
        {
            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                int id = (int)e.Parameter;
                using (MobileContext db = new MobileContext())
                {
                    ex = db.Exercises.FirstOrDefault(c => c.Id == id);
                }
            }

            if (ex != null)
            {
                headerBlock.Text = "Редактирование упражнения";
                tbName.Text = ex.Name;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (MobileContext db = new MobileContext())
            {
                if (ex != null)
                {
                    ex.Name = tbName.Text;
                    db.Exercises.Update(ex);
                }
                else
                {
                    db.Exercises.Add(new Exercise { Name = tbName.Text });
                }
                db.SaveChanges();
            }
            GoToMainPage();
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
                Frame.Navigate(typeof(exercisesList));
        }

    }
}
