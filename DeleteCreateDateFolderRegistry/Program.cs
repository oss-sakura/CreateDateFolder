using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CreateDateFolder;
using Microsoft.Win32;

namespace DeleteCreateDateFolderRegistry
{
    internal class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<保留中>")]
        private static void Main(string[] args)
        {
            if (!CreateDateFolderRegistry.IsAdministrator())
            {
                _ = MessageBox.Show(
                    "「管理者として実行」で起動してください。",
                    "アンインストール",
                    MessageBoxButton.OK,
                    MessageBoxImage.Stop);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "CreateDateFolderのレジストリ設定を削除します。",
                "アンインストール",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (result.Equals(MessageBoxResult.Yes))
            {
                try
                {
                    if (!CreateDateFolderRegistry.RegistryExists(
                        Registry.ClassesRoot, CreateDateFolderConst.DATE_FOLDER_KEY))
                    {
                        _ = MessageBox.Show(
                            "削除対象のレジストリが存在しません。\n処理を終了します。",
                            "アンインストール",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        return;
                    }
                }
                catch (Exception)
                {
                    _ = MessageBox.Show(
                        "レジストリ読込に失敗しました。",
                        "アンインストール",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                try
                {
                    CreateDateFolderRegistry.DeleteSubKeyTree(
                        Registry.ClassesRoot, CreateDateFolderConst.DATE_FOLDER_KEY);
                }
                catch (Exception)
                {
                    _ = MessageBox.Show(
                        "レジストリ削除に失敗しました。",
                        "アンインストール",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
                _ = MessageBox.Show(
                    "レジストリ削除が成功しました。",
                    "アンインストール",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                _ = MessageBox.Show(
                    "レジストリ削除を中止しました。",
                    "アンインストール",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}
