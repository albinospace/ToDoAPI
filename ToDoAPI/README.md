# Todo API
Простой REST API для управления задачами (Todo), написанный на ASP.NET Core + Entity Framework Core + SQLite.

## Технологии
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- C#

## Структура проекта
```TodoApi/
- Controllers/         # API-контроллеры
- Data/                # DbContext
- DTOs/                # Объекты для передачи данных
- Models/              # Сущности базы данных
- Services/            # Бизнес-логика
- Program.cs
- appsettings.json
```

## Возможности
- Создание, чтение, обновление и удаление задач
- Отметка задачи как выполненной
- Фильтрация задач (все / выполненные / активные)
- Использование DTO и Service-слоя

## Запуск
1. Клонировать репозиторий
2. Открыть решение в Visual Studio
3. Нажать F5

После запуска откроется Swagger UI.
## Примеры запросов
### Создать задачу
`POST /api/todos`
```json
{
  "title": "Купить молоко",
  "description": "В магазине у дома"
}
```

### Получить все задачи
`GET /api/todos`

### Получить только активные
`GET /api/todos?completed=false`

### Отметить как выполненную
`PATCH /api/todos/{id}/complete`

### Обновить задачу
`PUT /api/todos/{id}`
```
{
  "title": "Купить молоко (обновлено)",
  "description": "Придется идти завтра"
}
```


