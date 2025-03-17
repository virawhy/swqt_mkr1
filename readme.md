#Модель якості
## 🔹 1. Функціональна придатність
- **Покриття функціональних вимог (%)** – частка реалізованих функцій відповідно до вимог.
- **Кількість критичних дефектів** – помилки, що блокують роботу додатку.
- **Процент успішного виконання тестів** – відношення пройдених тестів до загальної кількості тестів.

## 🔹 2. Надійність
- **Середній час між збоями (MTBF)** – середній час роботи без відмов.
- **Час відновлення після збою (MTTR)** – середній час, необхідний для усунення помилки.
- **Відсоток відмов під навантаженням** – визначає стабільність під високими навантаженнями.

## 🔹 3. Продуктивність
- **Час відповіді (Response Time)** – середній час виконання запиту.
- **Використання ресурсів (CPU, RAM, Disk I/O)** – аналіз ефективності використання ресурсів.
- **Кількість одночасних запитів (Concurrency Rate)** – максимальна кількість запитів, які система може обробити без деградації.

## 🔹 4. Зручність супроводу (Maintainability)
- **Чистота коду (Code Smells Count)** – кількість порушень принципів чистого коду.
- **Середня складність коду (Cyclomatic Complexity)** – кількість незалежних шляхів виконання в коді.
- **Кількість повторного використання коду (%)** – частка повторного використання кодових блоків.
- **Покриття тестами (Test Coverage, %)** – частка коду, покрита юніт-тестами.

## 🔹 5. Переносимість
- **Крос-платформна сумісність (%)** – кількість підтримуваних платформ.
- **Відповідність стандартам (Linting Compliance)** – відсоток коду, що відповідає стандартам розробки.

#Ключові метрики коду
## 🔹 Продуктивність та ефективність  
- **Час виконання** – Швидкість виконання основних операцій.  
- **Споживання ресурсів** – Використання CPU та пам’яті.  

## 🔹 Надійність та безпека  
- **MTBF (Mean Time Between Failures)** – Середній час між збоями.  
- **Кількість вразливостей** – Виявлені проблеми безпеки (OWASP, CVE).  

## 🔹 Обслуговуваність  
- **Цикломатична складність** – Вимірює складність логіки коду.  
- **Читабельність коду** – Оцінка за допомогою аналізаторів (SonarQube).  

## 🔹 Сумісність та переносимість  
- **Кросплатформеність** – Підтримка різних ОС і середовищ.  
- **Легкість міграції** – Час та зусилля на адаптацію коду.  

## 🔹 Функціональність та тестування  
- **Покриття тестами (%)** – Відсоток коду, що перевіряється тестами.  
- **Частота дефектів** – Кількість багів на 1000 рядків коду (KLOC).  

