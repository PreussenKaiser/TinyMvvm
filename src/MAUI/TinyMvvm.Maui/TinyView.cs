﻿namespace TinyMvvm;

public abstract class TinyView : ContentPage
{
    internal SemaphoreSlim InternalLock { get; private set; } = new SemaphoreSlim(1, 1);

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext is ITinyViewModel viewModel)
        {
            try
            {
                if (!viewModel.IsInitialized)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await InternalLock.WaitAsync();
                        await viewModel.Initialize();

                        viewModel.IsInitialized = true;
                    });
                }
            }
            finally
            {
                InternalLock.Release();
            }
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ITinyViewModel viewModel)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await InternalLock.WaitAsync();

                if (!viewModel.IsInitialized)
                {
                    await viewModel.Initialize();
                    viewModel.IsInitialized = true;
                }

                await viewModel.OnAppearing();

                InternalLock.Release();
            });

        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (BindingContext is ITinyViewModel viewModel)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await InternalLock.WaitAsync();

                await viewModel.OnDisappearing();

                InternalLock.Release();
            });
        }
    }
}