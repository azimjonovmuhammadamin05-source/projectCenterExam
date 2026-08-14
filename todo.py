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
