﻿namespace TinyMvvm;

public interface IViewModelBase : INotifyPropertyChanged
{
    /// <summary>
    /// Is ViewModel initialized.
    /// </summary>
    bool IsInitialized { get; set; }

    /// <summary>
    /// This method will run after that the ViewModel has been created. It will only run once.
    /// </summary>
    Task Initialize();

    /// <summary>
    /// This method will run after navigating back.
    /// </summary>
    Task Returning();

    /// <summary>
    /// This method will run every time the view is apperaing.
    /// </summary>
    Task OnAppearing();

    /// <summary>
    /// This method will run the first time the view is apperaing.
    /// </summary>
    Task OnFirstAppear();

    /// <summary>
    /// This method will run every time the view is disapperaing.
    /// </summary>
    Task OnDisappearing();

    /// <summary>
    /// This method will run after that the application resumes after has been sleeping.
    /// </summary>
    Task OnApplicationResume();

    /// <summary>
    /// This method will run when the application goes to sleep.
    /// </summary>
    Task OnApplicationSleep();

    /// <summary>
    /// Parameter that passed to this ViewModel when navigating back to it.
    /// </summary>
    object? ReturningParameter { get; set; }

    /// <summary>
    /// Parameter that passed to this ViewModel when navigated, will not be available in the constructor, use it when Initialize is running or later. 
    /// </summary>
    object? NavigationParameter { get; set; }

    /// <summary>
    /// Queryparameters as a Dictionary, this is only for use with Shell.
    /// </summary>
    Dictionary<string, string>? QueryParameters { get; set; }
}
