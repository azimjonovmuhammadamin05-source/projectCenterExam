import argparse
import json
import os

DATA_FILE = "tasks.json"


class Task:
    def __init__(self, task_id: int, title: str, done: bool = False):
        self.id = task_id
        self.title = title
        self.done = done

    def to_dict(self) -> dict:
        return {"id": self.id, "title": self.title, "done": self.done}

    @staticmethod
    def from_dict(data: dict) -> "Task":
        return Task(data["id"], data["title"], data.get("done", False))

    def __str__(self) -> str:
        status = "x" if self.done else " "
        return f"[{status}] {self.id}: {self.title}"


class TaskManager:
    def __init__(self, data_file: str = DATA_FILE):
        self.data_file = data_file
        self.tasks: list[Task] = []
        self.load()

    def load(self) -> None:
        if os.path.exists(self.data_file):
            with open(self.data_file, "r", encoding="utf-8") as f:
                data = json.load(f)
                self.tasks = [Task.from_dict(d) for d in data]

    def save(self) -> None:
        with open(self.data_file, "w", encoding="utf-8") as f:
            json.dump([t.to_dict() for t in self.tasks], f, indent=2, ensure_ascii=False)

    def add(self, title: str) -> Task:
        new_id = (max((t.id for t in self.tasks), default=0)) + 1
        task = Task(new_id, title)
        self.tasks.append(task)
        self.save()
        return task

    def list(self) -> list[Task]:
        return self.tasks

    def remove(self, task_id: int) -> bool:
        for t in self.tasks:
            if t.id == task_id:
                self.tasks.remove(t)
                self.save()
                return True
        return False

    def mark_done(self, task_id: int) -> bool:
        for t in self.tasks:
            if t.id == task_id:
                t.done = True
                self.save()
                return True
        return False


def build_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(description="Todo CLI")
    subparsers = parser.add_subparsers(dest="command")

    add_p = subparsers.add_parser("add", help="Add a new task")
    add_p.add_argument("title")

    subparsers.add_parser("list", help="List all tasks")

    remove_p = subparsers.add_parser("remove", help="Remove a task")
    remove_p.add_argument("id", type=int)

    done_p = subparsers.add_parser("done", help="Mark a task as done")
    done_p.add_argument("id", type=int)

    return parser


def main():
    parser = build_parser()
    args = parser.parse_args()
    manager = TaskManager()

    if args.command == "add":
        task = manager.add(args.title)
        print(f"Added: {task}")
    elif args.command == "list":
        for t in manager.list():
            print(t)
    elif args.command == "remove":
        ok = manager.remove(args.id)
        print("Removed" if ok else "Not found")
    elif args.command == "done":
        ok = manager.mark_done(args.id)
        print("Marked done" if ok else "Not found")
    else:
        parser.print_help()


if __name__ == "__main__":
    main()
