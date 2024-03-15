using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using Common.Exceptions;
using Common.Mvvm.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Mvvm.Dialog
{
    /// <summary>
    /// Сервис для работы с диалоговыми окнами
    /// </summary>
    public abstract class DialogService : IDialogService
    {
        private readonly IContainerRegistry _containerRegistry;

        protected DialogService(IContainerRegistry containerRegistry)
        {
            _containerRegistry = containerRegistry;
        }

        #region Dialogs

        public TV Create<TV>(string title = null) where TV : ViewModelBase
        {
            var wnd = _containerRegistry.GetDialog<TV>();

            if (wnd is not Window dialog) throw new CommonException($"{nameof(wnd)} не является {typeof(Window)}");

            var wm = SimpleIoC.Current.GetRequiredService<TV>();

            //var wm = (TV)dialog.DataContext;

            dialog.DataContext = wm;
            
            //if (wm == null) return null;
            if (!string.IsNullOrEmpty(title))
            {
                wm.Title = title;
            }

            return wm;
        }

        TV IDialogService.Create<TV>(double percentHeight, double percentWidth)
        {
            if (percentWidth is > 1.0f or < 0)
                throw new Exception("Процент ширины окна не должен выходить за пределы диапазона [0,1].");
            if (percentHeight is > 1.0f or < 0)
                throw new Exception("Процент высоты окна не должен выходить за пределы диапазона [0,1].");

            var wnd = _containerRegistry.GetDialog<TV>();

            if (wnd is not Window dialog) throw new CommonException($"{nameof(wnd)} не является {typeof(Window)}");

            if (Math.Abs(percentHeight) > 0.0f)
            {
                wnd.Height = SystemParameters.WorkArea.Height * percentHeight;
            }

            if (Math.Abs(percentWidth) > 0.0f)
            {
                wnd.Width = SystemParameters.WorkArea.Width * percentWidth;
            }

#if DEBUG
            Console.WriteLine(
                $"Высота окна - {SystemParameters.WorkArea.Height}, Ширина окна - {SystemParameters.WorkArea.Width}");
#endif

            var wm = SimpleIoC.Current.GetRequiredService<TV>();

            //var wm = (TV)dialog.DataContext;
            dialog.DataContext = wm;
            //dialog.UpdateLayout();
            return wm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TV"></typeparam>
        /// <param name="title"></param>
        /// <param name="percentHeight"></param>
        /// <param name="percentWidth"></param>
        /// <returns></returns>
        public TV Create<TV>(string title, double percentHeight, double percentWidth) where TV : ViewModelBase
        {
            if (percentWidth is > 1.0f or < 0)
                throw new Exception("Процент ширины окна не должен выходить за пределы диапазона [0,1].");
            if (percentHeight is > 1.0f or < 0)
                throw new Exception("Процент высоты окна не должен выходить за пределы диапазона [0,1].");

            var wnd = _containerRegistry.GetDialog<TV>();

            if (wnd is not Window dialog) throw new CommonException($"{nameof(wnd)} не является {typeof(Window)}");

            if (Math.Abs(percentHeight) > 0.0f)
            {
                wnd.Height = SystemParameters.WorkArea.Height * percentHeight;
            }

            if (Math.Abs(percentWidth) > 0.0f)
            {
                wnd.Width = SystemParameters.WorkArea.Width * percentWidth;
            }

#if DEBUG
            Console.WriteLine(
                $"Высота окна - {SystemParameters.WorkArea.Height}, Ширина окна - {SystemParameters.WorkArea.Width}");
#endif

            //var wm = (TV)dialog.DataContext;

            var wm = SimpleIoC.Current.GetRequiredService<TV>();

            //var wm = (TV)dialog.DataContext;

            dialog.DataContext = wm;

            //if (wm == null) return null;
            if (!string.IsNullOrEmpty(title))
            {
                wm.Title = title;
            }

            return wm;
        }

        public bool? ShowViewModelDialog<TV>() where TV : ViewModelBase
        {
            bool? result = null;

            var type = _containerRegistry.GetDialogType<TV>();

            var
                wnd = /*Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.GetType().ToString() == type) ??*/
                    Application.Current.Windows.OfType<Window>()
                        .LastOrDefault(w => w.DataContext?.GetType() == typeof(TV));

            Debug.WriteLine(type);

            var owner = Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.IsActive || w.IsFocused)
                        ?? Application.Current.MainWindow;

            if (wnd == null) throw new NullReferenceException(nameof(wnd));

            if (wnd.DataContext is TV wm)
            {
                if (owner == null)
                {
                    result = wnd.ShowDialog();
                }
                else
                {
                    if (owner.IsActive)
                    {
                        wnd.Owner = owner;
                        Debug.WriteLine($"Owner - {owner.GetType()}");
                    }

                    result = wnd.ShowDialog();
                }
            }

            return result;
        }

        public void ShowViewModel<TV>(bool bOwner = true) where TV : ViewModelBase
        {
            var type = _containerRegistry.GetDialogType<TV>();

            var wnd = Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.GetType().ToString() == type);

            Debug.WriteLine(type);

            if (wnd == null) return;

            if (bOwner)
            {
                var owner = Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.IsActive || w.IsFocused)
                            ?? Application.Current.MainWindow;

                if (owner == null) return;
                Debug.WriteLine($"Owner - {owner.GetType()}");

                wnd.Owner = owner;
            }

            wnd.Show();
        }

        public void CloseViewModel<TV>() where TV : ViewModelBase
        {
            var type = _containerRegistry.GetDialogType<TV>();

            var wnd = Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.GetType().ToString() == type);

            wnd?.Close();
        }

        public void CloseApplicationForce()
        {
            Application.Current.Shutdown();
        }

        public void SetMainViewModel<TV>() where TV : ViewModelBase
        {
            var type = _containerRegistry.GetDialogType<TV>();

            var wnd = Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.GetType().ToString() == type);

            Debug.WriteLine(type);

            if (wnd == null) return;

            Application.Current.MainWindow = wnd;
        }

        public void ChangeSize<TV>(double percentHeight, double percentWidth, bool changePosition = true)
            where TV : ViewModelBase
        {
            if (percentWidth is > 1.0f or < 0)
                throw new Exception("Процент ширины окна не должен выходить за пределы диапазона [0,1].");
            if (percentHeight is > 1.0f or < 0)
                throw new Exception("Процент высоты окна не должен выходить за пределы диапазона [0,1].");

            var type = _containerRegistry.GetDialogType<TV>();

            var wnd = Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.GetType().ToString() == type);

            if (wnd == null) return;
            if (Math.Abs(percentHeight) > 0.0f)
            {
                wnd.Height = SystemParameters.WorkArea.Height * percentHeight;
            }

            if (Math.Abs(percentWidth) > 0.0f)
            {
                wnd.Width = SystemParameters.WorkArea.Width * percentWidth;
            }

