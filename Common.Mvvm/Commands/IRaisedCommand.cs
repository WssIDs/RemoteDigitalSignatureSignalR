﻿using System.Windows.Input;

namespace Common.Mvvm.Commands
{
    /// <summary>
    /// Интерфейс команды с возможностью вызова метода дополнительной проверки выполнения
    /// </summary>
    public interface IRaisedCommand : ICommand
    {
        /// <summary>
        /// Принудительная проверка доступности выполнения команды
        /// </summary>
        void RaiseCanExecuteChanged();
    }
}
