using System.Text.Json;
using AdventureGameManual.Models;

namespace AdventureGameManual.Services;

// DO NOT wrap a class inside another class named NoteService.
// Make sure the file defines exactly ONE class NoteService in this namespace.
public class NoteService

{


    private readonly string _filePath;
    private readonly object _lock = new();
    private List<Note> _notes = new();
    private int _nextId = 1;

    public NoteService(string filePath)
    {
        _filePath = filePath;
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        Load();
    }

    public IReadOnlyList<Note> Notes => _notes.OrderByDescending(n => n.UpdatedAt).ToList();

    public Note Create(Note note)
    {
        note.Id = _nextId++;
        note.CreatedAt = note.UpdatedAt = DateTime.UtcNow;
        _notes.Add(note);
        Save();
        return note;
    }

    public void Update(Note updated)
    {
        var note = _notes.FirstOrDefault(n => n.Id == updated.Id);
        if (note is null) return;

        note.Title = updated.Title;
        note.Content = updated.Content;
        note.References = updated.References;
        note.UpdatedAt = DateTime.UtcNow;
        Save();
    }

    public void Delete(int id)
    {
        _notes.RemoveAll(n => n.Id == id);
        Save();
    }

    public void AddReference(int noteId, NoteReference reference)
    {
        var note = _notes.FirstOrDefault(n => n.Id == noteId);
        if (note is null) return;

        note.References.Add(reference);
        note.UpdatedAt = DateTime.UtcNow;
        Save();
    }

    public void RemoveReference(int noteId, NoteReference reference)
    {
        var note = _notes.FirstOrDefault(n => n.Id == noteId);
        if (note is null) return;

        note.References.RemoveAll(r => r.Kind == reference.Kind && r.TargetId == reference.TargetId);
        note.UpdatedAt = DateTime.UtcNow;
        Save();
    }

    private void Load()
    {
        if (!File.Exists(_filePath))
        {
            _notes = new();
            _nextId = 1;
            return;
        }

        try
        {
            var json = File.ReadAllText(_filePath);
            _notes = JsonSerializer.Deserialize<List<Note>>(json) ?? new();
            _nextId = _notes.Any() ? _notes.Max(n => n.Id) + 1 : 1;
        }
        catch
        {
            _notes = new();
            _nextId = 1;
        }
    }

    private void Save()
    {
        lock (_lock)
        {
            var json = JsonSerializer.Serialize(_notes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
