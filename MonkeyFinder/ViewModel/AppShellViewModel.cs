namespace MonkeyFinder.ViewModel;

public partial class AppShellViewModel : ObservableObject
{  
  public AppShellViewModel()
  {
    SelectedTicketingAppName = "BINDING";
  }

  [ObservableProperty]
  string _selectedTicketingAppName;

  [ObservableProperty]
  bool _visible = true;
}
