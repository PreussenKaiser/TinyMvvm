﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyMvvm;

namespace ClassicSampleApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<string> names;
        public ObservableCollection<string> Names
        {
            get => names;
            set => Set(ref names, value);
        }

        public async override Task Initialize()
        {
            IsBusy = true;

             await base.Initialize();

            Names = new ObservableCollection<string>(new List<string>()
            {
                "Daniel",
                "Ella",
                "Willner"
            });

            IsBusy = false;
        }

        public override Task OnAppearing()
        {
            return base.OnAppearing();
        }

        public override Task OnDisappearing()
        {
            return base.OnDisappearing();
        }

        private ICommand details;
        public ICommand Details => details ??= new TinyCommand<string>(async(name) =>
        {
            await Navigation.NavigateToAsync("DetailsView", name);
        });
    }
}
