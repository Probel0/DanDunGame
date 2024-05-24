using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Lukomor.Reactive;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class CodeGenerator : MonoBehaviour
{
    [MenuItem("Tools/Generate View Classes")]
    public static void GenerateViewClasses()
    {
        // Путь к директории для сгенерированных файлов
        string outputPath = "Assets/GeneratedViews/";
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        // Получение всех интерфейсов, наследующих I2ViewModel
        var viewModelInterfaces = Assembly.GetAssembly(typeof(I2ViewModel))
                                          .GetTypes()
                                          .Where(t => t.IsInterface && typeof(I2ViewModel).IsAssignableFrom(t))
                                          .ToArray();

        foreach (var viewModelInterface in viewModelInterfaces)
        {
            GenerateViewClass(viewModelInterface, outputPath);
        }

        // Обновляем проект, чтобы сгенерированные файлы появились в Unity
        AssetDatabase.Refresh();
    }

    private static void GenerateViewClass(Type viewModelInterface, string outputPath)
    {
        string className = viewModelInterface.Name.Replace("IVM", "") + "View";
        string fileName = outputPath + className + ".cs";

        var properties = viewModelInterface.GetProperties()
                                           .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IReactiveProperty<>))
                                           .ToArray();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine("using UnityEngine.Events;");
        sb.AppendLine();
        sb.AppendLine("public class " + className + " : BaseViewSingle<" + viewModelInterface.Name + ">");
        sb.AppendLine("{");

        // Генерация полей UnityEvent для каждого свойства
        foreach (var property in properties)
        {
            string propertyName = property.Name;
            string propertyType = property.PropertyType.GetGenericArguments()[0].Name;
            sb.AppendLine("    public UnityEvent<" + propertyType + "> on" + propertyName + "Changed;");
        }

        sb.AppendLine();
        sb.AppendLine("    public " + className + "(" + viewModelInterface.Name + " viewModel) : base(viewModel) { }");
        sb.AppendLine();
        sb.AppendLine("    protected override void Initialize()");
        sb.AppendLine("    {");

        // Генерация подписок для каждого свойства
        foreach (var property in properties)
        {
            string propertyName = property.Name;
            sb.AppendLine("        viewModel." + propertyName + ".Subscribe(on" + propertyName + "Changed.Invoke);");
        }

        sb.AppendLine("    }");
        sb.AppendLine("}");

        File.WriteAllText(fileName, sb.ToString());
        Debug.Log("Generated: " + fileName);
    }
}
