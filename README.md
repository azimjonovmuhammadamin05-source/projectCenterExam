# Todo CLI

Простое консольное приложение для управления задачами (todo-list) на Python.

## Возможности
- Добавление задач
- Просмотр списка задач
- Отметка задач как выполненных
- Удаление задач
- Хранение данных в JSON-файле

## Установка
```bash
git clone <repo_url>
cd projectCenterExam
```

## Использование

```bash
# Добавить задачу
python3 todo.py add "Купить молоко"

# Показать список задач
python3 todo.py list

# Отметить задачу выполненной
python3 todo.py done 1

# Удалить задачу
python3 todo.py remove 1
```

## Тесты
```bash
pip install pytest
python3 -m pytest tests/ -v
```

## CI
При каждом push в `main` автоматически запускаются тесты через GitHub Actions.

