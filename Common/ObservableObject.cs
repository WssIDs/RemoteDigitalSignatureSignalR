using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Common
{
    /// <summary>
    /// Класс Template для обновления свойств модели
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    /// <summary>
    /// Класс Template для обновления свойств модели (DataErrorInfo)
    /// </summary>
    public class DataErrorInfoObservableObject : ObservableObject, IDataErrorInfo
    {
        public string Error
        {
            get => _error;
            set => Set(ref _error, value);
        }

        public string this[string columnName]
        {
            get
            {
                var result = string.Empty;

                if (string.IsNullOrEmpty(columnName) || columnName is not "Error") return result;
                result = string.Join("\n", DataErrors);
                return result;
            }
        }

        public void AddWithoutPropertyError(string error)
        {
            DataErrors.Remove(error);
            DataErrors.Add(error);
            OnPropertyChanged(nameof(Error));
        }

        public void RemoveWithoutPropertyError(string error)
        {
            DataErrors.Remove(error);
            OnPropertyChanged(nameof(Error));
        }

        public void ClearWithoutPropertyErrors()
        {
            DataErrors.Clear();
            OnPropertyChanged(nameof(Error));
        }

        public readonly List<string> DataErrors = new();
        private string _error = string.Empty;
    }

    /// <summary>
    /// Класс Template для обновления свойств модели и валидации используя DataAnnotation
    /// </summary>
    public class DataAnnotationObservableObject : ObservableObject, INotifyDataErrorInfo
    {
        /// <summary>
        /// 
        /// </summary>
        private bool _useValidate = true;

        protected void UseValidate(bool bUseValidate)
        {
            _useValidate = bUseValidate;
        }

        protected DataAnnotationObservableObject()
        {
            if (_useValidate)
            {
                // Валидация объекта при инициализации
                Validate();
            }
        }

        #region INotifyPropertyChanged

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == null) return;
            if (!_useValidate) return;
            Validate();
            CustomValidate(propertyName);
        }

        #endregion

        private readonly ConcurrentDictionary<string, List<string>> _errors = new();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        //private readonly object _lock = new object();

        public bool HasErrors => _errors.Any(propErrors => propErrors.Value is { Count: > 0 });

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return _errors.SelectMany(err => err.Value.ToList());
            if (!_errors.ContainsKey(propertyName)) return new List<string>();
            if (_errors.TryGetValue(propertyName, out var li) && li is {Count: > 0}) return li;

            return new List<string>();
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void ValidateProperty(object value, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) return;
            var validationContext = new ValidationContext(this)
            {
                MemberName = propertyName
            };

            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(value, validationContext, validationResults);

            //clear previous _errors from tested property  
            if (_errors.ContainsKey(propertyName))
                _errors.TryRemove(propertyName, out _);
            OnErrorsChanged(propertyName);
            HandleValidationResults(validationResults);
        }

        //private Task ValidateAsync() => Task.Run(Validate);

        //protected void Validate()
        //{
        //    lock (_lock)
        //    {
        //        var validationContext = new ValidationContext(this, null, null);
        //        var validationResults = new List<ValidationResult>();
        //        Validator.TryValidateObject(this, validationContext, validationResults, true);

        //        //clear all previous _errors  
        //        var propNames = _errors.Keys.ToList();
        //        _errors.Clear();
        //        propNames.ForEach(OnErrorsChanged);
        //        HandleValidationResults(validationResults);
        //    }
        //}

        private readonly ConcurrentDictionary<string, string> _externalErrors = new();

        /// <summary>
        /// Добавить ошибку для свойства
        /// </summary>
        /// <param name="error"></param>
        /// <param name="propertyName"></param>
        public void AddExternalError(string error, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName)) return;

            if (!_externalErrors.ContainsKey(propertyName))
            {
                _externalErrors.TryAdd(propertyName, error);
            }
            else
            {
                _externalErrors.TryRemove(propertyName, out _);
                _externalErrors.TryAdd(propertyName, error);
            }

            OnErrorsChanged(propertyName);
        }

        /// <summary>
        /// Удалить ошибку для свойства
        /// </summary>
        /// <param name="propertyName"></param>
        public void RemoveExternalError([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName)) return;
            _externalErrors.TryRemove(propertyName, out _);
            OnErrorsChanged(propertyName);
        }

        private ValidationContext _validationContext;

        protected void Validate()
        {
            _validationContext = new ValidationContext(this, null, null);

            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, _validationContext, validationResults, true);

            foreach (var kv in _errors.ToList()
                .Where(kv => validationResults.All(r => r.MemberNames.All(m => m != kv.Key))))
            {
                _errors.TryRemove(kv.Key, out _);
                OnErrorsChanged(kv.Key);
            }

            var q = from r in validationResults
                    from m in r.MemberNames
                    group r by m
                into g
                    select g;

            foreach (var prop in q)
            {
                var messages = prop.Select(r => r.ErrorMessage).ToList();

                if (_errors.ContainsKey(prop.Key))
                {
                    _ = _errors.TryRemove(prop.Key, out _);
                }

                if (_errors.TryAdd(prop.Key, messages!))
                {
                    OnErrorsChanged(prop.Key);
                }
            }

            if (_externalErrors.IsEmpty) return;
            foreach (var externalError in _externalErrors)
            {
                if (_errors.ContainsKey(externalError.Key))
                {
                    _errors[externalError.Key].Add(externalError.Value);
                }
                else
                {
                    _errors.TryAdd(externalError.Key, new List<string>() { externalError.Value });
                }

                OnErrorsChanged(externalError.Key);
            }
        }

        private void HandleValidationResults(IEnumerable<ValidationResult> validationResults)
        {
            //Group validation results by property names  
            var resultsByPropNames = from res in validationResults
                                     from mName in res.MemberNames
                                     group res by mName into g
                                     select g;
            //add _errors to dictionary and inform binding engine about _errors  
            foreach (var prop in resultsByPropNames)
            {
                var messages = prop.Select(r => r.ErrorMessage).ToList();
                _errors.TryAdd(prop.Key, messages!);
                OnErrorsChanged(prop.Key);
            }
        }

        /// <summary>
        /// Метод пользовательской валидации данных
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void CustomValidate([CallerMemberName] string propertyName = null) { }

        /// <summary>
        /// Возвращает состояния ошибки валидации для определенного свойства
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool HasPropertyErrors([CallerMemberName] string propertyName = null)
        {
            return !string.IsNullOrEmpty(propertyName) && _errors.ContainsKey(propertyName);
        }

        /// <summary>
        /// Добавить ошибку для свойства
        /// </summary>
        /// <param name="error"></param>
        /// <param name="propertyName"></param>
        public void AddError(string error, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName)) return;

            if (!_errors.ContainsKey(propertyName))
            {
                _errors.TryAdd(propertyName, new List<string>() { error });
            }
            else
            {
                _errors[propertyName].Remove(error);
                _errors[propertyName].Add(error);
            }

            OnErrorsChanged(propertyName);
        }
    }
}