# TinyMvvm (for Xamarin Forms)

TinyMvvm is yet a small MVVM framework designed specifically with Xamarin Forms in mind. 

This documentation does not cover the basics of MVVM and assumes that you are already familiar with the concept of MVVM.


## How to install
Install the TinyMvvm.Forms package from NuGet, https://www.nuget.org/packages/TinyMvvm.Forms/

```
Install-Package TinyMvvm.Forms 
```
If you want to separate the ViewModels in a separate project that doesn't have references to Xamarin.Forms, just install the TinyMvvm package on that project.


```
Install-Package TinyMvvm 
```

## Initiation

### Configure navigation
```csharp
// Option 1:
var navigationHelper = new NavigationHelper(this);
navigationHelper.RegisterView<MainView>("MainView");
		
// Option 2: Register single views		
var navigationHelper = new FormsNavigationHelper(this);		
navigationHelper.RegisterView("MainView", typeof(MainView));		
		
// Option 3: Register all views (pages) that is inherited from Page		
// The class name will be the key. To use this, you need to add using System.Reflection;		
var asm = typeof(App).GetTypeInfo().Assembly;		
navigationHelper.RegisterViewsInAssembly(asm);

// Option 4: For Xamarin.Forms Shell URL navigation. Option 1-3 can be combined with Option 4 because ShellNavigationHelper is a subclass of FormsNavigationHelper.
var navigationHelper = new ShellNavigationHelper();

```

## The overall structure

### ShellViewBase&lt;T&gt;
ShellViewBase shoule be used when using Xamarin.Forms Shell. It should be used as with ViewBase<T>, described below. But if you use ViewBase, it will not work with navigation paramters or query paramters.

### ViewBase&lt;T&gt;

Features (or drawbacks) of the ViewBase

* It strongly types the ViewModel as any class implementing INotifyPropertyChanged
* It creates the ViewModel for you if you inherit the ViewModel from ViewModelBase

The view should inherit from `ViewBase<T>` where `T` is the ViewModel. The ViewModel can be any class that implements `INotifyPropertyChanged`. 

`ViewBase<T>` itself inherits from `Xamarin.Forms.ContentPage` and can be treated by Xamarin Forms as any page.

If you decide to use `ViewModelBase` as the base class for your view model and at the same time have the IoC resolver enabled, the the view will automatically create the view model for you when the view is created. Hence no need to inject the view model in the constructor and assign it manually. Feature or not, you decide.

An example of a typical page in TinyMvvm would look like this xaml copied from the sample app.

```xml
<?xml version="1.0" encoding="UTF-8"?>
<tinymvvm:ViewBase x:TypeArguments="viewmodels:MainViewModel" 
    xmlns="http://xamarin.com/schemas/2014/forms" 
    xmlns:viewmodels="clr-namespace:TinyMvvm.Forms.Sample.ViewModels;assembly=TinyMvvm.Forms.Samples"
    xmlns:tinymvvm="clr-namespace:TinyMvvm.Forms;assembly=TinyMvvm.Forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
    x:Class="TinyMvvm.Forms.Sample.Views.MainView">
    
	<ContentPage.Content>
        <StackLayout Margin="10,40">
            <Label Text="Hello World" />
            <Button Text="About" Command="{Binding NavigateTo}" CommandParameter="AboutView" />
        </StackLayout>
    </ContentPage.Content>
    
</tinymvvm:ViewBase>
```

What you need to do is:

* Define two namespaces (viewmodels and tinymvvm)
* Change the view type to `tinymvvm:ViewBase`
* Add a type argument pointing to your view model

The only thing that changes in the code behind is the base class.

```csharp
public partial class MainView : TinyMvvm.Forms.ViewBase<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
    }
}
```

### ViewModelBase

TinyMvvm also defines a base class for the view model called `ViewModelBase`.

Features (or drawbacks) of the ViewModelBase

* Wraps navigation for you through the INavigation interface (Implemented in TinyNavigation)
* Implements INotifyPropertyChanged for you
* Propagates life cycle events to the view (Initialize, OnAppearing, OnDisapparing)

### Navigation from ViewModel

Shell URL navigation:
```csharp
await Navigation.NavigateToAsync("//home/messages/id=1");
```
Xamarin.Forms traditional:

```csharp
await Navigation.NavigateToAsync("AboutView");
```
with parameter:

```csharp
var myObject = new MyObject();

await Navigation.NavigateToAsync("AboutView", myObject);
```

or open as modal:

```csharp
await Navigation.OpenModalAsync("AboutView");
```

#### QueryParameters in ViewModelBase
With URL navigation you can pass query parameters. With TinyMvvm you can access them from the ViewModel via the **QueryParameters** property of **ViewModelBase**. QueryParameters is of type Dictionary<string, string>.

```csharp
public override async Task Initialize()
{
	await base.Initialize();
	
	var id = QueryParameters["id"];
}
```


#### NavigationParameter in ViewModelBase
```csharp
public override async Task Initialize()
{
	await base.Initialize();
	
	var myObject = NavigationParameter as MyObject;
}
```

### IoC
Tiny Mvvm is not bound to any specific IoC provider. There are a provider for Autoface that you can install with the "TinyMvvm.Autoface" package.

```
Install-Package TinyMvvm.Autofac
```

TinyMvvm has a Resolver in it's core project. To use it you need to add on provider to it that implements the IResolver interface, for example our Autofac provider.

```csharp
var container = builder.Build();
var resolver = new AutofacResolver(container);
Resolver.SetResolver(resolver);
```			

```csharp
var navigationHelper = Resolver.Resolve<INavigationHelper>();
```

### TinyCommand

TinyMvvm has an own implementation of ICommand that not has any references to Xamarin.Forms so you can use it in an library without reference Xamarin.Forms.

```csharp
public ICommand WithParameter
{
      get
      {
                return new TinyCommand(async() =>
                {
                   //Do stuff
                });
      }
}
```

#### Messaging

TinyMvvm has no messaging system built-in, we recommend you to take a look at TinyPubSub if you need messaging in your app.

Check out [TinyPubSub](https://github.com/johankson/TinyPubSub) for more detailed information about it.

