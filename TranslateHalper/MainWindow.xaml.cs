using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using TranslateHalper.Model;
using Path = System.IO.Path;

namespace TranslateHalper
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileInfo fi;
        private string _directPath, _mane, _buffstrig;
        private string[] _splitPath;
        private List<TextWorker> _textHelper;
        private List<TranslateDictonary> _buffer;

        private FolderBrowserDialog filderOpen;
        private OpenFileDialog openFile;
        private Nullable<DialogResult> dialogOK;

        // разного рода подготовки
        public MainWindow()
        {
            InitializeComponent();

            if (!File.Exists("Dictonary.txt"))
            {
                string[] text = new[] { "Словарь замен", "Образец заполнения:", "Оригинал : Перевод оригинала {*Gulp* : *Глоть*}" };
                File.WriteAllLines("Dictonary.txt", text);
            }
            List<string> eCoding = new List<string>{"Default", "UTF-8", "Unicode", "ASCII", "Windows-1251", "ISO-8859-6"};
            comboxCoder.ItemsSource = eCoding;
            comboxCoder.SelectedIndex = 0;
        }

        // путь к фаилу/папке (может можно уместить в одну функцию)
        private void OpenFileDialog_OnClick(object sender, RoutedEventArgs e)
        {
            if (DirectPathCheck.IsChecked == true)
            {
                filderOpen = new FolderBrowserDialog();
                filderOpen.ShowNewFolderButton = false;
                dialogOK = filderOpen.ShowDialog();
            }
            else
            {
                openFile = new OpenFileDialog();
                openFile.Multiselect = true;
                openFile.Filter = "Text|*.txt";
                openFile.DefaultExt = ".txt";
                dialogOK = openFile.ShowDialog();
            }

            if (dialogOK == System.Windows.Forms.DialogResult.OK)
            {
                string sFilenames = String.Empty;
                if (DirectPathCheck.IsChecked == true) sFilenames = filderOpen.SelectedPath;
                else
                {
                    foreach (var sFilename in openFile.FileNames)
                    {
                        sFilenames += ";" + sFilename;
                    }

                    sFilenames = sFilenames.Substring(1);
                }

                textBox.Text = sFilenames;
            }
        }

        // примочка для обнуления строки
        private void FocusClictBox(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == "Путь до фаила/папки")
            {
                textBox.Text = "";
            }
        }

        // открытие словоря
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("Dictonary.txt");
        }

        // информационное окно
        private void ButtonInfo_OnClick(object sender, RoutedEventArgs e)
        {
            MassegeWin massegeWin = new MassegeWin();
            this.Visibility = Visibility.Hidden;
            massegeWin.ShowDialog();
            this.Visibility = Visibility.Visible;
        }

        // главная кнопка (в процессе) разобраться сделать шкалу прогресса
        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            var pathText = textBox.Text;
            _splitPath = pathText.Split(';');

            var dictanary = EditDict();

            if (_splitPath.Length >= 1 && _splitPath.All(x => x.EndsWith(".txt")))
            {
                fi = new FileInfo(_splitPath[0]);
                var newDirectory = fi.Directory?.FullName + "_new";
                Directory.CreateDirectory(newDirectory);
                
                if (!fi.Name.EndsWith(".txt") && fi.DirectoryName != null)
                {
                    _directPath = fi.DirectoryName;

                    var filePaths = Directory.GetFiles(_directPath, "*.txt");
                    foreach (string path in filePaths)
                    {
                        TextWorker(path, dictanary);
                    }
                }
                else
                {
                    TextWorker(fi.FullName, dictanary);
                }
            }
        }

        // подготовка словаря
        private List<TranslateDictonary> EditDict()
        {
            string[] ditcStrings = System.IO.File.ReadAllLines(@"Dictonary.txt");

            Char[] splitChar = {':', ';', '|', '/'};
            _buffer = new List<TranslateDictonary>();

            foreach (var translateLine in ditcStrings)
            {
                if (translateLine.Split(splitChar).Length == 2 && translateLine.Split(splitChar)[0] != String.Empty
                                                               && translateLine.Split(splitChar)[1] != String.Empty)
                {
                    _buffer.Add(new TranslateDictonary
                    {
                        OriginalText = translateLine.Split(splitChar)[0].TrimStart(' ').TrimEnd(' '),
                        TranslateText = translateLine.Split(splitChar)[1].TrimStart(' ').TrimEnd(' ')
                    });
                }
            }
            return _buffer.OrderByDescending(x => x.OriginalText).ToList();
            
            //// разобраться с сортировкой
            //var orig = originaList.ToList();
            //orig.Sort((s1, s2) => s1.Length.CompareTo(s2.Length));
            //originaL = orig.ToArray();

            //var translate = translateList.ToList();
            //translate.Sort((s1, s2) => s1.Length.CompareTo(s2.Length));
            //translateL = translate.ToArray();
        }

        // конвертирует листы
        private List<TextWorker> ConvertList(List<string> nowResult)
        {
            var result = new List<TextWorker>();
            foreach (var item in nowResult)
            {
                result.Add(new TextWorker(){OrgText = item});
            }
            return result;
        }

        // обработчик текста (в процессе) улучшить?
        private void TextWorker(string pathText, List<TranslateDictonary> dictList)
        {
            var newF = new FileInfo(pathText);

            _textHelper = new List<TextWorker>();
            var bufferList = new List<string>();

            var gg = Path.Combine(newF.Directory.FullName + "_new\\", newF.Name);

            using (var src = new StreamReader(pathText, EncodingCase))
            using (var dst = new StreamWriter(gg, false, EncodingCase))
            {
                while ((_mane = src.ReadLine()) != null)
                {
                    if (_mane.StartsWith("> ") || _mane == String.Empty)
                    {
                        if (_mane == String.Empty && _textHelper.Last().OrgText.EndsWith("UNTRANSLATED"))
                        {
                            _textHelper.AddRange(ConvertList(bufferList));
                            bufferList.Clear();
                        }
                        else _textHelper.Add(new TextWorker {OrgText = _mane});
                    }
                    else
                    {
                        _buffstrig = _mane;
                        foreach (var removeItem in dictList)
                        {
                            _buffstrig = _buffstrig.Replace(removeItem.OriginalText, removeItem.TranslateText);
                        }

                        _textHelper.Add(new TextWorker {OrgText = _mane});
                        bufferList.Add(_buffstrig);
                        _buffstrig = null;
                    }
                }

                src.Dispose();
                src.Close();

                foreach (var all in _textHelper)
                {
                    dst.WriteLine(all.OrgText);
                }

                _textHelper.Clear();
                dst.Dispose();
                dst.Close();
            }
        }

        // подбор кодировки
        private Encoding EncodingCase
        {
            get
            {
                switch (comboxCoder.Text)
                {
                    case "Default": return Encoding.Default;
                    case "UTF-8": return Encoding.UTF8;
                    case "Unicode": return Encoding.Unicode;
                    case "ASCII": return Encoding.ASCII;
                    case "Windows-1251": return Encoding.GetEncoding("windows-1251");
                    case "ISO-8859-6": return Encoding.GetEncoding("iso-8859-6");
                    default: return Encoding.Default;
                }
            }
        }
    }
}
