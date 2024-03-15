using Common.Mvvm.DI;
using Common.Mvvm.Dialog;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Mvvm
{
    /// <summary>
    /// Базовая модель-представления
    /// </summary>
    public abstract class ViewModelBase : DataAnnotationObservableObject
    {
        protected readonly Guid UniqueNumber;

        /// <summary>
        /// Сервис диалоговых окон
        /// </summary>
        protected IDialogService DialogService { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        protected ViewModelBase()
        {
            DialogService = SimpleIoC.Current.GetRequiredService<IDialogService>();
            UniqueNumber = Guid.NewGuid();
        }

        private bool _isOpened;

        public bool IsOpened
        {
            get => _isOpened;
            set => Set(ref _isOpened, value);
        }

        private string _title;

        /// <summary>
        /// Заголовок окна
        /// </summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{GetType().Name}_{UniqueNumber}";
    }
}