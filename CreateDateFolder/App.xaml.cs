using System;
using System.Windows;

namespace CreateDateFolder
{
    public partial class App : Application
    {
        private CreateDateFolderBody body { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<保留中>")]
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            body = new CreateDateFolderBody(e.Args);

            try
            {
                if (!body.SettingExistAndCreate())
                {
                    Shutdown();
                    return;
                }
            }
            catch (Exception)
            {
                _ = MessageBox.Show(
                    "エラーが発生しました。\n" +
                    "処理を終了します。",
                    "報告",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                base.Shutdown();
                return;
            }

            if (body.LaunchRightClick)
            {
                if (!body.CheckRightClickArguments())
                {
                    base.Shutdown();
                    return;
                }
                // Create YYYYMMDD Folder
                try
                {
                    body.CreateYMDFolder();
                }
                catch (Exception)
                {
                    _ = MessageBox.Show(
                        "フォルダ作成に失敗しました。\n" +
                        "アクセス権限などによりエラーが発生した可能性があります。",
                        "失敗",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    Shutdown();
                    return;
                }
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (body.LaunchIconClick)
            {
                MainWindow w = new MainWindow();
                w.Show();
            }
            else
            {
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
