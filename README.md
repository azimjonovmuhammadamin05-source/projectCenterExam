# Todo CLI (C# / .NET)

Консольное приложение для управления задачами (todo-list) на C# (.NET 8).

## Структура
```
src/TodoApp/          — консольное приложение
tests/TodoApp.Tests/  — тесты (xUnit)
```

## Возможности
- Добавление задач с приоритетом, дедлайном и категорией
- Просмотр списка задач с фильтрацией и сортировкой
- Редактирование задач
- Поиск задач по ключевому слову
- Отметка задач как выполненных
- Удаление задач
- Цветной вывод в консоли (просроченные — красным, высокий приоритет — жёлтым, выполненные — серым)
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

# Добавить задачу
dotnet run -- add "Купить молоко"
dotnet run -- add "Сдать проект" --priority High --due 2026-08-20 --category work

# Список задач
dotnet run -- list
dotnet run -- list --pending
dotnet run -- list --category work
dotnet run -- list --sort-due
dotnet run -- list --sort-priority

# Редактировать
dotnet run -- edit 1 "Новое название" --priority Low

# Поиск
dotnet run -- search молоко

# Отметить выполненной / удалить
dotnet run -- done 1
dotnet run -- remove 1
```

## Тесты
```bash
dotnet test
```

## CI
При каждом push в `main` автоматически собирается проект и запускаются тесты через GitHub Actions.
