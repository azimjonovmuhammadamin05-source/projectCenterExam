import os
import sys
import tempfile

sys.path.insert(0, os.path.join(os.path.dirname(__file__), ".."))

from todo import Task, TaskManager


def make_manager():
    tmp = tempfile.NamedTemporaryFile(suffix=".json", delete=False)
    tmp.close()
    os.remove(tmp.name)
    return TaskManager(data_file=tmp.name)


def test_add_task():
    m = make_manager()
    t = m.add("write tests")
    assert t.title == "write tests"
    assert t.id == 1
    assert not t.done


def test_list_tasks():
    m = make_manager()
    m.add("a")
    m.add("b")
    assert len(m.list()) == 2


def test_remove_task():
    m = make_manager()
    t = m.add("temp")
    assert m.remove(t.id) is True
    assert len(m.list()) == 0


def test_remove_nonexistent():
    m = make_manager()
    assert m.remove(999) is False


def test_mark_done():
    m = make_manager()
    t = m.add("finish project")
    assert m.mark_done(t.id) is True
    assert m.list()[0].done is True


def test_task_to_dict_and_back():
    t = Task(1, "sample", True)
    d = t.to_dict()
    t2 = Task.from_dict(d)
    assert t2.id == t.id
    assert t2.title == t.title
    assert t2.done == t.done
