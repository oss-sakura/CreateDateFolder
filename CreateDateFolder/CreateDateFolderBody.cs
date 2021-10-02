using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;

namespace CreateDateFolder
{
    public class CreateDateFolderBody
    {
        private string FolderyName { get; set; }            // yyyyMMdd
        private bool SequenceEanble { get; set; }           // 通し番号の付与の有効無効フラグ
        private string SequenceNumFormat { get; set; }      // 0:D数値
        private string Delimiter { get; set; }              // YYYYYMMDD-SeqNo, YYYYMMDD_SeqNo
        private string[] Args { get; set; }                 // 呼び出し引数
        private bool IsAdmin { get; set; }                  // 管理者として実行？
        private DateTime DT { get; set; }                   // フォルダ名に使用する日付
        private string TargetDirectory { get; set; }        // ディレクトリ名に使用する日付
        public bool LaunchRightClick { get; private set; }  // 右クリックメニューからの起動？
        public bool LaunchIconClick { get; private set; }   // アイコンからの起動？

        public CreateDateFolderBody(string[] args)
        {
            ReadProperties();

            Delimiter = Properties.Settings.Default.DelimiterChar;
            IsAdmin = CreateDateFolderRegistry.IsAdministrator();

            if (args.Length == 2)
            {
                // 右クリックメニューからの起動
                Args = args;
                TargetDirectory = Args[1];
                DT = DateTime.Now;
                FolderyName = DT.ToString("yyyyMMdd");
                LaunchRightClick = true;
                LaunchIconClick = !LaunchRightClick;
            }
            else
            {
                LaunchIconClick = true;
                LaunchRightClick = !LaunchIconClick;
            }
        }

        public bool SettingExistAndCreate()
        {
            bool result = CreateDateFolderRegistry.RegistryExists(
                Registry.ClassesRoot,
                CreateDateFolderConst.DATE_FOLDER_KEY);
            if (!result && !IsAdmin)
            {
                _ = MessageBox.Show(
                    "本プログラムに必要なレジストリ設定が登録されていません。\n" +
                    "レジストリ登録のため、初回起動のみ「管理者として実行」で起動してください。",
                    "お願い",
                    MessageBoxButton.OK,
                    MessageBoxImage.Stop);
            }
            else if (!result && IsAdmin)
            {
                CreateContextMenuRegistry();
            }

            return result;
        }

        public bool CheckRightClickArguments()
        {
            if ((Args.Length == 2) && Args[0].Equals(CreateDateFolderConst.RIGHT_CLICK_ARGUMENT))
            {
                return true;
            }
            _ = MessageBox.Show(
                "不正なレジストリ値または引数を検出したので処理を終了します。",
                "報告",
                MessageBoxButton.OK,
                MessageBoxImage.Stop);

            return false;
        }

        public void CreateYMDFolder()
        {
            string createDirPath;
            if (!SequenceEanble)
            {
                createDirPath = TargetDirectory + Path.DirectorySeparatorChar + FolderyName;
                if (Directory.Exists(createDirPath))
                {
                    _ = MessageBox.Show(
                        "既に " + FolderyName + "フォルダは存在します。",
                        "報告",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }
                else
                {
                    _ = Directory.CreateDirectory(createDirPath);
                }
            }
            else
            {
                int seq = GenerateSequenceNo();
                string seqString = string.Format(SequenceNumFormat, seq);
                createDirPath = TargetDirectory + Path.DirectorySeparatorChar + FolderyName + Delimiter + seqString;
                _ = Directory.CreateDirectory(createDirPath);
            }
        }

        private void ReadProperties()
        {
            SequenceNumFormat = null;
            Delimiter = "";
            SequenceEanble = Properties.Settings.Default.AddSeqNo;
            if (SequenceEanble)
            {
                SequenceNumFormat = "{0:D" + Properties.Settings.Default.ZeroPaddingLength + "}";
                Delimiter = Properties.Settings.Default.DelimiterChar;
            }
        }

        private int GenerateSequenceNo()
        {
            if (!SequenceEanble)
            {
                return 0;
            }

            string searchPattern = FolderyName + Delimiter + @"*";
            string[] dirs = Directory.GetDirectories(
                TargetDirectory,
                searchPattern,
                SearchOption.TopDirectoryOnly);

            if (dirs.Length == 0)
            {
                return 1;
            }

            List<int> seqList = new List<int>();
            foreach (string dirname in dirs)
            {
                string deleteString = TargetDirectory + Path.DirectorySeparatorChar + FolderyName + Delimiter;
                string numstr = dirname.Replace(deleteString, "");

                bool success = int.TryParse(numstr, out int outnum);
                if (success)
                {
                    seqList.Add(outnum);
                }
            }

            return seqList.Count == 0 ? 1 : seqList.Max() + 1;
        }

        private void CreateContextMenuRegistry()
        {
            // Directory\Background\shell\CreateDateFolder\null
            CreateDateFolderRegistry.CreateSubKeyAndSetValue(
                Registry.ClassesRoot,
                CreateDateFolderConst.DATE_FOLDER_KEY,
                null,
                CreateDateFolderConst.DATE_FOLDER_KEY_VALUE);

            // Directory\Background\shell\CreateDateFolder\command
            string cmdFullPath = string.Format(
                CreateDateFolderConst.DATE_FOLDER_COMMAND_KEY_VALUE,
                 System.Reflection.Assembly.GetExecutingAssembly().Location);
            CreateDateFolderRegistry.CreateSubKeyAndSetValue(
                Registry.ClassesRoot,
                CreateDateFolderConst.DATE_FOLDER_COMMAND_KEY,
                null,
                cmdFullPath);

            // Directory\Background\shell\CreateDateFolder\icon
            CreateDateFolderRegistry.CreateSubKeyAndSetValue(
                Registry.ClassesRoot,
                CreateDateFolderConst.DATE_FOLDER_KEY,
                @"icon",
                System.Reflection.Assembly.GetExecutingAssembly().Location);

            _ = MessageBox.Show(
                "本プログラムに必要なレジストリを登録しました。",
                "報告",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}