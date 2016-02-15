using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TaskTimeTracker.Command;
using TaskTimeTracker.ViewModel.Base;

namespace TaskTimeTracker.ViewModel
{
    public class AppearanceSettingViewModel : ViewModelBase
    {
        private Dictionary<string, Color> _Themes;
        private Dictionary<string, Color> _Accents;

        private KeyValuePair<string, Color> _SelectedTheme;
        private KeyValuePair<string, Color> _SelectedAccent;

        public AppearanceSettingViewModel()
        {
            _Themes = new Dictionary<string, Color>();
            _Accents = new Dictionary<string, Color>();

            InitializeDefaultThemes();
            InitializeDefaultAccents();
        }

        #region Public Properties

        public Dictionary<string, Color> Accents
        {
            get {return _Accents; }
            set
            {
                if (_Accents != value)
                {
                    _Accents = value;
                    NotifyPropertyChanged(() => this.Accents);
                }
            }
        }

        public Dictionary<string, Color> Themes
        {
            get{ return _Themes; }
            set
            {
                if (_Themes != value)
                {
                    _Themes = value;
                    NotifyPropertyChanged(() => this.Themes);
                }
            }
        }

        public KeyValuePair<string, Color> SelectedAccent
        {
            get { return _SelectedAccent; }
            set
            {

                _SelectedAccent = value;

                //Lets set app accent
                var theme = ThemeManager.DetectAppStyle();
                ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(value.Key), theme.Item1);

            }
        }

        public KeyValuePair<string, Color> SelectedTheme
        {
            get { return _SelectedTheme; }
            set
            {
                _SelectedTheme = value;

                //Lets set app theme
                var theme = ThemeManager.DetectAppStyle();
                ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, ThemeManager.GetAppTheme(value.Key));
            }
        }

        #endregion

        #region RelayCommands

        #endregion

        #region Private Methods

        private void InitializeDefaultThemes()
        {
            _Themes.Add("BaseDark", (Color)ColorConverter.ConvertFromString("#252525"));
            _Themes.Add("BaseLight", (Color)ColorConverter.ConvertFromString("#FFFFFF"));
        }

        private void InitializeDefaultAccents()
        {
            _Accents.Add("Red", (Color)ColorConverter.ConvertFromString("#EA4333"));
            _Accents.Add("Green", (Color)ColorConverter.ConvertFromString("#80BA45"));
            _Accents.Add("Blue", (Color)ColorConverter.ConvertFromString("#1585B5"));                       
            _Accents.Add("Purple", (Color)ColorConverter.ConvertFromString("#837AE5"));
            _Accents.Add("Orange", (Color)ColorConverter.ConvertFromString("#FB8633"));
            _Accents.Add("Lime", (Color)ColorConverter.ConvertFromString("#B6D033"));
            _Accents.Add("Emerald", (Color)ColorConverter.ConvertFromString("#33A133"));
            _Accents.Add("Teal", (Color)ColorConverter.ConvertFromString("#33BCBA"));
            _Accents.Add("Cyan", (Color)ColorConverter.ConvertFromString("#49B4E8"));
            _Accents.Add("Cobalt", (Color)ColorConverter.ConvertFromString("#3373F2"));
            _Accents.Add("Indigo", (Color)ColorConverter.ConvertFromString("#8833FF"));
            _Accents.Add("Violet", (Color)ColorConverter.ConvertFromString("#BB33FF"));
            _Accents.Add("Pink", (Color)ColorConverter.ConvertFromString("#F68ED9"));
            _Accents.Add("Magenta", (Color)ColorConverter.ConvertFromString("#E0338F"));
            _Accents.Add("Crimson", (Color)ColorConverter.ConvertFromString("#B53351"));
            _Accents.Add("Amber", (Color)ColorConverter.ConvertFromString("#F3B53B"));
            _Accents.Add("Yellow", (Color)ColorConverter.ConvertFromString("#FEE538"));
            _Accents.Add("Brown", (Color)ColorConverter.ConvertFromString("#9B7B56"));
            _Accents.Add("Olive", (Color)ColorConverter.ConvertFromString("#8A9F83"));
            _Accents.Add("Steel", (Color)ColorConverter.ConvertFromString("#83919F"));
            _Accents.Add("Mauve", (Color)ColorConverter.ConvertFromString("#9180A1"));
            _Accents.Add("Taupe", (Color)ColorConverter.ConvertFromString("#9F9471"));
            _Accents.Add("Sienna", (Color)ColorConverter.ConvertFromString("#B37557"));
        }

        #endregion
    }
}