#if DEBUG
            Console.WriteLine(
                $"Высота окна - {SystemParameters.WorkArea.Height}, Ширина окна - {SystemParameters.WorkArea.Width}");
#endif
            if (changePosition)
            {
                wnd.Top = (SystemParameters.WorkArea.Height - wnd.Height) / 0x00000002;
                wnd.Left = (SystemParameters.WorkArea.Width - wnd.Width) / 0x00000002;
            }

            wnd.UpdateLayout();
        }

        public DialogState GetDialogState<TV>() where TV : ViewModelBase
        {
            var type = _containerRegistry.GetDialogType<TV>();

            var wnd = Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.GetType().ToString() == type);

            if (wnd == null) throw new NullReferenceException(nameof(wnd));

            var state = wnd.WindowState switch
            {
                WindowState.Normal => DialogState.Normal,
                WindowState.Minimized => DialogState.Minimized,
                WindowState.Maximized => DialogState.Maximized,
                _ => throw new ArgumentOutOfRangeException()
            };

            return state;
        }

        public void Hide<TV>() where TV : ViewModelBase
        {
            var type = _containerRegistry.GetDialogType<TV>();

            var wnd = Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.GetType().ToString() == type);

            if (wnd == null) throw new NullReferenceException(nameof(wnd));

            wnd.Visibility = Visibility.Collapsed;
            wnd.WindowState = WindowState.Normal;
        }

        public void Show<TV>(DialogState state) where TV : ViewModelBase
        {
            var type = _containerRegistry.GetDialogType<TV>();

            var wnd = Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.GetType().ToString() == type);

            if (wnd == null) throw new NullReferenceException(nameof(wnd));
           
            wnd.Show();

            wnd.WindowState = state switch
            {
                DialogState.Normal => WindowState.Normal,
                DialogState.Minimized => WindowState.Minimized,
                _ => WindowState.Normal
            };
        }

        #endregion

        #region Messages

        public abstract bool? QuestionCancel(string message, string title = null, string header = null);

        public abstract void Error(string message, string title = null, string header = null);

        public abstract void Exception(string message, string title = null, string header = null);

        public abstract void Info(string message, string title = null, string header = null);

        public abstract void Notify(string message, string title = null, string header = null);

        public abstract bool Question(string message, string title = null, string header = null);

        public abstract void Success(string message, string title = null, string header = null);

        public abstract void Warning(string message, string title = null, string header = null);

        public bool OpenFileDialog(ref string filename)
        {
            return false;
            //throw new NotImplementedException();
        }

        public void SetCenter<TV>() where TV : ViewModelBase
        {
            var type = _containerRegistry.GetDialogType<TV>();

            var wnd = Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.GetType().ToString() == type);

            if (wnd == null) return;
