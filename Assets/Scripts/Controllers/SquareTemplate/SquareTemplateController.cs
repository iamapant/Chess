using System.Collections.Generic;

public class SquareTemplateController {
    private Dictionary<string, SquareTemplate> templates = new();

    public bool TryGetTemplate(string templateName, out SquareTemplate template) =>templates.TryGetValue(templateName, out template);
    public void AddTemplate(string templateName, SquareTemplate template) =>templates.Add(templateName, template);
    public void RemoveTemplate(string templateName) =>templates.Remove(templateName);

    public void ApplyTemplate(Square square, string templateName) {
        
    }
}