using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
using Windows.UI.Core;
using Windows.Devices.Sensors;
using System.Data;
using Windows.UI.ViewManagement;

namespace myFintess
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class history : Page
    {
        ObservableCollection<Training> trainList;
        List<Exercise> exercisesList;
        List<DateTime> dates;
        SolidColorBrush Brush = new SolidColorBrush(Windows.UI.Colors.DarkGray);

        public history()
        {
            this.InitializeComponent();
            using (MobileContext db = new MobileContext())
            {
                dates = db.Trainings.Select(x => x.Date).Distinct().ToList();
            }
        }

        private void calendar_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            if (dates.Contains(args.Item.Date.Date))
                args.Item.Background = Brush;
            else args.Item.Background = null;
        }

        private void calendar_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            if (calendar.SelectedDates == null) return;
            try
            {
                DateTime date = calendar.SelectedDates.First().Date;
                if (dates.Contains(date))
                {
                    using (MobileContext db = new MobileContext())
                    {
                        exercisesList = db.Exercises.ToList();
                        trainList = new ObservableCollection<Training>(db.Trainings.Where(r => r.Date.Date == date).Include(x => x.ex).ToList());
                    }
                    List.ItemsSource = trainList;
                }
                else List.ItemsSource = null;
            }
            catch { }
        }
    }
}