#if DEBUG
            Console.WriteLine(
                $"Высота окна - {SystemParameters.WorkArea.Height}, Ширина окна - {SystemParameters.WorkArea.Width}");
#endif

           wnd.Top = (SystemParameters.WorkArea.Height - wnd.Height) / 0x00000002;
           wnd.Left = (SystemParameters.WorkArea.Width - wnd.Width) / 0x00000002;

            wnd.UpdateLayout();
        }

        public void ChangePosition<TV>(WindowLocation xPostion, WindowLocation yPostion) where TV : ViewModelBase
        {
            var type = _containerRegistry.GetDialogType<TV>();

            var wnd = Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.GetType().ToString() == type);

            if (wnd == null) return;
#if DEBUG
            Console.WriteLine(
                $"Высота окна - {SystemParameters.WorkArea.Height}, Ширина окна - {SystemParameters.WorkArea.Width}");
#endif

            if(xPostion == WindowLocation.Left)
            {
                wnd.Left = 0;
            }
            else if(xPostion == WindowLocation.Right)
            {
                wnd.Left = SystemParameters.WorkArea.Width - wnd.ActualWidth;
            }
            if(yPostion == WindowLocation.Top)
            {
                wnd.Top = 0;
            }
            else if(yPostion == WindowLocation.Bottom)
            {
                wnd.Top = SystemParameters.WorkArea.Height - wnd.ActualHeight;
            }

            wnd.UpdateLayout();
        }

        public WindowSize GetSize<TV>() where TV : ViewModelBase
        {
            var type = _containerRegistry.GetDialogType<TV>();

            var wnd = Application.Current.Windows.OfType<Window>().LastOrDefault(w => w.GetType().ToString() == type);

            if (wnd == null) throw new NullReferenceException(nameof(wnd));
#if DEBUG
            Console.WriteLine(
                $"Высота окна - {SystemParameters.WorkArea.Height}, Ширина окна - {SystemParameters.WorkArea.Width}");
#endif

            return new WindowSize
            {
                Height = wnd.Height,
                Width = wnd.Width,
            };
        }

        public void RestartApp()
        {
            var appLocation = Assembly.GetEntryAssembly();
            Process.Start(Path.ChangeExtension(appLocation.Location,".exe"));
            Application.Current.Shutdown();
        }

        #endregion
    }
}