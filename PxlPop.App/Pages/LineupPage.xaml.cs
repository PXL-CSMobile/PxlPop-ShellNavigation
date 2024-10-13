using PxlPop.App.Data;
using PxlPop.App.Models;

namespace PxlPop.App.Pages;

public partial class LineupPage : ContentPage
{
	private List<LessonGroup> groupedLessons = new List<LessonGroup>();

    public LineupPage()
	{
		InitializeComponent();
	}

    private async void OnPageAppearing(object sender, EventArgs e)
    {
        indicator.IsRunning = true;
        lessonsCollectionView.ItemsSource = groupedLessons;
        indicator.IsRunning = false;
    }

    private async void OnDepartmentChanged(object sender, EventArgs e)
    {
        if(departmentPicker.SelectedIndex > -1)
        {
            indicator.IsRunning = true;
            await LoadData(departmentPicker.SelectedItem.ToString());
            indicator.IsRunning = false;
        }
    }

    private async Task LoadData(string department)
    {
        groupedLessons.Clear();

        List<Lesson> lessons = await LessonData.GenerateLessonsAsync();
        List<Lesson> departmentLessons = lessons.Where(l => l.Department == department).ToList();

        List<string> days = departmentLessons.OrderBy(l => l.Begin).Select(l => l.Begin.DayOfWeek.ToString()).Distinct().ToList();

        foreach (string day in days)
        {
            List<Lesson> dayLessons = departmentLessons.Where(l => l.Begin.DayOfWeek.ToString() == day).OrderBy(l => l.Begin).ToList();
            groupedLessons.Add(new LessonGroup(day, dayLessons));
        }

        lessonsCollectionView.ItemsSource = groupedLessons;
    }
}