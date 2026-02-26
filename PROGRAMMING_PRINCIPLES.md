# PROGRAMMING PRINCIPLES

## 1. MVVM (Model-View-ViewModel) Pattern
The application implements the MVVM architectural pattern, separating concerns between the UI layer and business logic.

### Example Implementation:
- **View Layer**: [OrdersView.xaml](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/View/OrdersView.xaml) and [ReportView.xaml](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/View/ReportView.xaml)
- **ViewModel Layer**: [OrdersViewModel.cs](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/ViewModels/OrdersViewModel.cs) and [ReportViewModel.cs](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/ViewModels/ReportViewModel.cs)
- **Model Layer**: [User.cs](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Models/User.cs)

The ViewModels act as intermediaries between the View and the database service, handling all business logic and command execution.

## 2. Single Responsibility Principle (SRP)
Each class has a single, well-defined responsibility:

- **ObservableObject** ([Base/ObservableObject.cs](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Base/ObservableObject.cs#L1-L14)): Handles property change notification
- **RelayCommand** ([Base/RelayCommand.cs](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Base/RelayCommand.cs#L1-L23)): Encapsulates command execution logic
- **DbService** ([Service/DbService.cs](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Service/DbService.cs#L1-L20)): Handles all database operations

## 3. Dependency Injection
Classes receive their dependencies through constructor injection:

- [MainViewModel.cs line 49-52](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/ViewModels/MainViewModel.cs#L49-L52): DbService is instantiated in the constructor
- [OrdersViewModel.cs line 25-31](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/ViewModels/OrdersViewModel.cs#L25-L31): User object is passed as a dependency
- [ReportViewModel.cs line 20](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/ViewModels/ReportViewModel.cs#L20): DbService instantiation

## 4. ICommand Pattern Implementation
The application uses the WPF ICommand interface through RelayCommand:

- [RelayCommand.cs - Implementation](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Base/RelayCommand.cs#L5-L23)
- [MainViewModel.cs - Usage Examples](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/ViewModels/MainViewModel.cs#L41-L48)
- [OrdersViewModel.cs - Command Definitions](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/ViewModels/OrdersViewModel.cs#L23-L25)

## 5. INotifyPropertyChanged Pattern
Observable properties enable automatic UI updates when data changes:

- [ObservableObject.cs - Base Implementation](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Base/ObservableObject.cs#L6-L12)
- [MainViewModel.cs - Property Usage](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/ViewModels/MainViewModel.cs#L13-L20)
- [OrdersViewModel.cs - Observable Collections](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/ViewModels/OrdersViewModel.cs#L15-L19)

## 6. Separation of Concerns
Clear separation between UI logic and business logic:

- UI Layer: [DeliverySystem.UI](https://github.com/yuxarida/DeliveryApp/tree/main/DeliverySystem.UI) - XAML views and ViewModels
- Business Logic Layer: [DeliverySystem.Core](https://github.com/yuxarida/DeliveryApp/tree/main/DeliverySystem.Core) - Database service and models
- The [MainWindow.xaml binding](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/MainWindow.xaml#L12-L14) demonstrates clean data binding

## 7. Database Access Abstraction
All database operations are centralized in DbService:

- [DbService.cs - Login Method](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Service/DbService.cs#L14-L34)
- [DbService.cs - GetOrders Method](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Service/DbService.cs#L36-L55)
- [DbService.cs - CRUD Operations](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Service/DbService.cs#L56-L97)

## 8. Parameterized Queries (SQL Injection Prevention)
Using parameterized queries with SqlParameter to prevent SQL injection:

- [DbService.cs - Login Query](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Service/DbService.cs#L19-L20)
- [DbService.cs - AddOrder with Parameters](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Service/DbService.cs#L62-L73)
- [DbService.cs - UpdateOrderStatus with Parameters](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Service/DbService.cs#L91-L98)

## 9. Error Handling
Try-catch blocks ensure graceful error handling:

- [OrdersViewModel.cs - LoadData Error Handling](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/ViewModels/OrdersViewModel.cs#L39-L41)
- [OrdersViewModel.cs - DoAdd Method](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/ViewModels/OrdersViewModel.cs#L48-L60)
- [ReportViewModel.cs - DoCalculate Method](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/ViewModels/ReportViewModel.cs#L25-L31)

## 10. Resource Management (Using Statements)
Proper use of using statements for database connections:

- [DbService.cs - Login Method](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Service/DbService.cs#L15-L34) - Multiple nested using statements
- [DbService.cs - GetOrders Method](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Service/DbService.cs#L36-L55) - Ensures connection closure
- [DbService.cs - AddOrder Method](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Service/DbService.cs#L57-L73) - Automatic resource cleanup

## 11. Role-Based Access Control
Authorization logic based on user roles:

- [OrdersViewModel.cs - AdminVisibility Property](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/ViewModels/OrdersViewModel.cs#L20-L21)
- [MainViewModel.cs - WelcomeMsg Property](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/ViewModels/MainViewModel.cs#L27)
- [DbService.cs - GetOrders Filtering](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Service/DbService.cs#L45-L55)

## 12. Data Binding and XAML
Declarative UI with data binding eliminates boilerplate code:

- [MainWindow.xaml - DataContext Binding](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/MainWindow.xaml#L12-L14)
- [OrdersView.xaml - DataGrid Binding](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/View/OrdersView.xaml#L14-L15)
- [ReportView.xaml - Property Binding](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/View/ReportView.xaml#L10-L15)

## 13. Material Design Pattern
Using Material Design library for modern, consistent UI:

- [App.xaml - Theme Resources](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/App.xaml#L6-L12)
- [MainWindow.xaml - Material Components](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/MainWindow.xaml#L17-L20)
- [OrdersView.xaml - DataGrid Styling](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/View/OrdersView.xaml#L17-L22)

## 14. Stored Procedures Usage
Leveraging database stored procedures for complex operations:

- [DbService.cs - RecalculatePrice Method](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Service/DbService.cs#L109-L119)

## 15. DateTime Handling
Proper date and time management for reporting:

- [DbService.cs - GetRevenueByPeriod](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.Core/Service/DbService.cs#L100-L116)
- [ReportViewModel.cs - Date Properties](https://github.com/yuxarida/DeliveryApp/blob/main/DeliverySystem.UI/ViewModels/ReportViewModel.cs#L10-L12}
