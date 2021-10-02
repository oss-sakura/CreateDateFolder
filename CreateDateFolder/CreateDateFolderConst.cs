using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateDateFolder
{
    public static class CreateDateFolderConst
    {
        // RIGHT Click
        public const string RIGHT_CLICK_ARGUMENT = @"/contextmenu";
        // RIGHT Click Menu
        public const string DATE_FOLDER_KEY = @"Directory\Background\shell\CreateDateFolder";
        public const string DATE_FOLDER_KEY_VALUE = @"日付フォルダの作成";
        // RIGHT Click Command
        public const string DATE_FOLDER_COMMAND_KEY = @"Directory\Background\shell\CreateDateFolder\command";
        public const string DATE_FOLDER_COMMAND_KEY_VALUE = "\"{0}\" \"" + RIGHT_CLICK_ARGUMENT + "\" \"%V\"";
    }
}
