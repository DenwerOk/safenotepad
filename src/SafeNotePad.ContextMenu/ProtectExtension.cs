using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SharpShell.SharpContextMenu;
using System.Runtime.InteropServices;
using SharpShell.Attributes;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using SafeNotePad.ContextMenu.Properties;
using SafeNotePad.Models;
using SafeNotePad.Views;
using SafeNotePad.Facade;
using SafeNotePad.Utils;
using MessageBox = System.Windows.Forms.MessageBox;

namespace SafeNotePad.ContextMenu
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".txt")]
    public class ProtectExtension : SharpContextMenu
    {
        protected override bool CanShowMenu()
        {
            return SelectedItemPaths.Count() == 1;
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();

            var itemProtect = new ToolStripMenuItem
            {
                Text = "Protect",
                Image = new Bitmap(Resources.Farm_Fresh_note.ToBitmap(), 21, 21)
            };

            itemProtect.Click += (sender, args) => DoProtect();

            menu.Items.Add(itemProtect);

            return menu;
        }

        private void DoProtect()
        {
            if (!SelectedItemPaths.Any())
            {
                return;
            }

            var filePath = SelectedItemPaths.First();

            var note = new Note()
            {
                Path = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + "." + Settings.NoteExtension)
            };

            var dialog = new EditNote();
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dialog.ResizeMode = ResizeMode.NoResize;
            dialog.ViewModel.Initialize();
            dialog.Loaded += DialogOnLoaded;
            var dialogResult = dialog.ShowDialog();

            if (dialogResult.GetValueOrDefault())
            {
                note.AccessKey = dialog.ViewModel.NewAccessKey;
            }
            else
            {
                return;
            }

            var context = new ServiceContext();

            var newFile = GetNewFile(filePath);
            var result = context.EncryptNoteAndSave(note, newFile);
            if (!result.HasError)
            {
                File.Delete(filePath);
            }
            else
            {
                MessageBox.Show(string.Format("Couldn't save file: {0}", result.Error.Message));
            }
        }

        private static string GetNewFile(string filePath)
        {
            var newFile =
                "<HTML><BODY><DIV STYLE=\"text - align:Left; font - family:Segoe UI; font - style:normal; font - weight:normal; font - size:12; color:#000000;\">{0}</DIV></BODY></HTML>";
            var newString = new StringBuilder();
            foreach (var line in File.ReadAllLines(filePath))
            {
                newString.AppendFormat("<P><SPAN>{0}</SPAN></P>", Escape(line));
            }

            return string.Format(newFile, newString);
        }

        private static string Escape(string xml)
        {
            return xml.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }

        private void DialogOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var dialog = (EditNote) sender;
            dialog.Focus();
        }
    }
}