#Формальна верифікація специфікацій
```
module Trello

// Основні сутності
sig User {
    tasks: set Task
}

sig Task {
    list: one List
}

sig List {
    board: one Board,
    tasks: set Task
}

sig Board {
    lists: set List
}

// Обмеження
fact Constraints {
    // Кожен користувач має хоча б одну задачу
    all u: User | some u.tasks

    // Кожна задача входить лише до одного списку
    all t: Task | one t.list

    // У кожному списку є лише ті задачі, які до нього прив'язані
    all l: List | l.tasks = {t: Task | t.list = l}

    // У кожній дошці є хоча б один список
    all b: Board | some b.lists

    // У кожного списку є лише одна дошка
    all l: List | one l.board

}

// Перевірка коректності: наявність прикладу, який задовольняє усі обмеження
run {} for 5

// Перевірка конкретних сценаріїв
pred CheckExample {
    some b: Board, l: List, t: Task, u: User |
        b.lists = l and l.tasks = t and t in u.tasks
}
run CheckExample for 5
```
![image](https://github.com/user-attachments/assets/784fbed3-21be-4b74-96e9-5cc11f92c9d7)
![image](https://github.com/user-attachments/assets/23f303c9-b453-4de6-aa33-8cf64345916c)

#Use Case діаграма
```
@startuml

actor User

    User --> (Add Task)
    User --> (Edit Task)
    User --> (Delete Task)
    User --> (Move Task to Between Lists)
    User --> (Add List)
    User --> (Delete List)

@enduml
```
![image](https://github.com/user-attachments/assets/4c83aec4-8a79-40ef-91f6-c6d1c738670e)

#Діаграма послідовностей
```
@startuml

actor User
participant "Form1" as F
participant "ListBox" as LB
participant "ContextMenuStrip" as CM

User -> F : Запуск програми
F -> CM : Ініціалізація контекстного меню
F -> F : Ініціалізація списку завдань

User -> F : Клацання ПКМ по формі
F -> CM : Відображення меню створення списку
User -> F : Вибір "Create New List"
F -> LB : Додавання нового списку

User -> LB : Клацання ПКМ по списку
F -> CM : Відображення меню списку
User -> F : Вибір "Add Task"
F -> LB : Додавання нового завдання

User -> LB : Вибір завдання, клацання ПКМ
F -> CM : Відображення меню завдання
User -> F : Вибір "Edit Task"
F -> LB : Редагування вибраного завдання

User -> LB : Вибір завдання, клацання ПКМ
F -> CM : Відображення меню завдання
User -> F : Вибір "Delete Task"
F -> LB : Видалення вибраного завдання

User -> LB : Клацання ПКМ по списку
F -> CM : Відображення меню списку
User -> F : Вибір "Delete List"
F -> LB : Видалення списку

User -> LB : Вибір завдання, клацання ПКМ
F -> CM : Відображення меню завдання
User -> F : Вибір "Move Task To"
F -> LB : Переміщення завдання у вибраний список

@enduml
```
![image](https://github.com/user-attachments/assets/6202b6b7-93b1-4ae2-9a9b-c989a3fdc3e9)

#Діаграма класів
```
@startuml

class Form1 {
    - List<ListBox> taskLists
    - ContextMenuStrip listContextMenu
    - ContextMenuStrip formContextMenu
    - int nextListX
    - int nextListY
    - ListBox activeListBox
    - ToolStripMenuItem moveTaskMenuItem

    + Form1()
    - void CreateNewList_Click(object, EventArgs)
    - void AddTask_Click(object, EventArgs)
    - void EditTask_Click(object, EventArgs)
    - void DeleteTask_Click(object, EventArgs)
    - void DeleteList_Click(object, EventArgs)
    - void MoveTask_Click(object, EventArgs)
    - void ListBoxTasks_MouseDown(object, MouseEventArgs)
    - void Form_MouseDown(object, MouseEventArgs)
    - void UpdateMoveTaskMenu()
    + static void Main()
}

class ListBox {
    + ListBox()
    + int SelectedIndex
    + object SelectedItem
    + int IndexFromPoint(Point)
    + void Items.Add(object)
    + void Items.Remove(object)
}

class ContextMenuStrip {
    + ContextMenuStrip()
    + ToolStripItemCollection Items
    + void Show(Control, Point)
}

class ToolStripMenuItem {
    + ToolStripMenuItem(string)
    + object Tag
    + event EventHandler Click
}

class AddTaskForm {
    + AddTaskForm()
    + AddTaskForm(Task)
    + Task NewTask
    + DialogResult ShowDialog()
}

class Task {
    + string Name
    + string Description
    + DateTime? DueDate
    + List<string> AssignedPersons
    + string Status
}

Form1 "1" o-- "*" ListBox : має
Form1 "1" o-- "1" ContextMenuStrip : має
Form1 "1" o-- "1" ToolStripMenuItem : має
Form1 "1" o-- "1" AddTaskForm : викликає
Form1 "1" o-- "*" Task : містить
ListBox "1" *-- "*" Task : містить
ContextMenuStrip "1" *-- "*" ToolStripMenuItem : містить
ToolStripMenuItem "1" --> "1" ListBox : може взаємодіяти

@enduml


```
![image](https://github.com/user-attachments/assets/472f4b7f-7a4c-47ab-b0d5-4300fa41b8a9)


#Скріншоти виконання
![image](https://github.com/user-attachments/assets/d62ac5cb-0f46-4e6a-9a3c-37073a0a979f)
![image](https://github.com/user-attachments/assets/02c7e6eb-eebb-47d4-bb5c-e8e00f76cba2)
![image](https://github.com/user-attachments/assets/22478070-270c-4150-aea9-6fd8f405ad83)
![image](https://github.com/user-attachments/assets/7929814b-bad2-471c-9c41-71f0ed134c43)
![image](https://github.com/user-attachments/assets/a16a0d87-e0d7-46e8-88db-0371e2e9fded)


