﻿using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Common.Mvvm.Behaviors
{
    /// <summary>
    /// Behavior that will connect an UI event to a viewmodel Command,
    /// allowing the event arguments to be passed as the CommandParameter.
    /// </summary>
    public class EventToCommandBehavior : Behavior<FrameworkElement>
    {
        private Delegate _handler;
        private EventInfo _oldEvent;

        // Event
        public string Event
        {
            get => (string)GetValue(EventProperty);
            set => SetValue(EventProperty, value);
        }

        public static readonly DependencyProperty EventProperty = DependencyProperty.Register("Event", typeof(string),
            typeof(EventToCommandBehavior), new PropertyMetadata(null, OnEventChanged));

        // Command
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
            typeof(ICommand), typeof(EventToCommandBehavior), new PropertyMetadata(null));

        // PassArguments (default: false)
        public bool PassArguments
        {
            get => (bool)GetValue(PassArgumentsProperty);
            set => SetValue(PassArgumentsProperty, value);
        }

        public static readonly DependencyProperty PassArgumentsProperty = DependencyProperty.Register("PassArguments",
            typeof(bool), typeof(EventToCommandBehavior), new PropertyMetadata(false));


        private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var beh = (EventToCommandBehavior)d;

            if (beh.AssociatedObject != null) // is not yet attached at initial load
                beh.AttachHandler((string)e.NewValue);
        }

        protected override void OnAttached()
        {
            AttachHandler(Event); // initial set
        }

        /// <summary>
        /// Attaches the handler to the event
        /// </summary>
        private void AttachHandler(string eventName)
        {
            // detach old event
            if (_oldEvent != null)
                _oldEvent.RemoveEventHandler(this.AssociatedObject, _handler);

            // attach new event
            if (string.IsNullOrEmpty(eventName)) return;
            var ei = this.AssociatedObject.GetType().GetEvent(eventName);
            if (ei != null)
            {
                var mi = GetType().GetMethod("ExecuteCommand", BindingFlags.Instance | BindingFlags.NonPublic);
                if (mi != null) _handler = Delegate.CreateDelegate(ei.EventHandlerType ?? throw new InvalidOperationException(), this, mi);
                ei.AddEventHandler(AssociatedObject, _handler);
                _oldEvent = ei; // store to detach in case the Event property changes
            }
            else
                throw new ArgumentException(
                    $"The event '{eventName}' was not found on type '{AssociatedObject.GetType().Name}'");
        }

        /// <summary>
        /// Executes the Command
        /// </summary>
        private void ExecuteCommand(object sender, EventArgs e)
        {
            object parameter = (PassArguments ? e : null) ?? throw new InvalidOperationException();
            //if (Command == null) return;
            if (Command.CanExecute(parameter))
                Command.Execute(parameter);
        }
    }
}