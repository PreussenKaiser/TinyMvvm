﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using TinyMvvm.Sample.Models;
using TinyMvvm.Sample.Services;

namespace TinyMvvm.Sample.ViewModels;

public partial class MainViewModel : TinyViewModel
{
    private readonly ICityService cityService;

    public MainViewModel(ICityService cityService)
    {
        this.cityService = cityService;
    }

    [ObservableProperty]
    private string text;

    private ICommand search;
    public ICommand Search => search ??= new TinyCommand(async () =>
    {
        IsBusy = true;

        var isMain = MainThread.IsMainThread;

        var result = await cityService.Search(Text);

        Cities = new ObservableCollection<City>(result);

        IsBusy = false;
    });

    private ICommand show;
    public ICommand Show => show ??= new TinyCommand<City>(async (city) =>
    {
        await Navigation.NavigateTo($"{nameof(DetailsViewModel)}?id=1", city);
    });

    [ObservableProperty]
    private ObservableCollection<City> cities = new ObservableCollection<City>();
}

