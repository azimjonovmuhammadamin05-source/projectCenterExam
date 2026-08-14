# Todo CLI (C# / .NET)

Консольное приложение для управления задачами (todo-list) на C# (.NET 8).

## Структура
```
src/TodoApp/          — консольное приложение
tests/TodoApp.Tests/  — тесты (xUnit)
```

## Возможности
- Добавление задач
- Просмотр списка задач
- Отметка задач как выполненных
- Удаление задач
- Хранение данных в JSON-файле (tasks.json)

## Установка
```bash
git clone https://github.com/azimjonovmuhammadamin05-source/projectCenterExam.git
cd projectCenterExam
dotnet restore
```

## Использование
```bash
cd src/TodoApp

dotnet run -- add "Купить молоко"
dotnet run -- list
dotnet run -- done 1
dotnet run -- remove 1
```

## Тесты
```bash
dotnet test
```

## CI
При каждом push в `main` автоматически собирается проект и запускаются тесты через GitHub Actions.
