﻿using FirstMauiApp.Controls;

namespace FirstMauiApp.Helpers
{
    public static class MenuBuilder
    {
        public static void BuildMenu()
        {
            Shell.Current.Items.Clear();

            Shell.Current.FlyoutHeader = new FlyOutHeader();

            var role = App.UserInfo.Role;

            if (role.Equals("Administrator"))
            {
                var flyOutItem = new FlyoutItem()
                {
                    Title = "Admin Car Management",
                    Route = nameof(MainPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = "dotnet_bot.svg",
                            Title = "Admin Page 1",
                            ContentTemplate = new DataTemplate(typeof(MainPage))
                        }
                    }
                };

                if (!Shell.Current.Items.Contains(flyOutItem))
                {
                    Shell.Current.Items.Add(flyOutItem);
                }
            }

            if (role.Equals("User"))
            {
                var flyOutItem = new FlyoutItem()
                {
                    Title = "User Car Management",
                    Route = nameof(MainPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = "dotnet_bot.svg",
                            Title = "User Page 1",
                            ContentTemplate = new DataTemplate(typeof(MainPage))
                        }
                    }
                };

                if (!Shell.Current.Items.Contains(flyOutItem))
                {
                    Shell.Current.Items.Add(flyOutItem);
                }
            }

            var logoutFlyoutItem = new FlyoutItem()
            {
                Title = "Logout",
                Route = nameof(LogoutPage),
                FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem,
                Items =
                {
                    new ShellContent
                    {
                        Icon = "dotnet_bot.svg",
                        Title = "Logout",
                        ContentTemplate = new DataTemplate(typeof(LogoutPage))
                    }
                }
            };

            if (!Shell.Current.Items.Contains(logoutFlyoutItem))
            {
                Shell.Current.Items.Add(logoutFlyoutItem);
            }
        }
    }
}
