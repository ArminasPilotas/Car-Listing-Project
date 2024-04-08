namespace FirstMauiApp.Controls;

public partial class FlyOutHeader : StackLayout
{
	public FlyOutHeader()
	{
		InitializeComponent();
		SetValues();
	}

	private void SetValues()
	{
		if (App.UserInfo is not null)
		{
			lblUserName.Text = App.UserInfo.Username;
			lblRole.Text = App.UserInfo.Role;
		}
	}
}